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
    public class Rule4
    {
        /// <summary>
        /// Transform simple process into place and transition connected by arc
        /// </summary>
        /// <param name="transitionTemplate"></param>
        /// <param name="placeTemplate"></param>
        /// <param name="arcTemplate"></param>
        /// <param name="previousNode"></param>
        /// <returns></returns>
        public (PlaceModel, TransitionModel, ArcModel, string) ApplyRule(
            string transitionTemplate, 
            string placeTemplate, 
            string arcTemplate,
            string arrayName,
            PreviousNode previousNode)
        {
            //T4 Transition
            TransitionModel tr = new TransitionModel()
            {
                Id1 = IdManagements.GetlastestTransitionId(),
                Id2 = IdManagements.GetlastestTransitionId(),
                Id3 = IdManagements.GetlastestTransitionId(),
                Id4 = IdManagements.GetlastestTransitionId(),
                Id5 = IdManagements.GetlastestTransitionId(),

                Name = IdManagements.GetlastestTransitionName(),

                xPos1 = PositionManagements.xPos1,
                yPos1 = PositionManagements.GetLastestyPos1(),

                xPos4 = PositionManagements.GetLastestxPos4(),
                yPos4 = PositionManagements.GetLastestyPos4(),
            };

            //P4
            PlaceModel pl = new PlaceModel()
            {
                Id1 = IdManagements.GetlastestPlaceId(),
                Id2 = IdManagements.GetlastestPlaceId(),
                Id3 = IdManagements.GetlastestPlaceId(),

                Name = IdManagements.GetlastestPlaceName(),

                xPos1 = PositionManagements.xPos1,
                yPos1 = PositionManagements.GetLastestyPos1(),

                xPos2 = PositionManagements.GetLastestxPos2(),
                yPos2 = PositionManagements.GetLastestyPos2(),

                Type = "loopi"
            };

            //Arc from P2 to T3
            ArcModel a1 = new ArcModel()
            {
                Id1 = IdManagements.GetlastestArcId(),
                Id2 = IdManagements.GetlastestArcId(),

                TransEnd = tr.Id1,
                PlaceEnd = pl.Id1,

                xPos = PositionManagements.GetLastestxArcPos(),
                yPos = PositionManagements.GetLastestyArcPos(),

                Orientation = "TtoP", //Transition to Place
                Type = $"(i,{arrayName})"
            };

            TransformationApproach approach = new TransformationApproach();
            var place1 = approach.CreatePlace(placeTemplate, pl);
            var arc1 = approach.CreateArc(arcTemplate, a1);
            var transition = approach.CreateTransition(transitionTemplate, tr);

            var allNode = place1 + transition + arc1;
            return (pl, tr, a1, allNode);
        }
    }
}
