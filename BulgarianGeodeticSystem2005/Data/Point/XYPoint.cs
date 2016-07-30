namespace BulgarianGeodeticSystem2005.Data.Point
{
    internal class XYPoint
    {
        private double x;
        private double y;

        public XYPoint()
            : this(0, 0)
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

        public override bool Equals(object obj)
        {
            XYPoint other = obj as XYPoint;
            if (other == null)
            {
                return false;
            }

            return (this.X == other.X && this.Y == other.Y);
        }
    }
}