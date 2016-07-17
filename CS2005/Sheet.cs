namespace CS2005
{
    using System;
    using System.Linq;

    internal class Sheet
    {
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