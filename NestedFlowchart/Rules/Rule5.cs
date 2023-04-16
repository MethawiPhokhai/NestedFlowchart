using NestedFlowchart.Functions;
using NestedFlowchart.Models;
using NestedFlowchart.Position;

namespace NestedFlowchart.Rules
{
    public class Rule5 : ArcBaseRule
    {
        /// <summary>
        /// Transform connector into transition andplace connected by arc
        /// </summary>
        /// <param name="transitionTemplate"></param>
        /// <param name="placeTemplate"></param>
        /// <param name="arcTemplate"></param>
        /// <param name="previousPlace"></param>
        /// <returns></returns>
        public (PlaceModel, TransitionModel?, ArcModel?, string) ApplyRule(
            string arrayName,
			PreviousNode previousNode,
            PositionManagements position,
			int countSubPage)
        {
			var arcVariable = DeclareArcVariable(arrayName, countSubPage);
			TransitionModel tr = null;
			PlaceModel pl = null;
			ArcModel a1 = null, a2 = null;
			string previousTypeReturn = string.Empty;

			if (previousNode.Type == "place")
            {
				//T3 Transition
				tr = new TransitionModel()
				{
					Id1 = IdManagements.GetlastestTransitionId(),
					Id2 = IdManagements.GetlastestTransitionId(),
					Id3 = IdManagements.GetlastestTransitionId(),
					Id4 = IdManagements.GetlastestTransitionId(),
					Id5 = IdManagements.GetlastestTransitionId(),

					Name = IdManagements.GetlastestTransitionName(),

					xPos1 = position.xPos1,
					yPos1 = position.GetLastestyPos1(),

					xPos4 = position.GetLastestxPos4(),
					yPos4 = position.GetLastestyPos4(),
				};

				//CN1 Place
				pl = new PlaceModel()
				{
					Id1 = IdManagements.GetlastestPlaceId(),
					Id2 = IdManagements.GetlastestPlaceId(),
					Id3 = IdManagements.GetlastestPlaceId(),

					Name = IdManagements.GetlastestPlaceConnectorName(),

					xPos1 = position.xPos1,
					yPos1 = position.GetLastestyPos1(),

					xPos2 = position.GetLastestxPos2(),
					yPos2 = position.GetLastestyPos2(),

					Type = "loopi"
				};

				//Arc from T3 to CN1
				a1 = new ArcModel()
				{
					Id1 = IdManagements.GetlastestArcId(),
					Id2 = IdManagements.GetlastestArcId(),

					TransEnd = tr.Id1,
					PlaceEnd = pl.Id1,

					xPos = position.GetLastestxArcPos(),
					yPos = position.GetLastestyArcPos(),

					Orientation = "TtoP", //Transition to Place
					Type = arcVariable
				};

				previousTypeReturn = "place";

            }
			else
			{
				//CN1 Place
				pl = new PlaceModel()
				{
					Id1 = IdManagements.GetlastestPlaceId(),
					Id2 = IdManagements.GetlastestPlaceId(),
					Id3 = IdManagements.GetlastestPlaceId(),

					Name = IdManagements.GetlastestPlaceConnectorName(),

					xPos1 = position.xPos1,
					yPos1 = position.GetLastestyPos1(),

					xPos2 = position.GetLastestxPos2(),
					yPos2 = position.GetLastestyPos2(),

					Type = "loopj"
				};

                previousTypeReturn = "transition";

            }

			return (pl, tr, a1, previousTypeReturn);
        }
	}
}
