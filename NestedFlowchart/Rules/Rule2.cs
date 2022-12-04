using NestedFlowchart.Functions;
using NestedFlowchart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NestedFlowchart.Rules
{
    /// <summary>
    /// Transform Initialize Process into transition and place connected by arc
    /// </summary>
    public class Rule2
    {
        public (PlaceModel, TransitionModel, ArcModel, string) ApplyRule(string transitionTemplate, string placeTemplate, string arcTemplate, PlaceModel placeRule1)
        {
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

            };

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

                Type = "INTs"
            };

            ArcModel a1 = new ArcModel()
            {
                Id1 = IdManagements.GetlastestArcId(),
                Id2 = IdManagements.GetlastestArcId(),

                TransEnd = tr.Id1,
                PlaceEnd = placeRule1.Id1,

                xPos = PositionManagements.xArcPos,
                yPos = PositionManagements.yArcPos,

                Orientation = "PtoT", //Place to Transition
                Type = "arr"
            };

            ArcModel a2 = new ArcModel()
            {
                Id1 = IdManagements.GetlastestArcId(),
                Id2 = IdManagements.GetlastestArcId(),

                TransEnd = tr.Id1,
                PlaceEnd = pl.Id1,

                xPos = PositionManagements.GetLastestxArcPos(),
                yPos = PositionManagements.GetLastestyArcPos(),

                Orientation = "TtoP", //Transition to Place
                Type = "arr"
            };

            TransformationApproach approach = new TransformationApproach();
            var place1 = approach.CreatePlace(placeTemplate, placeRule1);
            var arc1 = approach.CreateArc(arcTemplate, a1);
            var transition = approach.CreateTransition(transitionTemplate, tr);
            var arc2 = approach.CreateArc(arcTemplate, a2);
            var place2 = approach.CreatePlace(placeTemplate, pl);

            var allNode = place1 + place2 + transition + arc1 + arc2;
            return (pl, tr, a2, allNode);

        }
    }
}
