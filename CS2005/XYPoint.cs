namespace CS2005
{
    using System;
    using System.Linq;

    internal class XYPoint
    {
        private double x;
        private double y;

        public XYPoint()
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