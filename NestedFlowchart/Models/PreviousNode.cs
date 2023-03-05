namespace NestedFlowchart.Models
{
    public class PreviousNode
    {
        public string elementId { get; set; }
        public PlaceModel previousPlaceModel { get; set; } = new PlaceModel();
        public TransitionModel previousTransitionModel { get; set; } = new TransitionModel();

        //place, transition
        public string Type { get; set; }
    }
}
