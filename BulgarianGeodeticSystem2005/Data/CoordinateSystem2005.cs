﻿namespace BulgarianGeodeticSystem2005.Data
{
    using System;
    using System.Text;
    using BulgarianGeodeticSystem2005.Contracts;
    using Data.Map;
    using Data.Point;

    internal static class CoordinateSystem2005
    {
        private const double E2 = 0.0066943800229;

        private const double Fi0 = 42.6678756833333;
        private const double Lambda0 = 25.5;

        private const double Re = 12083793.8966;
        private const double R0 = 6929897.5566;

        private const double X0 = 4725824.3591;
        private const double Y0 = 500000;

        public static int[] SupportedZones
        {
            get
            {
                return new int[2] { 34, 35 };
            }
        }

        public static int[] SupportedScales
        {
            get
            {
                return new int[3] { 1000, 5000, 100000 };
            }
        }

        public static XYPoint Transform(LatLonPoint geographicPoint)
        {
            if (geographicPoint == null)
            {
                throw new ArgumentNullException("geographicPoint", "Geographic point is null!");
            }

            double gamma = (geographicPoint.Longitude - Lambda0) * Math.Sin(Fi0 * Math.PI / 180);
            double q = 0.5 * (Math.Log((1 + Math.Sin(geographicPoint.Latitude * Math.PI / 180)) / (1 - Math.Sin(geographicPoint.Latitude * Math.PI / 180))) - Math.Sqrt(E2) * Math.Log((1 + Math.Sqrt(E2) * Math.Sin(geographicPoint.Latitude * Math.PI / 180)) / (1 - Math.Sqrt(E2) * Math.Sin(geographicPoint.Latitude * Math.PI / 180))));
            double r = Re / Math.Exp(q * Math.Sin(Fi0 * Math.PI / 180));

            double x = R0 + X0 - r * Math.Cos(gamma * Math.PI / 180);
            double y = Y0 + r * Math.Sin(gamma * Math.PI / 180);

            XYPoint projectedPoint = new XYPoint();
            projectedPoint.X = x;
            projectedPoint.Y = y;

            return projectedPoint;
        }

        public static string GenerateSheets(IZone zone, int scale)
        {
            StringBuilder output = new StringBuilder();

            output.AppendLine("OSNAP OFF");
            output.AppendLine("-UNITS 2 4 3 4 300 Y");

            int gridSize = Sheet.GetRowSizeByScale(scale);

            if (gridSize == 0)
            {
                return output.ToString();
            }

            double sheetLength = Zone.Length / gridSize;
            double sheetWidth = Zone.Width / gridSize;

            for (var sheetRowIndex = 1; sheetRowIndex <= gridSize; sheetRowIndex++)
            {
                for (var sheetColumnIndex = 1; sheetColumnIndex <= gridSize; sheetColumnIndex++)
                {
                    string sheetNumber = Sheet.GetSheetNumber(scale, sheetRowIndex, sheetColumnIndex);

                    Sheet sheet = new Sheet(string.Format("K-{0}-{1}", zone.Number, sheetNumber), scale, gridSize);

                    LatLonPoint topLeftPoint = new LatLonPoint();
                    topLeftPoint.Longitude = zone.StartingLongitude + (sheetColumnIndex - 1) * sheetLength;
                    topLeftPoint.Latitude = zone.StartingLatitude - (sheetRowIndex - 1) * sheetWidth;

                    sheet.GeographicPoints[0] = topLeftPoint;
                    sheet.ProjectedPoints[0] = CoordinateSystem2005.Transform(topLeftPoint);

                    LatLonPoint topRightPoint = new LatLonPoint();
                    topRightPoint.Longitude = zone.StartingLongitude + sheetColumnIndex * sheetLength;
                    topRightPoint.Latitude = zone.StartingLatitude - (sheetRowIndex - 1) * sheetWidth;

                    sheet.GeographicPoints[1] = topRightPoint;
                    sheet.ProjectedPoints[1] = CoordinateSystem2005.Transform(topRightPoint);

                    LatLonPoint bottomRightPoint = new LatLonPoint();
                    bottomRightPoint.Longitude = zone.StartingLongitude + sheetColumnIndex * sheetLength;
                    bottomRightPoint.Latitude = zone.StartingLatitude - sheetRowIndex * sheetWidth;

                    sheet.GeographicPoints[2] = bottomRightPoint;
                    sheet.ProjectedPoints[2] = CoordinateSystem2005.Transform(bottomRightPoint);

                    LatLonPoint bottomLeftPoint = new LatLonPoint();
                    bottomLeftPoint.Longitude = zone.StartingLongitude + (sheetColumnIndex - 1) * sheetLength;
                    bottomLeftPoint.Latitude = zone.StartingLatitude - sheetRowIndex * sheetWidth;

                    sheet.GeographicPoints[3] = bottomLeftPoint;
                    sheet.ProjectedPoints[3] = CoordinateSystem2005.Transform(bottomLeftPoint);

                    output.AppendFormat("LINE {0},{1} {2},{3} ", sheet.ProjectedPoints[0].Y, sheet.ProjectedPoints[0].X, sheet.ProjectedPoints[3].Y, sheet.ProjectedPoints[3].X);
                    output.AppendLine();
                    output.AppendFormat("LINE {0},{1} {2},{3} ", sheet.ProjectedPoints[0].Y, sheet.ProjectedPoints[0].X, sheet.ProjectedPoints[1].Y, sheet.ProjectedPoints[1].X);
                    output.AppendLine();

                    if (sheetRowIndex == gridSize)
                    {
                        output.AppendFormat("LINE {0},{1} {2},{3} ", sheet.ProjectedPoints[2].Y, sheet.ProjectedPoints[2].X, sheet.ProjectedPoints[3].Y, sheet.ProjectedPoints[3].X);
                        output.AppendLine();
                    }

                    if (sheetColumnIndex == gridSize)
                    {
                        output.AppendFormat("LINE {0},{1} {2},{3} ", sheet.ProjectedPoints[1].Y, sheet.ProjectedPoints[1].X, sheet.ProjectedPoints[2].Y, sheet.ProjectedPoints[2].X);
                        output.AppendLine();
                    }

                    double textRotationAngle = Math.Atan2(sheet.ProjectedPoints[1].Y - sheet.ProjectedPoints[0].Y, sheet.ProjectedPoints[1].X - sheet.ProjectedPoints[0].X) * 200 / Math.PI;

                    output.Append("TEXT ");
                    output.Append(string.Format("{0},{1} ", sheet.ProjectedPoints[3].Y + 25, sheet.ProjectedPoints[3].X + 25));
                    output.AppendFormat("{0} ", scale / 1000);
                    output.AppendFormat("{0} ", textRotationAngle);
                    output.AppendLine(string.Format("{0}", sheet.Number));
                }
            }

            return output.ToString();
        }
    }
}