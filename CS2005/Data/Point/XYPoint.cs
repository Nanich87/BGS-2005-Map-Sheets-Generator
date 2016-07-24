namespace CS2005.Data.Point
{
    internal class XYPoint
    {
        private double x;
        private double y;

        public XYPoint() : this(0, 0)
        {
        }

        public XYPoint(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public double Y
        {
            get
            {
                return this.y;
            }

            set
            {
                this.y = value;
            }
        }

        public double X
        {
            get
            {
                return this.x;
            }

            set
            {
                this.x = value;
            }
        }
    }
}