using NestedFlowchart.Declaration;
using NestedFlowchart.Functions;
using NestedFlowchart.Models;
using NestedFlowchart.Position;
using System.Windows.Forms;

namespace NestedFlowchart.Rules
{
    public class ArcBaseRule
    {
        public (ArcModel?, ArcModel?, PreviousNode, int, int) CreateArcWithPreviousNode(
            TempArrow arrow, 
            string type,
            PositionManagements position, 
            string arrayName,
            List<PreviousNode> previousNodes, 
            bool isDeclaredI)
        {
            string arcVariable;
            string orientation;
            ArcModel arcModel, arcModel2;


            bool IsUsePreviousFalse = false;
            string outputPortPlaceIdSubPage = string.Empty;
            string outputPortPlaceIdMainPage = string.Empty;

            //หาตัวแรกที่ id ตรงกับ Destination เพื่อเอามาสร้าง Arc ต่อกัน
            var sourceNode = previousNodes.FirstOrDefault(x => x.elementId == arrow.Source);
            var destinationNode = previousNodes.FirstOrDefault(x => x.elementId == arrow.Destination);

            //ถ้า CurrentMainPage ของ destination - source = 1 แสดงว่าออกจาก Subpage ให้ลากใส่ Output port place ด้วย
            if(sourceNode?.CurrentMainPage - destinationNode?.CurrentMainPage > 0)
            {
                //Create arc for output port place
                outputPortPlaceIdMainPage = previousNodes.FirstOrDefault(x => x.CurrentMainPage == sourceNode?.CurrentMainPage &&
                                                  x.outputPortSubPagePlaceModel != null).outputPortMainPagePlaceModel.Id1;

                //Create arc for output port place
                outputPortPlaceIdSubPage = previousNodes.FirstOrDefault(x => x.CurrentMainPage == sourceNode?.CurrentMainPage &&
                                                  x.outputPortSubPagePlaceModel != null).outputPortSubPagePlaceModel.Id1;


                //กรณีอยู่หน้าแรก และยังไม่ประกาศ i ให้ใช้ arc variable array เฉยๆ นอกจากนั้นไป get ตาม page
                arcVariable = isDeclaredI ? DeclareArcVariable(arrayName, destinationNode.CurrentMainPage) : arrayName;

                arcModel = new ArcModel
                {
                    Id1 = IdManagements.GetlastestArcId(),
                    Id2 = IdManagements.GetlastestArcId(),
                    xPos = position.xArcPos,
                    yPos = position.yArcPos == 84 ? position.yArcPos : position.GetLastestyArcPos(),
                    TransEnd = destinationNode?.currentTransitionModel?.Id1,
                    PlaceEnd = outputPortPlaceIdMainPage,
                    Orientation = "PtoT",
                    Type = arcVariable
                };

                arcModel2 = new ArcModel
                {
                    Id1 = IdManagements.GetlastestArcId(),
                    Id2 = IdManagements.GetlastestArcId(),
                    xPos = position.xArcPos,
                    yPos = position.yArcPos == 84 ? position.yArcPos : position.GetLastestyArcPos(),
                    TransEnd = sourceNode.currentFalseTransitionModel.Id1,
                    PlaceEnd = outputPortPlaceIdSubPage,
                    Orientation = "TtoP",
                    Type = arcVariable
                };

                var mainPage = destinationNode.CurrentMainPage;
                var subPage = destinationNode.CurrentSubPage;
                destinationNode.CurrentSubPage--;
                return (arcModel, arcModel2, destinationNode, mainPage, subPage);
            }


            //กรณีอยู่หน้าแรก และยังไม่ประกาศ i ให้ใช้ arc variable array เฉยๆ นอกจากนั้นไป get ตาม page
            arcVariable = isDeclaredI ? DeclareArcVariable(arrayName, destinationNode.CurrentMainPage) : arrayName;

            //กรณีลากใส่ CN2 (False)
            if (arrow.Id.Contains("3K-55") || arrow.Id.Contains("Rj-61") ||
            arrow.Id.Contains("Rj-58") || arrow.Id.Contains("Rj-52") ||
            arrow.Id.Contains("Rj-46") || arrow.Id.Contains("Rj-35") ||
            arrow.Id.Contains("5a-94") /* End NestedLoop */)
            {
                IsUsePreviousFalse = true;
            }
            //กรณีลากไป CN2 (True)
            else if (arrow.Id.Contains("KSG-24"))
            {
                arcVariable = "(i,j,array2)";
            }
            //กรณีลากไป CN1
            else if (arrow.Id.Contains("KSG-17"))
            {
                arcVariable = "(i2,array)";
            }
            //กรณีลากใส่ End
            else if (arrow.Id.Contains("3K-39"))
            {
                IsUsePreviousFalse = true;
                arcVariable = "array";
            }
            //กรณีลากใส่ P5
            else if (arrow.Id.Contains("KSG-26"))
            {
                type = "transition";
                arcVariable = "(i,j2,array)";
            }
            //Nestedif start to T1
            else if (arrow.Id.Contains("SwmT-1"))
            {
                arcVariable = "i";
            }

            if (arrow.Id.Contains("Rj-61") ||
                arrow.Id.Contains("Rj-58") || arrow.Id.Contains("Rj-52") ||
                arrow.Id.Contains("Rj-46") || arrow.Id.Contains("Rj-35") ||
                arrow.Id.Contains("5a-39") || /*NestedLoop T11 to CN5*/
                arrow.Id.Contains("5a-73") ||/*NestedLoop T12 to CN4*/
                arrow.Id.Contains("5a-79") || /*NestedLoop T13 to CN3*/
                arrow.Id.Contains("5a-85") /*NestedLoop T14 to CN2*/)
            {
                type = "transition";
            }

            if (arrow.Id.Contains("Rj-71"))
            {
                type = "place";
            }

            //ถ้าเป็น place ให้ใช้ PtoT, ถ้าเป็น transition ให้ใช้ TtoP
            orientation = (type == "place") ? "PtoT" : "TtoP";

            arcModel = new ArcModel
            {
                Id1 = IdManagements.GetlastestArcId(),
                Id2 = IdManagements.GetlastestArcId(),
                xPos = position.xArcPos,
                yPos = position.yArcPos == 84 ? position.yArcPos : position.GetLastestyArcPos(),
                Orientation = orientation,
                Type = arcVariable
            };

            if (type == "place")
            {
                arcModel.PlaceEnd = sourceNode?.currentPlaceModel?.Id1;
                arcModel.TransEnd = destinationNode?.currentTransitionModel?.Id1;
            }
            else
            {
                arcModel.TransEnd = IsUsePreviousFalse ? 
                    sourceNode.currentFalseTransitionModel.Id1 :
                    sourceNode?.currentTransitionModel?.Id1;

                arcModel.PlaceEnd = !string.IsNullOrEmpty(outputPortPlaceIdSubPage) ? 
                    outputPortPlaceIdSubPage : 
                    destinationNode?.currentPlaceModel?.Id1;
            }

            return (arcModel, null, destinationNode, destinationNode.CurrentMainPage, destinationNode.CurrentSubPage);
        }

