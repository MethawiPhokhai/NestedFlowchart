using NestedFlowchart.Functions;
using NestedFlowchart.Models;
using NestedFlowchart.Position;

namespace NestedFlowchart.Rules
{
    public class BaseRule
    {
        public ArcModel? CreateArcWithPreviousNode(TempArrow arrow, PositionManagements position, string arrayName, TransitionModel currentTransition, List<PreviousNode> previousNodes)
        {
            //Search on previous node that source is the same id
            var found = previousNodes.FirstOrDefault(x => x.elementId == arrow.Source);

            if (found != null)
            {
                //Rule2 สามารถเอาไปต่อกับตัวก่อนหน้าได้เลย เพราะไม่มีทางแยก
                return new ArcModel()
                {
                    Id1 = IdManagements.GetlastestArcId(),
                    Id2 = IdManagements.GetlastestArcId(),

                    TransEnd = currentTransition.Id1,
                    PlaceEnd = found.currentPlaceModel.Id1,

                    xPos = position.xArcPos,
                    yPos = position.yArcPos == 84 ? position.yArcPos : position.GetLastestyArcPos(),

                    Orientation = "PtoT", //Place to Transition
                    Type = arrayName
                };
            }

            return null;
        }
    }
}
