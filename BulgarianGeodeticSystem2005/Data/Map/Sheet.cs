namespace BulgarianGeodeticSystem2005.Data.Map
{
    using System;
    using System.Linq;
    using Data.Point;

    internal class Sheet
    {
        private readonly int scale;
        private readonly string number;

        private readonly LatLonPoint[] geographicPoints;
        private readonly XYPoint[] projectedPoints;

        public Sheet(string number, int scale)
        {
            this.number = number;
            this.scale = scale;

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

        public bool ContainsPoint(XYPoint point)
        {
            bool result = false;

            int j = this.ProjectedPoints.Count() - 1;

            for (int i = 0; i < this.ProjectedPoints.Count(); i++)
            {
                if ((this.ProjectedPoints[i].Y < point.Y && this.ProjectedPoints[j].Y >= point.Y) ||
                    (this.ProjectedPoints[j].Y < point.Y && this.ProjectedPoints[i].Y >= point.Y))
                {
                    if ((this.ProjectedPoints[i].X + (((point.Y - this.ProjectedPoints[i].Y) / (this.ProjectedPoints[j].Y - this.ProjectedPoints[i].Y)) * (this.ProjectedPoints[j].X - this.ProjectedPoints[i].X))) < point.X)
                    {
                        result = !result;
                    }
                }

                j = i;
            }

            return result;
        }

        public bool IsPointInsideSheet(XYPoint point)
        {
            for (int i = 0; i < this.ProjectedPoints.Length; i++)
            {
                if (i < this.ProjectedPoints.Length - 1)
                {
                    if ((int)((this.ProjectedPoints[i + 1].X - this.ProjectedPoints[i].X) * (point.Y - this.ProjectedPoints[i].Y) 
                        - (point.X - this.ProjectedPoints[i].X) * (this.ProjectedPoints[i + 1].Y - this.ProjectedPoints[i].Y)) < 0.000)
                    {
                        return false;
                    }
                }
                else
                {
                    if ((int)((this.ProjectedPoints[0].X - this.ProjectedPoints[i].X) * (point.Y - this.ProjectedPoints[i].Y) 
                        - (point.X - this.ProjectedPoints[i].X) * (this.ProjectedPoints[0].Y - this.ProjectedPoints[i].Y)) < 0.000)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}