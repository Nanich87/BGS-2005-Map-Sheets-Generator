namespace BulgarianGeodeticSystem2005.Data.Point
{
    internal class XYPoint
    {
        private string number;
        private double x;
        private double y;
        private string description;

        public XYPoint()
            : this(string.Empty, 0, 0)
        {
        }

        public XYPoint(double x, double y)
            : this(string.Empty, x, y)
        {
        }

        public XYPoint(string number, double x, double y)
            : this(number, x, y, string.Empty)
        {
        }

        public XYPoint(string number, double x, double y, string description)
        {
            this.Number = number;
            this.X = x;
            this.Y = y;
            this.Description = description;
        }

        public string Description
        {
            get
            {
                return this.description;
            }

            set
            {
                this.description = value;
            }
        }

        public string Number
        {
            get
            {
                return this.number;
            }

            set
            {
                this.number = value;
            }
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

            return this.X == other.X && this.Y == other.Y;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = 17;

                result = (result * 23) + ((this.number != null) ? this.number.GetHashCode() : 0);
                result = (result * 23) + this.x.GetHashCode();
                result = (result * 23) + this.y.GetHashCode();
                result = (result * 23) + ((this.description != null) ? this.description.GetHashCode() : 0);

                return result;
            }
        }
    }
}