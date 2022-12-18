using NestedFlowchart.Functions;
using NestedFlowchart.Models;
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
        public (PlaceModel, TransitionModel, TransitionModel, ArcModel, ArcModel) ApplyRule(
            PlaceModel previousPlace, 
            string trueCondition, 
            string falseCondition,
            string arrayName)
        {
            var xPos1 = PositionManagements.xPos1;
            var yPos1 = PositionManagements.GetLastestyPos1();

            var xPosArc = PositionManagements.GetLastestxArcPos();
            var yPosArc = PositionManagements.GetLastestyArcPos();

            //GF1 Transition
            TransitionModel trueTransition = new TransitionModel()
            {
                Id1 = IdManagements.GetlastestTransitionId(),
                Id2 = IdManagements.GetlastestTransitionId(),
                Id3 = IdManagements.GetlastestTransitionId(),
                Id4 = IdManagements.GetlastestTransitionId(),
                Id5 = IdManagements.GetlastestTransitionId(),

                Name = IdManagements.GetlastestFalseGuardTransitionName(),

                xPos1 = xPos1 - 39,
                yPos1 = yPos1,

                xPos2 = xPos1 - 80,
                yPos2 = yPos1 + 30,

                Condition = falseCondition

            };

            //GT1 Transition
            TransitionModel falseTransition = new TransitionModel()
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

            //Arc from CN1 to GF1
            ArcModel a1 = new ArcModel()
            {
                Id1 = IdManagements.GetlastestArcId(),
                Id2 = IdManagements.GetlastestArcId(),

                TransEnd = trueTransition.Id1,
                PlaceEnd = previousPlace.Id1,

                xPos = xPosArc - 84,
                yPos = yPosArc,

                Orientation = "PtoT", //Place to Transition
                Type = $"(i,{arrayName})"
            };

            //Arc from CN1 to GT1
            ArcModel a2 = new ArcModel()
            {
                Id1 = IdManagements.GetlastestArcId(),
                Id2 = IdManagements.GetlastestArcId(),

                TransEnd = falseTransition.Id1,
                PlaceEnd = previousPlace.Id1,

                xPos = xPosArc + 34,
                yPos = yPosArc,

                Orientation = "PtoT", //Place to Transition
                Type = $"(i,{arrayName})"
            };

            return (previousPlace, trueTransition, falseTransition, a1, a2);
        }

        public string CreateTrueCondition(string condition, string arrayName)
        {
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
