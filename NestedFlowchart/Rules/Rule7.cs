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
    public class Rule7
    {
        /// <summary>
        /// Transform end into place or transition, place connected by arc
        /// If previous node is place => Create Transition and then create place connected by arc
        /// If previous node is transition => Create place connected by arc
        /// </summary>
        /// <param name="placeTemplate"></param>
        /// <param name="arcTemplate"></param>
        /// <param name="previousNode"></param>
        /// <returns></returns>
        public (PlaceModel, TransitionModel, ArcModel) ApplyRule(
            string arrayName,
            PreviousNode previousNode,
            PositionManagements position)
        {
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

            if(previousNode.Type == "transition")
            {
                //Arc from GF1 to End
                ArcModel a1 = new ArcModel()
                {
                    Id1 = IdManagements.GetlastestArcId(),
                    Id2 = IdManagements.GetlastestArcId(),

                    TransEnd = previousNode.previousTransitionModel.Id1,
                    PlaceEnd = pl.Id1,

                    xPos = position.GetLastestxArcPos(),
                    yPos = position.GetLastestyArcPos(),

                    Orientation = "TtoP", //Transition to Place
                    Type = arrayName
                };

                return (pl, null, a1);
            }
            else
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
        }
    }
}
