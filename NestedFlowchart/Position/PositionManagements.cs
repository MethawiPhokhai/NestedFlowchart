namespace NestedFlowchart.Position
{
    public class PositionManagements
    {
        //Place, Transition Position
        public double xPos1 { get; set; } = -15.000000;
        public double yPos1 { get; set; } = 126.000000;

        //Place : Type, Transition : Condition
        public double xPos2 { get; set; } = 24.000000;
        public double yPos2 { get; set; } = 102.000000;

        //Place : Initial Marking, Transition : Time
        public double xPos3 { get; set; } = 41.000000;
        public double yPos3 { get; set; } = 149.000000;

        //Transition : Code Segment
        public double xPos4 { get; set; } = -15.000000;
        public double yPos4 { get; set; } = 126.000000;

        //Transition : Priority
        public double xPos5 { get; set; }
        public double yPos5 { get; set; }

        public double xArcPos { get; set; } = 10;
        public double yArcPos { get; set; } = 84;

        public double GetLastestyPos1()
        {
            yPos1 = yPos1 - 84;
            return yPos1;
        }

        public double GetLastestxPos2()
        {
            xPos2 = xPos2;
            return xPos2;
        }

        public double GetLastestyPos2()
        {
            yPos2 = yPos2 - 167;
            return yPos2;
        }

        public double GetLastestxPos4()
        {
            xPos4 = xPos4 + 90;
            return xPos4;
        }
        public double GetLastestyPos4()
        {
            yPos4 = yPos4 - 250;
            return yPos4;
        }

        public double GetLastestxArcPos()
        {
            xArcPos = xArcPos;
            return xArcPos;
        }
        public double GetLastestyArcPos()
        {
            yArcPos = yArcPos - 84;
            return yArcPos;
        }
    }


}
