using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NestedFlowchart.Models
{
    public class ArcModel
    {
        public string Id1 { get; set; }
        public string Id2 { get; set; }
        public string TransEnd { get; set; }
        public string PlaceEnd { get; set; }

        public string Orientation { get; set; }
        public string Type { get; set; }

    }
}
