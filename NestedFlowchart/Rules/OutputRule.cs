using NestedFlowchart.Functions;
using NestedFlowchart.Models;
using NestedFlowchart.Position;

namespace NestedFlowchart.Rules
{
    public class OutputRule : ArcBaseRule
    {
        private readonly ITypeBaseRule _typeBaseRule;

        public OutputRule()
        {
            _typeBaseRule = new TypeBaseRule(); ;
        }

        public (TransitionModel, ArcModel, PlaceModel, TransitionModel, ArcModel, string)
            ApplyRule(
            string arrayName,
            PositionManagements position,
            PreviousNode pv,
            int type,
            bool isEndNext)
        {
            string previousTypeReturn = "transition";
            PlaceModel pl = null;
            TransitionModel tr1 = null;
            ArcModel a1 = null;
            if (!pv.IsPreviousNodeCondition && pv.Type == "place")
            {
                tr1 = new TransitionModel()
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
            }

            pl = new PlaceModel()
            {
                Id1 = IdManagements.GetlastestPlaceId(),
                Id2 = IdManagements.GetlastestPlaceId(),
                Id3 = IdManagements.GetlastestPlaceId(),

                Name = IdManagements.GetlastestPlaceName(),

                xPos1 = position.xPos1,
                yPos1 = position.GetLastestyPos1(),

                xPos2 = position.GetLastestxPos2(),
                yPos2 = position.GetLastestyPos2(),

                Type = _typeBaseRule.GetTypeByInitialMarkingType(type, pv.CurrentMainPage)
            };

            if (!pv.IsPreviousNodeCondition && pv.Type == "place")
            {
                a1 = new ArcModel()
                {
                    Id1 = IdManagements.GetlastestArcId(),
                    Id2 = IdManagements.GetlastestArcId(),

                    TransEnd = tr1.Id1,
                    PlaceEnd = pl.Id1,

                    xPos = position.GetLastestxArcPos(),
                    yPos = position.GetLastestyArcPos(),

                    Orientation = "TtoP",
                    Type = arrayName
                };

                previousTypeReturn = "place";
            }

            if (!pv.IsPreviousNodeCondition || isEndNext)
            //if ((!pv.IsPreviousNodeCondition == true) && (isEndNext == false))
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

                ArcModel a2 = new ArcModel()
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

                return (tr1, a1, pl, tr, a2, previousTypeReturn);
            }

            return (tr1, a1, pl, null, null, previousTypeReturn);
        }
    }
}