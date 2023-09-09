using NestedFlowchart.Declaration;
using NestedFlowchart.Functions;
using NestedFlowchart.Models;
using NestedFlowchart.Position;
using System.Configuration;

namespace NestedFlowchart.Rules
{
    public class ArcBaseRule
    {
        public (ArcModel?, ArcModel?, PreviousNode, int, int) CreateArcWithPreviousNode(
            TempArrow arrow,
            string elementType,
            PositionManagements position,
            string arrayName,
            List<PreviousNode> previousNodes,
            bool isDeclaredI,
            int type)
        {
            #region Variable
            string arcVariable;
            ArcModel arcModel, arcModel2;

            bool IsUsePreviousFalse = false;
            string outputPortPlaceIdSubPage = string.Empty;
            string outputPortPlaceIdMainPage = string.Empty;
            #endregion

            //หาตัวแรกที่ id ตรงกับ Destination เพื่อเอามาสร้าง Arc ต่อกัน
            var sourceNode = previousNodes.FirstOrDefault(x => x.elementId == arrow.Source);
            var destinationNode = previousNodes.FirstOrDefault(x => x.elementId == arrow.Destination);

            //ถ้า CurrentMainPage ของ destination - source = 1 แสดงว่าออกจาก Subpage ให้ลากใส่ Output port place ด้วย
            if (sourceNode?.CurrentMainPage - destinationNode?.CurrentMainPage > 0)
            {
                CreateArcToOutputPortPlace(position, arrayName, previousNodes, isDeclaredI, type, out arcVariable, out arcModel, out arcModel2, out outputPortPlaceIdSubPage, out outputPortPlaceIdMainPage, sourceNode, destinationNode);

                var mainPage = destinationNode.CurrentMainPage;
                var subPage = destinationNode.CurrentSubPage;
                destinationNode.CurrentSubPage--;
                return (arcModel, arcModel2, destinationNode, mainPage, subPage);
            }

            //กรณีอยู่หน้าแรก และยังไม่ประกาศ i ให้ใช้ arc variable array เฉยๆ นอกจากนั้นไป get ตาม page
            arcVariable = isDeclaredI ? GetArcVariableByPageAndType(arrayName, destinationNode.CurrentMainPage, type) : arrayName;

            //When have increment, use another arc variable
            arcVariable = GetArcVariableAfterIncrement(arcVariable, sourceNode?.currentTransitionModel?.CodeSegment);

            LimitationCondition(arrow, ref elementType, ref arcVariable, ref IsUsePreviousFalse);

            //if sourceNode เป็น start และไม่มี array ให้ใช้ arc variable เป็น z
            if(sourceNode.currentPlaceModel?.Name.ToLower() == "start" && type != (int)eDeclareType.IsArray)
            {
                arcVariable = "z";
            }

            //if destinationNode เป็น end และเป็น array ให้ใช้ arc variable เป็น array
            if(destinationNode.currentPlaceModel?.Name.ToLower() == "end" && type == (int)eDeclareType.IsArray)
            {
                IsUsePreviousFalse = true;
                arcVariable = "array";
            }

            //ถ้าเป็น place ให้ใช้ PtoT, ถ้าเป็น transition ให้ใช้ TtoP
            var orientation = (elementType == "place") ? "PtoT" : "TtoP";

            arcModel = new ArcModel
            {
                Id1 = IdManagements.GetlastestArcId(),
                Id2 = IdManagements.GetlastestArcId(),
                xPos = position.xArcPos,
                yPos = position.yArcPos == 84 ? position.yArcPos : position.GetLastestyArcPos(),
                Orientation = orientation,
                Type = arcVariable
            };

            var currentSourceTransitionId = (sourceNode?.outputPreviousTransition?.Id1 != null) 
                ? sourceNode?.outputPreviousTransition?.Id1
                : sourceNode?.currentTransitionModel?.Id1;

            if (elementType == "place")
            {
                SetPlaceEndAndTransEnd(
                    arcModel,
                    sourceNode?.currentPlaceModel?.Id1,
                    destinationNode?.currentTransitionModel?.Id1
                );
            }
            else
            {
                SetPlaceEndAndTransEnd(
                    arcModel,
                    !string.IsNullOrEmpty(outputPortPlaceIdSubPage) ? outputPortPlaceIdSubPage : destinationNode?.currentPlaceModel?.Id1,
                    IsUsePreviousFalse ? sourceNode.currentFalseTransitionModel.Id1 : currentSourceTransitionId
                );
            }

            return (arcModel, null, destinationNode, destinationNode.CurrentMainPage, destinationNode.CurrentSubPage);
        }

