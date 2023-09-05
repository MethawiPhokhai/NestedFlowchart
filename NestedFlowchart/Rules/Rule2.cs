using NestedFlowchart.Declaration;
using NestedFlowchart.Functions;
using NestedFlowchart.Models;
using NestedFlowchart.Position;
using System.Xml.Linq;

namespace NestedFlowchart.Rules
{
    /// <summary>
    /// Transform Initialize Process into transition and place connected by arc
    /// </summary>
    public class Rule2 : ArcBaseRule
    {
        public (PlaceModel, TransitionModel, ArcModel)
            ApplyRule(
            string arrayName,
            string cSeg,
            PositionManagements position)
        {
            TransitionModel tr = new TransitionModel()
            {
                Id1 = IdManagements.GetlastestTransitionId(),
                Id2 = IdManagements.GetlastestTransitionId(),
                Id3 = IdManagements.GetlastestTransitionId(),
                Id4 = IdManagements.GetlastestTransitionId(),
                Id5 = IdManagements.GetlastestTransitionId(),

                Name = IdManagements.GetlastestTransitionName(),

                xPos1 = position.xPos1,
                yPos1 = position.GetLastestyPos1(),
                CodeSegment = cSeg
            };

            PlaceModel pl = new PlaceModel()
            {
                Id1 = IdManagements.GetlastestPlaceId(),
                Id2 = IdManagements.GetlastestPlaceId(),
                Id3 = IdManagements.GetlastestPlaceId(),

                Name = IdManagements.GetlastestPlaceName(),

                xPos1 = position.xPos1,
                yPos1 = position.GetLastestyPos1(),

                xPos2 = position.GetLastestxPos2(),
                yPos2 = position.GetLastestyPos2(),

                Type = (arrayName == "array") ? "INTs" : "aa"
            };

            ArcModel a1 = new ArcModel()
            {
                Id1 = IdManagements.GetlastestArcId(),
                Id2 = IdManagements.GetlastestArcId(),

                TransEnd = tr.Id1,
                PlaceEnd = pl.Id1,

                xPos = position.GetLastestxArcPos(),
                yPos = position.GetLastestyArcPos(),

                Orientation = "TtoP", //Transition to Place
                Type = arrayName
            };

            return (pl, tr, a1);
        }

        public (string, string, int, int) AssignInitialMarking(
            List<XMLCellNode> sortedFlowcharts, 
            string arcVariable,
            PreviousNode previousNode, 
            int i)
        {
            string cSeg = string.Empty;
            int declareType, variableCount = 0;

            if (sortedFlowcharts[i].ValueText.Contains('[')) //กรณีเป็น Array
            {
                arcVariable = SubstringBefore(sortedFlowcharts[i].ValueText.Trim(), '=').Trim();
                var arrayValue = SubstringAfter(sortedFlowcharts[i].ValueText.Trim(), '=');

                previousNode.currentPlaceModel.Type = "INTs";
                previousNode.currentPlaceModel.InitialMarking = arrayValue;

                declareType = (int)eDeclareType.IsArray;
            }
            else if (sortedFlowcharts[i].ValueText.ToLower().Trim().Contains("i ="))
            {
                previousNode.currentPlaceModel.Type = "INT";
                previousNode.currentPlaceModel.InitialMarking = "1";
                arcVariable = "x"; //สร้างเป็นตัวแปร INT

                declareType = (int)eDeclareType.IsNone;
            }
            else //กรณีตัวแปรปกตื
            {
                string arrayValue = string.Empty;
                var arrayValues = sortedFlowcharts[i].ValueText.Split("<br>");
                variableCount = arrayValues.Length;

                foreach ( var val in arrayValues )
                {
                    arrayValue += SubstringBefore(val, '=').Trim();
                    arrayValue += ",";
                }

                var arcVar = string.Format("({0})", arrayValue.Remove(arrayValue.Length - 1));


                previousNode.currentPlaceModel.Type = "INT";
                previousNode.currentPlaceModel.InitialMarking = "1";
                arcVariable = arcVar; //For arc
                cSeg = string.Format("input (); \r\n " +
                    "output{1}; \r\n " +
                    "action\r\n " +
                    "let \r\n {0} \r\n" +
                    "in \r\n" +
                    "{2} \r\n" +
                    "end", "val " + sortedFlowcharts[i].ValueText.Replace("<br>", "\r\nval "), arcVar, arcVar);

                declareType = (int)eDeclareType.IsInteger;
            }

            return (arcVariable, cSeg, declareType, variableCount);
        }

        private string SubstringBefore(string str, char ch)
        {
            int index = str.IndexOf(ch);
            return index >= 0 ? str.Substring(0, index) : str;
        }

        private string SubstringAfter(string str, char ch)
        {
            int index = str.IndexOf(ch);
            return index >= 0 ? str.Substring(index + 1) : string.Empty;
        }
    }
}