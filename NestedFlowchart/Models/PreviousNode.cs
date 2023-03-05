namespace NestedFlowchart.Models
{
    public class PreviousNode
    {
        public string elementId { get; set; }
        public PlaceModel previousPlaceModel { get; set; } = new PlaceModel();
        public TransitionModel previousTransitionModel { get; set; } = new TransitionModel();

        //Place, Transition
        public string Type { get; set; }
    }
}
