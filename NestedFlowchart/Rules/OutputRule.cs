using NestedFlowchart.Functions;
using NestedFlowchart.Models;
using NestedFlowchart.Position;

namespace NestedFlowchart.Rules
{
    public class OutputRule : ArcBaseRule
    {
        public (PlaceModel, TransitionModel, ArcModel)
            ApplyRule(
            string arrayName,
            PositionManagements position,
            PreviousNode pv)
        {
            PlaceModel pl = new PlaceModel()
            {
                Id1 = IdManagements.GetlastestPlaceId(),
                Id2 = IdManagements.GetlastestPlaceId(),
                Id3 = IdManagements.GetlastestPlaceId(),

                Name = IdManagements.GetlastestPlaceName(),

                xPos1 = position.xPos1,
                yPos1 = position.GetLastestyPos1(),

                xPos2 = position.GetLastestxPos2(),
                yPos2 = position.GetLastestyPos2(),

                Type = (arrayName == "array") ? "INTs" : "aa"
            };

            if (!pv.IsPreviousNodeCondition)
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
                    yPos1 = position.GetLastestyPos1()
                };

                ArcModel a1 = new ArcModel()
                {
                    Id1 = IdManagements.GetlastestArcId(),
                    Id2 = IdManagements.GetlastestArcId(),

                    TransEnd = tr.Id1,
                    PlaceEnd = pl.Id1,

                    xPos = position.GetLastestxArcPos(),
                    yPos = position.GetLastestyArcPos(),

                    Orientation = "PtoT",
                    Type = arrayName
                };

                return (pl, tr, a1);
            }

            return (pl, null, null);

        }
    }
}
