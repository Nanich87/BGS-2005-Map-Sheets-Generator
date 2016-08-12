namespace BulgarianGeodeticSystem2005.Data.Map
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data.Point;

    internal class Sheet
    {
        private static readonly Dictionary<int, string> sheetNomenclature = new Dictionary<int, string>()
        {
            { 1, "I" }, { 2, "II" }, { 3, "III" }, { 4, "IV" }, { 5, "V" }, { 6, "VI" }, { 7, "VII" }, { 8, "VIII" }, { 9, "IX" }, { 10, "X" },
            { 11, "XI" }, { 12, "XII" }, { 13, "XIII" }, { 14, "XIV" }, { 15, "XV" }, { 16, "XVI" }, { 17, "XVII" }, { 18, "XVIII" }, { 19, "XIX" }, { 20, "XX" },
            { 21, "XXI" }, { 22, "XXII" }, { 23, "XXIII" }, { 24, "XXIV" }, { 25, "XXV" }
        };

        private readonly int scale;
        private readonly int sheetSize;
        private readonly string number;
        private readonly LatLonPoint[] geographicPoints;
        private readonly XYPoint[] projectedPoints;

        public Sheet(string number, int scale, int sheetSize)
        {
            this.number = number;
            this.scale = scale;
            this.sheetSize = sheetSize;
            this.geographicPoints = new LatLonPoint[4];
            this.projectedPoints = new XYPoint[4];
        }

        public static int GetRowSizeByScale(int scale)
        {
            switch (scale)
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

        private static bool IsValidGridPosition(int scale, int field)
        {
            switch (scale)
            {
                case 1000:
                    return (field > 0 && field <= 960);
                case 5000:
                    return (field > 0 && field <= 192);
                case 100000:
                    return (field > 0 && field <= 12);
                default:
                    return false;
            }
        }

        public static string GetSheetNumber(int scale, int row, int column)
        {
            if (!Sheet.IsValidGridPosition(scale, row))
            {
                throw new ArgumentOutOfRangeException("row", "Invalid row");
            }

            if (!Sheet.IsValidGridPosition(scale, column))
            {
                throw new ArgumentOutOfRangeException("column", "Invalid column");
            }

            int sheetNumber100000 = 0;
            int sheetNumber5000 = 0;
            int sheetNumber1000 = 0;

            int a = 0;
            int b = 0;

            switch (scale)
            {
                case 1000:
                    sheetNumber100000 = (int)((12 * Math.Ceiling(row / 80.0)) + Math.Ceiling(column / 80.0) - 12);
                    sheetNumber5000 = (int)((16 * Math.Ceiling(row / 5.0)) + Math.Ceiling(column / 5.0) - 16);

                    a = row % 5 > 0 ? row % 5 : 5;
                    b = column % 5 > 0 ? column % 5 : 5;

                    sheetNumber1000 = 5 * a + b - 5;

                    return string.Format("{0}-{1}-{2}", sheetNumber100000, sheetNumber5000, Sheet.sheetNomenclature[sheetNumber1000]);
                case 5000:
                    sheetNumber100000 = (int)((12 * Math.Ceiling(row / 16.0)) + Math.Ceiling(column / 16.0) - 12);

                    a = row % 16 > 0 ? row % 16 : 16;
                    b = column % 16 > 0 ? column % 16 : 16;

                    sheetNumber5000 = 16 * a + b - 16;

                    return string.Format("{0}-{1}", sheetNumber100000, sheetNumber5000);
                case 100000:
                    sheetNumber100000 = (12 * row) + column - 12;

                    return string.Format("{0}", sheetNumber100000);
                default:
                    throw new ArgumentOutOfRangeException("scale", "Invalid scale");
            }
        }

        public bool ContainsPoint(XYPoint point)
        {
            bool result = false;

            int j = this.ProjectedPoints.Count() - 1;

            for (int i = 0; i < this.ProjectedPoints.Count(); i++)
            {
                if (this.ProjectedPoints[i].Y < point.Y && this.ProjectedPoints[j].Y >= point.Y || this.ProjectedPoints[j].Y < point.Y && this.ProjectedPoints[i].Y >= point.Y)
                {
                    if (this.ProjectedPoints[i].X + (point.Y - this.ProjectedPoints[i].Y) / (this.ProjectedPoints[j].Y - this.ProjectedPoints[i].Y) * (this.ProjectedPoints[j].X - this.ProjectedPoints[i].X) < point.X)
                    {
                        result = !result;
                    }
                }

                j = i;
            }

            return result;
        }

        public XYPoint[] ProjectedPoints
        {
            get
            {
                return this.projectedPoints;
            }
        }

        public string Number
        {
            get
            {
                return this.number;
            }
        }

        public int SheetSize
        {
            get
            {
                return this.sheetSize;
            }
        }

        public LatLonPoint[] GeographicPoints
        {
            get
            {
                return this.geographicPoints;
            }
        }

        public int Scale
        {
            get
            {
                return this.scale;
            }
        }
    }
}