        public string GetArcVariableByPageAndType(string arrayName, int page, int type)
        {
            #region Loop variables
            var loop1 = ConfigurationManager.AppSettings["loop1"]?.ToString() ?? "loop1";
            var loop2 = ConfigurationManager.AppSettings["loop2"]?.ToString() ?? "loop2";
            var loop3 = ConfigurationManager.AppSettings["loop3"]?.ToString() ?? "loop3";
            var loop4 = ConfigurationManager.AppSettings["loop4"]?.ToString() ?? "loop4";
            var loop5 = ConfigurationManager.AppSettings["loop5"]?.ToString() ?? "loop5";
            #endregion

            if (type == (int)eDeclareType.IsArray)
            {
                return page switch
                {
                    0 => $"({loop1},{arrayName})",
                    1 => $"({loop1},{loop2},{arrayName})",
                    2 => $"({loop1},{loop2},{loop3},{arrayName})",
                    3 => $"({loop1},{loop2},{loop3},{loop4},{arrayName})",
                    4 => $"({loop1},{loop2},{loop3},{loop4},{loop5},{arrayName})",
                    _ => string.Empty
                };
            }
            else if (type == (int)eDeclareType.IsNone)
            {
                return page switch
                {
                    0 => $"{loop1}",
                    1 => $"({loop1},{loop2})",
                    2 => $"({loop1},{loop2},{loop3})",
                    3 => $"({loop1},{loop2},{loop3},{loop4})",
                    4 => $"({loop1},{loop2},{loop3},{loop4},{loop5})",
                    _ => string.Empty
                };
            }
            else if (type == (int)eDeclareType.IsInteger)
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
                    return ConfigurationManager.AppSettings["loop1"]?.ToString() ?? "loop1";

                case 1:
                    return ConfigurationManager.AppSettings["loop2"]?.ToString() ?? "loop2";

                case 2:
                    return ConfigurationManager.AppSettings["loop3"]?.ToString() ?? "loop3";

                case 3:
                    return ConfigurationManager.AppSettings["loop4"]?.ToString() ?? "loop4";

                case 4:
                    return ConfigurationManager.AppSettings["loop5"]?.ToString() ?? "loop5";

                default:
                    return string.Empty;
            }
        }

        public string GetArcVariableAfterIncrement(string arrayName, string incVar)
        {
            #region Loop variables
            var loop1 = ConfigurationManager.AppSettings["loop1"]?.ToString() ?? "loop1";
            var loop2 = ConfigurationManager.AppSettings["loop2"]?.ToString() ?? "loop2";
            var loop3 = ConfigurationManager.AppSettings["loop3"]?.ToString() ?? "loop3";
            var loop4 = ConfigurationManager.AppSettings["loop4"]?.ToString() ?? "loop4";
            var loop5 = ConfigurationManager.AppSettings["loop5"]?.ToString() ?? "loop5";
            #endregion

            if (incVar != null)
            {
                return incVar switch
                {
                    var str when str.Contains($"{loop1}2") => arrayName.Replace(loop1, $"{loop1}2"),
                    var str when str.Contains($"{loop2}2") => arrayName.Replace(loop2, $"{loop2}2"),
                    var str when str.Contains($"{loop3}2") => arrayName.Replace(loop3, $"{loop3}2"),
                    var str when str.Contains($"{loop4}2") => arrayName.Replace(loop4, $"{loop4}2"),
                    var str when str.Contains($"{loop5}2") => arrayName.Replace(loop5, $"{loop5}2"),
                    var str when str.Contains("array2") => arrayName.Replace("array", "array2"),
                    _ => arrayName,
                };
            }

            return arrayName;
        }

        private void CreateArcToOutputPortPlace(PositionManagements position, string arrayName, List<PreviousNode> previousNodes, bool isDeclaredI, int type, out string arcVariable, out ArcModel arcModel, out ArcModel arcModel2, out string outputPortPlaceIdSubPage, out string outputPortPlaceIdMainPage, PreviousNode? sourceNode, PreviousNode? destinationNode)
        {
            //Create arc for output port place
            outputPortPlaceIdMainPage = previousNodes.FirstOrDefault(x => x.CurrentMainPage == sourceNode?.CurrentMainPage &&
                                              x.outputPortSubPagePlaceModel != null).outputPortMainPagePlaceModel.Id1;

            //Create arc for output port place
            outputPortPlaceIdSubPage = previousNodes.FirstOrDefault(x => x.CurrentMainPage == sourceNode?.CurrentMainPage &&
                                              x.outputPortSubPagePlaceModel != null).outputPortSubPagePlaceModel.Id1;

            //กรณีอยู่หน้าแรก และยังไม่ประกาศ i ให้ใช้ arc variable array เฉยๆ นอกจากนั้นไป get ตาม page
            arcVariable = isDeclaredI ? GetArcVariableByPageAndType(arrayName, destinationNode.CurrentMainPage, type) : arrayName;

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
        }

        private void LimitationCondition(TempArrow arrow, ref string elementType, ref string arcVariable, ref bool IsUsePreviousFalse)
        {
            //กรณีลากใส่ CN2 (False)
            if (arrow.Id.Contains("3K-55") || arrow.Id.Contains("Rj-61") ||
            arrow.Id.Contains("Rj-58") || arrow.Id.Contains("Rj-52") ||
            arrow.Id.Contains("Rj-46") || arrow.Id.Contains("Rj-35") ||
            arrow.Id.Contains("5a-94") /* End NestedLoop */)
            {
                IsUsePreviousFalse = true;
            }

            if (arrow.Id.Contains("KSG-26") || //Bubble sort into P5
                arrow.Id.Contains("Rj-61") ||
                arrow.Id.Contains("Rj-58") || arrow.Id.Contains("Rj-52") ||
                arrow.Id.Contains("Rj-46") || arrow.Id.Contains("Rj-35") ||
                arrow.Id.Contains("5a-39") || /*NestedLoop T11 to CN5*/
                arrow.Id.Contains("5a-73") ||/*NestedLoop T12 to CN4*/
                arrow.Id.Contains("5a-79") || /*NestedLoop T13 to CN3*/
                arrow.Id.Contains("5a-85") /*NestedLoop T14 to CN2*/)
            {
                elementType = "transition";
            }

            //Nested-If P6 to T3
            if (arrow.Id.Contains("Rj-71"))
            {
                elementType = "place";
            }
        }

        private void SetPlaceEndAndTransEnd(ArcModel arcModel, string placeEndId, string transEndId)
        {
            arcModel.PlaceEnd = placeEndId;
            arcModel.TransEnd = transEndId;
        }
    }
}