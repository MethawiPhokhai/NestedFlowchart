namespace NestedFlowchart.Models
{
    public class PreviousNode
    {
        public string elementId { get; set; }

        public PlaceModel previousPlaceModel { get; set; } = new PlaceModel();
        public TransitionModel previousTransitionModel { get; set; } = new TransitionModel();

        public PlaceModel currentPlaceModel { get; set; } = new PlaceModel();
        public TransitionModel currentTransitionModel { get; set; } = new TransitionModel();

        //place, transition
        public string Type { get; set; }

        public int CurrentMainPage { get; set; }
        public int CurrentSubPage { get; set; }
        public bool IsCreateSubPage { get; set; }
        public bool IsPreviousNodeCondition { get; set; }
        public bool IsConnector { get; set; }
    }
}
