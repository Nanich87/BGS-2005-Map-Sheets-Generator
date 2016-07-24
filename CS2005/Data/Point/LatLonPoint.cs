namespace CS2005.Data.Point
{
    internal class LatLonPoint
    {
        private double latitude;
        private double longitude;

        public LatLonPoint() : this(0, 0)
        {
        }

        public LatLonPoint(double latitude, double longitude)
        {
            this.latitude = latitude;
            this.longitude = longitude;
        }

        public double Longitude
        {
            get
            {
                return this.longitude;
            }

            set
            {
                this.longitude = value;
            }
        }

        public double Latitude
        {
            get
            {
                return this.latitude;
            }

            set
            {
                this.latitude = value;
            }
        }
    }
}