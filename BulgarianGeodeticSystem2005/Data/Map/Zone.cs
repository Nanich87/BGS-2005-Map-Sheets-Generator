namespace BulgarianGeodeticSystem2005.Data.Map
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BulgarianGeodeticSystem2005.Contracts;

    internal class Zone : IZone
    {
        private const double ZoneWidth = 4.0;
        private const double ZoneLength = 6.0;

        private static readonly Dictionary<int, string> sheetNomenclature = new Dictionary<int, string>()
        {
            { 1, "I" }, { 2, "II" }, { 3, "III" }, { 4, "IV" }, { 5, "V" }, { 6, "VI" }, { 7, "VII" }, { 8, "VIII" }, { 9, "IX" }, { 10, "X" },
            { 11, "XI" }, { 12, "XII" }, { 13, "XIII" }, { 14, "XIV" }, { 15, "XV" }, { 16, "XVI" }, { 17, "XVII" }, { 18, "XVIII" }, { 19, "XIX" }, { 20, "XX" },
            { 21, "XXI" }, { 22, "XXII" }, { 23, "XXIII" }, { 24, "XXIV" }, { 25, "XXV" }
        };

        private readonly int[] validZoneNumbers = new int[2] { 34, 35 };

        private int number;
        private double startingLatitude;
        private double startingLongitude;

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
                if (!this.validZoneNumbers.Contains(value))
                {
                    throw new ArgumentOutOfRangeException("Number", "Invalid zone number!");
                }

                this.number = value;
            }
        }

        public static int GetZoneSizeByMapScale(int mapScale)
        {
            switch (mapScale)
            {
                case 1000:
                    return 960;
                case 5000:
                    return 192;
                case 100000:
                    return 12;
                default:
                    return 0;
            }
        }

        public static bool IsValidSheetPosition(int mapScale, int sheetPosition)
        {
            switch (mapScale)
            {
                case 1000:
                    return sheetPosition > 0 && sheetPosition <= 960;
                case 5000:
                    return sheetPosition > 0 && sheetPosition <= 192;
                case 100000:
                    return sheetPosition > 0 && sheetPosition <= 12;
                default:
                    return false;
            }
        }

        public static string GetSheetNumber(int mapScale, int row, int column)
        {
            if (!Zone.IsValidSheetPosition(mapScale, row))
            {
                throw new ArgumentOutOfRangeException("row", "Invalid row");
            }

            if (!Zone.IsValidSheetPosition(mapScale, column))
            {
                throw new ArgumentOutOfRangeException("column", "Invalid column");
            }

            int sheetNumber100000 = 0;
            int sheetNumber5000 = 0;
            int sheetNumber1000 = 0;

            int a = 0;
            int b = 0;

            switch (mapScale)
            {
                case 1000:
                    sheetNumber100000 = (int)((12 * Math.Ceiling(row / 80.0)) + Math.Ceiling(column / 80.0) - 12);
                    sheetNumber5000 = (int)((16 * Math.Ceiling(row / 5.0)) + Math.Ceiling(column / 5.0) - 16);

                    a = row % 5 > 0 ? row % 5 : 5;
                    b = column % 5 > 0 ? column % 5 : 5;

                    sheetNumber1000 = (5 * a) + b - 5;

                    return string.Format("{0}-{1}-{2}", sheetNumber100000, sheetNumber5000, Zone.sheetNomenclature[sheetNumber1000]);
                case 5000:
                    sheetNumber100000 = (int)((12 * Math.Ceiling(row / 16.0)) + Math.Ceiling(column / 16.0) - 12);

                    a = row % 16 > 0 ? row % 16 : 16;
                    b = column % 16 > 0 ? column % 16 : 16;

                    sheetNumber5000 = (16 * a) + b - 16;

                    return string.Format("{0}-{1}", sheetNumber100000, sheetNumber5000);
                case 100000:
                    sheetNumber100000 = (12 * row) + column - 12;

                    return string.Format("{0}", sheetNumber100000);
                default:
                    throw new ArgumentOutOfRangeException("scale", "Invalid scale");
            }
        }
    }
}