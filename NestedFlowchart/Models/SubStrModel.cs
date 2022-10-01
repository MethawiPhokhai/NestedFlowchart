namespace NestedFlowchart.Models
{
    public class SubStrModel
    {
        public string SubPageId { get; set; }
        public string NewInputPlaceId { get; set; }
        public string OldInputPlaceId { get; set; }

        public string NewOutputPlaceId { get; set; }
        public string OldOutputPlaceId { get; set; }

        public string Id { get; set; }
        public string Name { get; set; }

        public double xPos { get; set; }
        public double yPos { get; set; }
    }
}
