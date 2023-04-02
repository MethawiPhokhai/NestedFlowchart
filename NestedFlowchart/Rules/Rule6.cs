using NestedFlowchart.Functions;
using NestedFlowchart.Models;
using NestedFlowchart.Position;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NestedFlowchart.Rules
{
    public class Rule6
    {
        /// <summary>
        /// Transform dicision into place and transition connected by arc
        /// </summary>
        /// <param name="transitionTemplate"></param>
        /// <param name="placeTemplate"></param>
        /// <param name="arcTemplate"></param>
        /// <param name="previousNode"></param>
        /// <param name="trueCondition"></param>
        /// <param name="falseCondition"></param>
        /// <returns></returns>
        public (PlaceModel, PlaceModel, TransitionModel, TransitionModel, ArcModel) ApplyRule(
            PreviousNode previousNode, 
            string trueCondition, 
            string falseCondition,
            string arrayName,
            PositionManagements position)
        {

            string arcVariable = DeclareArcVariable(arrayName, previousNode.CurrentMainPage);

            var xPos1 = position.xPos1;
            var yPos1 = position.GetLastestyPos1();

            var xPosArc = position.GetLastestxArcPos();
            var yPosArc = position.GetLastestyArcPos();

            PlaceModel? ps3 = null;
            ArcModel a1 = null;
            if (previousNode.IsPreviousNodeCondition)
            {
                ps3 = new PlaceModel()
                {
                    Id1 = IdManagements.GetlastestPlaceId(),
                    Id2 = IdManagements.GetlastestPlaceId(),
                    Id3 = IdManagements.GetlastestPlaceId(),

                    Name = IdManagements.GetlastestPlaceName(),

                    xPos1 = position.xPos1 + 39,
                    yPos1 = position.GetLastestyPos1() + 100,

                    xPos2 = position.GetLastestxPos2(),
                    yPos2 = position.GetLastestyPos2() + 190,

                    Type = "loopj"
                };

                // Adjust yPos values because a new place was added above
                yPos1 -= 80;
                yPosArc -= 60;
            }

            //TODO: Create False transition later
            ////GF1 Transition
            //TransitionModel falseTransition = new TransitionModel()
            //{
            //    Id1 = IdManagements.GetlastestTransitionId(),
            //    Id2 = IdManagements.GetlastestTransitionId(),
            //    Id3 = IdManagements.GetlastestTransitionId(),
            //    Id4 = IdManagements.GetlastestTransitionId(),
            //    Id5 = IdManagements.GetlastestTransitionId(),

            //    Name = IdManagements.GetlastestFalseGuardTransitionName(),

            //    xPos1 = xPos1 - 39,
            //    yPos1 = yPos1,

            //    xPos2 = xPos1 - 80,
            //    yPos2 = yPos1 + 30,

            //    Condition = falseCondition

            //};

            //GT1 Transition
            TransitionModel trueTransition = new TransitionModel()
            {
                Id1 = IdManagements.GetlastestTransitionId(),
                Id2 = IdManagements.GetlastestTransitionId(),
                Id3 = IdManagements.GetlastestTransitionId(),
                Id4 = IdManagements.GetlastestTransitionId(),
                Id5 = IdManagements.GetlastestTransitionId(),

                Name = IdManagements.GetlastestTrueGuardTransitionName(),

                xPos1 = xPos1 + 39,
                yPos1 = yPos1,

                xPos2 = xPos1 + 80,
                yPos2 = yPos1 + 30,

                Condition = trueCondition
            };

            if (previousNode.IsPreviousNodeCondition)
            {
                //Arc from CN1 to GT1
                a1 = new ArcModel()
                {
                    Id1 = IdManagements.GetlastestArcId(),
                    Id2 = IdManagements.GetlastestArcId(),

                    TransEnd = trueTransition.Id1,
                    PlaceEnd = ps3.Id1,

                    xPos = xPosArc + 34,
                    yPos = yPosArc,

                    Orientation = "PtoT", //Place to Transition
                    Type = arcVariable
                };
            }

            return (previousNode.currentPlaceModel, ps3, null, trueTransition, a1);
        }

        private string DeclareArcVariable(string arrayName, int countSubPage)
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

        public string CreateTrueCondition(string condition, string arrayName)
        {
            if (condition.Contains("["))
            {
                var index = condition.IndexOf("[");

                //Replace array[j with List.nth(array,j
                condition = condition.Replace(arrayName + "[" + condition[index + 1], "List.nth(" + arrayName + "," + condition[index + 1]);
                
                //Replace ] with )
                condition = condition.Replace("]", ")");
            }

            return "[" + condition.Replace("N", $" length {arrayName}") + "]";
        }

        public string CreateFalseDecision(string condition)
        {
            if (condition.Contains("&gt;"))
            {
                return condition.Replace("&gt;", "&lt;=");
            }
            else if (condition.Contains("&lt;"))
            {
                return condition.Replace("&lt;", "&gt;=");
            }
            else if (condition.Contains("="))
            {
                return condition.Replace("=", "!=");
            }
            else if (condition.Contains("!="))
            {
                return condition.Replace("!=", "=");
            }
            else
            {
                return condition;
            }
        }
    }
}
