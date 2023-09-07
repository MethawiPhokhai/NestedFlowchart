using NestedFlowchart.Declaration;

namespace NestedFlowchart.Models
{
    public class PreviousNode
    {
        public string elementId { get; set; }

        public PlaceModel previousPlaceModel { get; set; } = new PlaceModel();

        public PlaceModel currentPlaceModel { get; set; } = new PlaceModel();
        public TransitionModel currentTransitionModel { get; set; } = new TransitionModel();
        public TransitionModel currentFalseTransitionModel { get; set; } = new TransitionModel();

        public PlaceModel outputPortMainPagePlaceModel { get; set; } = new PlaceModel();
        public PlaceModel outputPortSubPagePlaceModel  { get; set; } = new PlaceModel();

        //Output previous transition
        public TransitionModel outputPreviousTransition { get; set; } = new TransitionModel();

        //place, transition
        public string Type { get; set; }

        //declare type
        public int InitialMarkingType { get; set; }
        public string ArrayName { get; set; } = string.Empty;

        public int CurrentMainPage { get; set; }
        public int CurrentSubPage { get; set; }
        public bool IsBacktoPreviousPage { get; set; }
        public bool IsCreateSubPage { get; set; }
        public bool IsPreviousNodeCondition { get; set; }
        public bool IsConnectedEnd { get; set; }
    }
}