        //new
        public string GetArcVariableByPageAndType(string arrayName, int page, int type)
        {
            if(type == (int)eDeclareType.IsArray)
            {
                return page switch
                {
                    0 => $"(i,{arrayName})",
                    1 => $"(i,j,{arrayName})",
                    2 => "(i,j,k)",
                    3 => "(i,j,k,l)",
                    4 => "(i,j,k,l,m)",
                    _ => string.Empty
                };
            }
            else if(type == (int)eDeclareType.IsNone)
            {
                return page switch
                {
                    0 => "i",
                    1 => "(i,j)",
                    2 => "(i,j,k)",
                    3 => "(i,j,k,l)",
                    4 => "(i,j,k,l,m)",
                    _ => string.Empty
                };
            }
            else if(type == (int)eDeclareType.IsInteger)
            {
                return arrayName;
            }

            return string.Empty;
            
        }

        public string GetArcVariableOnlyOne(int page)
        {
            switch (page)
            {
                case 0:
                    return "i";
                case 1:
                    return "j";
                case 2:
                    return "k";
                case 3:
                    return "l";
                case 4:
                    return "m";
                default:
                    return string.Empty;
            }
        }




        //old
        public string DeclareArcVariable(string arrayName, int countSubPage)
        {
            //arc variable
            string arcVariable = string.Empty;

            if (arrayName.Contains("array"))
            {
                switch (countSubPage)
                {
                    case 0:
                        arcVariable = $"(i,{arrayName})";
                        break;
                    case 1:
                        arcVariable = $"(i,j,{arrayName})";
                        break;
                }
            }
            else
            {
                switch (countSubPage)
                {
                    case 0:
                        arcVariable = $"{arrayName}";
                        break;
                }
            }


            return arcVariable;
        }

    }
}
