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

                return (arcModel, arcModel2, destinationNode, 0, 1);
            }


            //กรณีอยู่หน้าแรก และยังไม่ประกาศ i ให้ใช้ arc variable array เฉยๆ นอกจากนั้นไป get ตาม page
            arcVariable = isDeclaredI ? DeclareArcVariable(arrayName, destinationNode.CurrentMainPage) : arrayName;

            //กรณีลากใส่ CN2 (False)
            if (arrow.Id.Contains("3K-55"))
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

        public string DeclareArcVariable(string arrayName, int countSubPage)
        {
            //arc variable
            string arcVariable = string.Empty;
            switch (countSubPage)
            {
                case 0:
                    arcVariable = $"(i,{arrayName})";
                    break;
                case 1:
                    arcVariable = $"(i,j,{arrayName})";
                    break;
            }

            return arcVariable;
        }

    }
}
