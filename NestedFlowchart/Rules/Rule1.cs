using NestedFlowchart.Functions;
using NestedFlowchart.Models;
using NestedFlowchart.Position;

namespace NestedFlowchart.Rules
{
    /// <summary>
    /// Transform Start to place start
    /// </summary>
    public class Rule1
    {
        public PlaceModel ApplyRule()
        {
            Page1Position position = new Page1Position();
            PlaceModel pl = new PlaceModel()
            {
                Id1 = IdManagements.GetlastestPlaceId(),
                Id2 = IdManagements.GetlastestPlaceId(),
                Id3 = IdManagements.GetlastestPlaceId(),
                Name = "Start",
                Type = "UNIT",

                xPos1 = position.xPos1,
                yPos1 = position.yPos1,

                xPos2 = position.xPos2,
                yPos2 = position.yPos2,

                xPos3 = position.xPos3,
                yPos3 = position.yPos3
            };

            return pl;
        }
    }
}