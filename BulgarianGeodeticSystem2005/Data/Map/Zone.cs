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

            switch (mapScale)
            {
                case 1000:
                    {
                        // Sheet number 100000

                        int sheetNumber100000 = Zone.GetSheetIndex((int)Math.Ceiling(row / 80.0), (int)Math.Ceiling(column / 80.0), 12);

                        // Sheet number 5000

                        int row100000 = Zone.GetRowBySheetIndex(sheetNumber100000, 12);
                        int column100000 = Zone.GetColumnBySheetIndex(sheetNumber100000, 12);

                        int reducedRow100000 = row - (row100000 - 1) * 16 * 5;
                        int reducedColumn100000 = column - (column100000 - 1) * 16 * 5;

                        int sheetNumber5000 = Zone.GetSheetIndex((int)Math.Ceiling(reducedRow100000 / 5.0), (int)Math.Ceiling(reducedColumn100000 / 5.0), 16);

                        // Sheet number 1000

                        int row1000 = Zone.ReduceChildField(row, 5);
                        int column1000 = Zone.ReduceChildField(column, 5);

                        int sheetNumber1000 = Zone.GetSheetIndex(row1000, column1000, 5);

                        return string.Format("{0}-{1}-{2}", sheetNumber100000, sheetNumber5000, Zone.sheetNomenclature[sheetNumber1000]);
                    }
                case 5000:
                    {
                        int row100000 = Zone.ReduceParentField(row, 16);
                        int column100000 = Zone.ReduceParentField(column, 16);

                        int sheetNumber100000 = Zone.GetSheetIndex(row100000, column100000, 12);

                        int reducedRow = Zone.ReduceChildField(row, 16);
                        int reducedColumn = Zone.ReduceChildField(column, 16);

                        int sheetNumber5000 = Zone.GetSheetIndex(reducedRow, reducedColumn, 16);

                        return string.Format("{0}-{1}", sheetNumber100000, sheetNumber5000);
                    }
                case 100000:
                    return string.Format("{0}", Zone.GetSheetIndex(row, column, 12));
                default:
                    throw new ArgumentOutOfRangeException("scale", "Invalid scale");
            }
        }

        public static int ReduceParentField(int field, int value)
        {
            return (int)Math.Ceiling(field / (double)value);
        }

        public static int ReduceChildField(int field, int value)
        {
            return field % value > 0 ? field % value : value;
        }

        public static int GetRowBySheetIndex(int sheetIndex, int sheetSize)
        {
            int row = (int)Math.Ceiling(sheetIndex / (double)sheetSize);

            return row;
        }

        public static int GetColumnBySheetIndex(int sheetIndex, int sheetSize)
        {
            int column = sheetIndex % sheetSize > 0 ? sheetIndex % sheetSize : sheetSize;

            return column;
        }

        public static int GetSheetIndex(int row, int column, int sheetSize)
        {
            if (sheetSize <= 0)
            {
                throw new ArgumentOutOfRangeException("sheetSize", "Invalid sheet size!");
            }

            if (row <= 0 || row > sheetSize)
            {
                throw new ArgumentOutOfRangeException("row", "Invalid row!");
            }

            if (column <= 0 || column > sheetSize)
            {
                throw new ArgumentOutOfRangeException("column", "Invalid column!");
            }

            int sheetIndex = (row * sheetSize) + column - sheetSize;

            return sheetIndex;
        }

        public static int GetSheetSubIndex(int parentRow, int parentColumn, int childSheetSize)
        {
            if (childSheetSize <= 0)
            {
                throw new ArgumentOutOfRangeException("sheetSize", "Invalid sheet size!");
            }

            if (parentRow <= 0)
            {
                throw new ArgumentOutOfRangeException("row", "Invalid row!");
            }

            if (parentColumn <= 0)
            {
                throw new ArgumentOutOfRangeException("column", "Invalid column!");
            }

            double reducedChildRow = parentRow / childSheetSize;
            reducedChildRow = Math.Ceiling(reducedChildRow);

            int childRow = (int)(parentRow - (childSheetSize * (reducedChildRow - 1)));

            double reducedChildColumn = parentColumn / childSheetSize;
            reducedChildColumn = Math.Ceiling(reducedChildColumn);

            int childColumn = (int)(parentColumn - (childSheetSize * (reducedChildColumn - 1)));

            int sheetIndex = (childRow * childSheetSize) + childColumn - childSheetSize;

            return sheetIndex;
        }
    }
}