﻿namespace NestedFlowchart.Functions
{
    public static class IdManagements
    {
        public static int ColorSetId { get; set; } = 26767;
        public static int VarId { get; set; } = 22143;
        public static int InstanceId { get; set; } = 2148;
        public static int PlaceId { get; set; } = 48771;
        public static int TransitionId { get; set; } = 48771;
        public static int ArcId { get; set; } = 49811;
        public static int PageId { get; set; } = 50866;

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

        public static string GetlastestTransitionId()
        {
            TransitionId++;
            return "ID14128" + TransitionId;
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
    }
}