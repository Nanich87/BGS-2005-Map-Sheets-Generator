namespace CS2005
{
    using System;
    using System.Linq;

    internal class LatLonPoint
    {
        private double latitude;
        private double longitude;

        public LatLonPoint()
        {
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