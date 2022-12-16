namespace NestedFlowchart.Functions
{
    public static class IdManagements
    {
        public static int ColorSetId { get; set; } = 26767;
        public static int VarId { get; set; } = 22143;
        public static int InstanceId { get; set; } = 2148;

        public static int PlaceId { get; set; } = 48771;
        public static int PlaceName { get; set; } = 0;
        public static int ConnectorPlaceName { get; set; } = 0;

        public static int TransitionId { get; set; } = 48771;
        public static int TransitionName { get; set; } = 0;
        public static int TrueGuardTransitionName { get; set; } = 0;
        public static int FalseGuardTransitionName { get; set; } = 0;
        public static int SubPageTransitionName { get; set; } = 0;

        public static int ArcId { get; set; } = 49811;
        public static int PageId { get; set; } = 50866;

        public static int SubStrId { get; set; } = 99300;
        public static int PortId { get; set; } = 99400;

        public static string GetlastestColorSetId()
        {
            ColorSetId++;
            return "ID14124" + ColorSetId;
        }

        public static string GetlastestVarId()
        {
            VarId++;
            return "ID14124" + VarId;
        }

        public static string GetlastestInstanceId()
        {
            InstanceId++;
            return "ID" + InstanceId;
        }

        public static string GetlastestPlaceId()
        {
            PlaceId++;
            return "ID14129" + PlaceId;
        }

        public static string GetlastestPlaceName()
        {
            PlaceName++;
            return "P" + PlaceName;
        }

        public static string GetlastestPlaceConnectorName()
        {
            ConnectorPlaceName++;
            return "CN" + ConnectorPlaceName;
        }

        public static string GetlastestTransitionId()
        {
            TransitionId++;
            return "ID14128" + TransitionId;
        }

        public static string GetlastestTransitionName()
        {
            TransitionName++;
            return "T" + TransitionName;
        }

        public static string GetlastestTrueGuardTransitionName()
        {
            TrueGuardTransitionName++;
            return "GT" + TrueGuardTransitionName;
        }

        public static string GetlastestFalseGuardTransitionName()
        {
            FalseGuardTransitionName++;
            return "GF" + FalseGuardTransitionName;
        }

        //TODO: Sub Page is need more id if nested
        public static string GetlastestSubPageTransitionName()
        {
            SubPageTransitionName++;
            return "TS" + SubPageTransitionName;
        }

        public static string GetlastestArcId()
        {
            ArcId++;
            return "ID14129" + ArcId;
        }

        public static string GetlastestPageId()
        {
            PageId++;
            return "ID14129" + PageId;
        }

        public static string GetlastestSubStrId()
        {
            SubStrId++;
            return "ID14123" + SubStrId;
        }

        public static string GetlastestPortId()
        {
            PortId++;
            return "ID14123" + PortId;
        }
    }
}