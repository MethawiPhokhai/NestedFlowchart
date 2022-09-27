using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NestedFlowchart.Models
{
    public class TransitionModel
    {
        public string Id1 { get; set; }
        public string Id2 { get; set; }
        public string Id3 { get; set; }
        public string Id4 { get; set; }
        public string Id5 { get; set; }

        public string Name { get; set; }
        public string Condition { get; set; }
        public string CodeSegment { get; set; }

        public double xPos1 { get; set; }
        public double yPos1 { get; set; }

        public double xPos2 { get; set; }
        public double yPos2 { get; set; }
        
        public double xPos3 { get; set; }
        public double yPos3 { get; set; }

        public double xPos4 { get; set; }
        public double yPos4 { get; set; }

        public double xPos5 { get; set; }
        public double yPos5 { get; set; }
    }
}
