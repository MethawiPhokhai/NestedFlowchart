using NestedFlowchart.Functions;
using NestedFlowchart.Models;
using NestedFlowchart.Position;

namespace NestedFlowchart.Rules
{
    public class Rule7 : ArcBaseRule
    {
        /// <summary>
        /// Transform end into place or transition, place connected by arc
        /// If previous node is place => Create Transition and then create place connected by arc
        /// If previous node is transition => Create place connected by arc
        /// </summary>
        /// <param name="placeTemplate"></param>
        /// <returns></returns>
        public PlaceModel ApplyRule(
            string arrayName,
            PositionManagements position,
            PreviousNode pv)
        {
            //End Place
            PlaceModel pl = new PlaceModel()
            {
                Id1 = IdManagements.GetlastestPlaceId(),
                Id2 = IdManagements.GetlastestPlaceId(),
                Id3 = IdManagements.GetlastestPlaceId(),
                Name = "End",
                Type = (arrayName == "array") ? "INTs" : "aa",
                InitialMarking = string.Empty,

                xPos1 = position.xPos1 - 4,
                yPos1 = position.yPos1 - 168,

                xPos2 = position.xPos2 - 4,
                yPos2 = position.yPos2 - 167,

                xPos3 = position.xPos3 - 4,
                yPos3 = position.yPos3 - 167
            };

            return pl;

        }
    }
}

