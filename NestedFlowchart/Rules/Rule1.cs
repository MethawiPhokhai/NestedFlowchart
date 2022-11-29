using NestedFlowchart.Functions;
using NestedFlowchart.Models;

namespace NestedFlowchart.Rules
{
    /// <summary>
    /// Transform Start to place start
    /// </summary>
    public class Rule1
    {
        public (PlaceModel, string) ApplyRule(string placeTemplate)
        {
            PlaceModel pl = new PlaceModel()
            {
                Id1 = IdManagements.GetlastestPlaceId(),
                Id2 = IdManagements.GetlastestPlaceId(),
                Id3 = IdManagements.GetlastestPlaceId(),
                Name = "Start",
                Type = "UNIT",

                xPos1 = PositionManagements.xPos1,
                yPos1 = PositionManagements.yPos1,

                xPos2 = PositionManagements.xPos2,
                yPos2 = PositionManagements.yPos2,

                xPos3 = PositionManagements.xPos3,
                yPos3 = PositionManagements.yPos3
            };

            TransformationApproach approach = new TransformationApproach();
            var place1 = approach.CreatePlace(placeTemplate, pl);

            return (pl, place1);
        }
    }
}
