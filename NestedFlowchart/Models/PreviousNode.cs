using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NestedFlowchart.Models
{
    public class PreviousNode
    {
        public PlaceModel previousPlaceModel = new PlaceModel();
        public TransitionModel previousTransitionModel = new TransitionModel();

        //Place, Transition
        public string Type{ get; set; }
    }
}
