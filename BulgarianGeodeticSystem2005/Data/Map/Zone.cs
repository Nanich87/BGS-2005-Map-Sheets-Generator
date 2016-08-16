namespace BulgarianGeodeticSystem2005.Data.Map
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Helpers;

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

        public static string GetSheetNumber(int mapScale, int row, int column)
        {
            if (!ZoneHelper.IsValidSheetPosition(mapScale, row))
            {
                throw new ArgumentOutOfRangeException("row", "Invalid row!");
            }

            if (!ZoneHelper.IsValidSheetPosition(mapScale, column))
            {
                throw new ArgumentOutOfRangeException("column", "Invalid column!");
            }

            switch (mapScale)
            {
                case 1000:
                    {
                        // Sheet number 100000
                        int row100000 = (int)Math.Ceiling(row / 80.0);
                        int column100000 = (int)Math.Ceiling(column / 80.0);

                        int sheetNumber100000 = ZoneHelper.GetGridIndex(row100000, column100000, 12);

                        // Sheet number 5000

                        int reducedRow100000 = row - ((row100000 - 1) * 16 * 5);
                        int reducedColumn100000 = column - ((column100000 - 1) * 16 * 5);

                        int row5000 = (int)Math.Ceiling(reducedRow100000 / 5.0);
                        int column5000 = (int)Math.Ceiling(reducedColumn100000 / 5.0);

                        int sheetNumber5000 = ZoneHelper.GetGridIndex(row5000, column5000, 16);

                        // Sheet number 1000
                        int row1000 = ZoneHelper.ReduceChildField(row, 5);
                        int column1000 = ZoneHelper.ReduceChildField(column, 5);

                        int sheetNumber1000 = ZoneHelper.GetGridIndex(row1000, column1000, 5);

                        return string.Format("{0}-{1}-{2}", sheetNumber100000, sheetNumber5000, Zone.sheetNomenclature[sheetNumber1000]);
                    }

                case 5000:
                    {
                        int row100000 = ZoneHelper.ReduceParentField(row, 16);
                        int column100000 = ZoneHelper.ReduceParentField(column, 16);

                        int sheetNumber100000 = ZoneHelper.GetGridIndex(row100000, column100000, 12);

                        int reducedRow = ZoneHelper.ReduceChildField(row, 16);
                        int reducedColumn = ZoneHelper.ReduceChildField(column, 16);

                        int sheetNumber5000 = ZoneHelper.GetGridIndex(reducedRow, reducedColumn, 16);

                        return string.Format("{0}-{1}", sheetNumber100000, sheetNumber5000);
                    }

                case 100000:
                    return string.Format("{0}", ZoneHelper.GetGridIndex(row, column, 12));
                default:
                    throw new ArgumentOutOfRangeException("scale", "Invalid scale!");
            }
        }
    }
}