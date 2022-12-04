using NestedFlowchart.Functions;
using NestedFlowchart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NestedFlowchart.Rules
{
    public class Rule7
    {
        /// <summary>
        /// Transform end into place or transition, place connected by arc
        /// </summary>
        /// <param name="placeTemplate"></param>
        /// <param name="arcTemplate"></param>
        /// <param name="previousNode"></param>
        /// <returns></returns>
        public (PlaceModel, string) ApplyRule(string placeTemplate, string arcTemplate,
            PreviousNode previousNode)
        {
            if (previousNode.Type == "place")
            {
                //TODO: Create Transition
            }

            //End Place
            PlaceModel pl = new PlaceModel()
            {
                Id1 = IdManagements.GetlastestPlaceId(),
                Id2 = IdManagements.GetlastestPlaceId(),
                Id3 = IdManagements.GetlastestPlaceId(),
                Name = "End",
                Type = "INTs",
                InitialMarking = string.Empty,

                xPos1 = previousNode.previousPlaceModel.xPos1 - 4,
                yPos1 = previousNode.previousPlaceModel.yPos1 - 168,

                xPos2 = previousNode.previousPlaceModel.xPos2 - 4,
                yPos2 = previousNode.previousPlaceModel.yPos2 - 167,

                xPos3 = previousNode.previousPlaceModel.xPos3 - 4,
                yPos3 = previousNode.previousPlaceModel.yPos3 - 167,

            };

            //TODO: find solution to create Arc
            if (previousNode.Type == "place")
            {
                //TODO: Connect Arc to pl
            }

            //Arc from GF1 to End
            ArcModel a1 = new ArcModel()
            {
                Id1 = IdManagements.GetlastestArcId(),
                Id2 = IdManagements.GetlastestArcId(),

                TransEnd = previousNode.previousTransitionModel.Id1,
                PlaceEnd = pl.Id1,

                xPos = PositionManagements.GetLastestxArcPos(),
                yPos = PositionManagements.GetLastestyArcPos(),

                Orientation = "TtoP", //Transition to Place
                Type = "arr"
            };

            TransformationApproach approach = new TransformationApproach();
            var place1 = approach.CreatePlace(placeTemplate, pl);
            var arc1 = approach.CreateArc(arcTemplate, a1);
            return (pl, place1 + arc1);
        }
    }
}
