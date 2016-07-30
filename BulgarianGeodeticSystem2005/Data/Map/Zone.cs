namespace BulgarianGeodeticSystem2005.Data.Map
{
    using System;
    using System.Linq;
    using BulgarianGeodeticSystem2005.Contracts;

    internal class Zone : IZone
    {
        private const double ZoneWidth = 4.0;
        private const double ZoneLength = 6.0;

        private int number;
        private double startingLatitude;
        private double startingLongitude;

        private readonly int[] validZoneNumbers = new int[2] { 34, 35 };

        public Zone(int number, double startingLatitude, double startingLongitude)
        {
            this.Number = number;
            this.StartingLatitude = startingLatitude;
            this.StartingLongitude = startingLongitude;
        }

        public static double Length
        {
            get
            {
                return ZoneLength;
            }
        }

        public static double Width
        {
            get
            {
                return ZoneWidth;
            }
        }

        public double StartingLongitude
        {
            get
            {
                return this.startingLongitude;
            }

            private set
            {
                this.startingLongitude = value;
            }
        }

        public double StartingLatitude
        {
            get
            {
                return this.startingLatitude;
            }

            private set
            {
                this.startingLatitude = value;
            }
        }

        public int Number
        {
            get
            {
                return this.number;
            }

            private set
            {
                if (!validZoneNumbers.Contains(value))
                {
                    throw new ArgumentOutOfRangeException("Number", "Invalid zone number!");
                }

                this.number = value;
            }
        }
    }
}