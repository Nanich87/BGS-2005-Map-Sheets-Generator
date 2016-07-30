namespace BulgarianGeodeticSystem2005.Data.Point
{
    using System;

    internal class LatLonPoint
    {
        private double latitude;
        private double longitude;

        public LatLonPoint() : this(0, 0)
        {
        }

        public LatLonPoint(double latitude, double longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        public double Longitude
        {
            get
            {
                return this.longitude;
            }

            set
            {
                if (value < -180 || value > 180)
                {
                    throw new ArgumentOutOfRangeException("Longitude", "Longitude cannot be less than -180 or greather than 180!");
                }

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
                if (value < -90 || value > 90)
                {
                    throw new ArgumentOutOfRangeException("Latitude", "Latitude cannot be less than -90 or greather than 90!");
                }

                this.latitude = value;
            }
        }
    }
}