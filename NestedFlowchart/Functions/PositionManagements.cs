namespace NestedFlowchart.Functions
{
    public static class PositionManagements
    {
        //Place, Transition Position
        public static double xPos1 { get; set; } = -15.000000;
        public static double yPos1 { get; set; } = 126.000000;

        //Place : Type, Transition : Condition
        public static double xPos2 { get; set; } = 24.000000;
        public static double yPos2 { get; set; } = 102.000000;

        //Place : Initial Marking, Transition : Time
        public static double xPos3 { get; set; } = 41.000000;
        public static double yPos3 { get; set; } = 149.000000;

        //Transition : Code Segment
        public static double xPos4 { get; set; } = xPos1;
        public static double yPos4 { get; set; } = yPos1;

        //Transition : Priority
        public static double xPos5 { get; set; }
        public static double yPos5 { get; set; }

        public static double xArcPos { get; set; } = 10;
        public static double yArcPos { get; set; } = 84;

        public static double GetLastestyPos1()
        {
            yPos1 = yPos1 - 84;
            return yPos1;
        }

        public static double GetLastestxPos2()
        {
            xPos2 = xPos2;
            return xPos2;
        }

        public static double GetLastestyPos2()
        {
            yPos2 = yPos2 - 167;
            return yPos2;
        }

        public static double GetLastestxPos4()
        {
            xPos4 = xPos4 + 90;
            return xPos4;
        }
        public static double GetLastestyPos4()
        {
            yPos4 = yPos4 - 250;
            return yPos4;
        }

        public static double GetLastestxArcPos()
        {
            xArcPos = xArcPos;
            return xArcPos;
        }
        public static double GetLastestyArcPos()
        {
            yArcPos = yArcPos - 84;
            return yArcPos;
        }
    }


}
