using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NestedFlowchart.Models
{
    public class RouterTable
    {
        public int RowNumber { get; set; }
        public string ArrowID { get; set; }
        public string ArrowText { get; set; }
        public string SourceID { get; set; }
        public string SourceType { get; set; }
        public string SourceText { get; set; }
        public string TargetID { get; set; }
        public string TargetType { get; set; }
        public string TargetText { get; set; }
    }
}
