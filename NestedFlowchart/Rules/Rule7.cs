using NestedFlowchart.Functions;
using NestedFlowchart.Models;
using NestedFlowchart.Position;

namespace NestedFlowchart.Rules
{
    public class Rule7 : ArcBaseRule
    {
        private readonly ITypeBaseRule _typeBaseRule;

        public Rule7()
        {
            _typeBaseRule = new TypeBaseRule();
        }

        /// <summary>
        /// Transform end into place or transition, place connected by arc
        /// </summary>
        /// <returns></returns>
        public PlaceModel ApplyRule(
            PositionManagements position,
            int type,
            int page)
        {
            //End Place
            PlaceModel pl = new PlaceModel()
            {
                Id1 = IdManagements.GetlastestPlaceId(),
                Id2 = IdManagements.GetlastestPlaceId(),
                Id3 = IdManagements.GetlastestPlaceId(),
                Name = "End",
                Type = _typeBaseRule.GetTypeByInitialMarkingType(type, page),
                InitialMarking = string.Empty,

                xPos1 = position.xPos1 - 4,
                yPos1 = position.yPos1 - 168,

                xPos2 = position.xPos2 - 4,
                yPos2 = position.yPos2 - 167,

                xPos3 = position.xPos3 - 4,
                yPos3 = position.yPos3 - 167,
            };

            return (pl);
        }
    }
}