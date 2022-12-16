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
        public (PlaceModel, TransitionModel, TransitionModel, ArcModel, string) ApplyRule(
            string transitionTemplate, 
            string placeTemplate, 
            string arcTemplate,
            PreviousNode previousNode, 
            string trueCondition, 
            string falseCondition,
            string arrayName)
        {

            var xPos1 = PositionManagements.xPos1;
            var yPos1 = PositionManagements.GetLastestyPos1();

            var xPosArc = PositionManagements.GetLastestxArcPos();
            var yPosArc = PositionManagements.GetLastestyArcPos();

            //GF1 Transition
            TransitionModel tr1 = new TransitionModel()
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
            TransitionModel tr2 = new TransitionModel()
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

                TransEnd = tr1.Id1,
                PlaceEnd = previousNode.previousPlaceModel.Id1,

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

                TransEnd = tr2.Id1,
                PlaceEnd = previousNode.previousPlaceModel.Id1,

                xPos = xPosArc + 34,
                yPos = yPosArc,

                Orientation = "PtoT", //Place to Transition
                Type = $"(i,{arrayName})"
            };

            TransformationApproach approach = new TransformationApproach();
            var transition1 = approach.CreateTransition(transitionTemplate, tr1);
            var transition2 = approach.CreateTransition(transitionTemplate, tr2);

            var arc1 = approach.CreateArc(arcTemplate, a1);
            var arc2 = approach.CreateArc(arcTemplate, a2);

            //Tr1 = GF
            //Tr2 = GT
            return (previousNode.previousPlaceModel, tr1, tr2, a2, transition1 + transition2 + arc1 + arc2);
        }
    }
}
