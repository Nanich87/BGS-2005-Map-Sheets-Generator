namespace BulgarianGeodeticSystem2005.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using Contracts;
    using Helpers;
    using Map;
    using Point;

    internal static class CoordinateSystem2005
    {
        private const double E2 = 0.0066943800229;

        private const double Fi0 = 42.6678756833333;
        private const double Lambda0 = 25.5;

        private const double Re = 12083793.8966;
        private const double R0 = 6929897.5566;

        private const double X0 = 4725824.3591;
        private const double Y0 = 500000;

        private static readonly ICollection<Sheet> sheets = new List<Sheet>();
        private static StringBuilder output;

        public static ICollection<Sheet> Sheets
        {
            get
            {
                return sheets;
            }
        }

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

            double gamma = (geographicPoint.Longitude - CoordinateSystem2005.Lambda0) * Math.Sin(CoordinateSystem2005.Fi0 * Math.PI / 180);
            double q = 0.5 * (Math.Log((1 + Math.Sin(geographicPoint.Latitude * Math.PI / 180)) / (1 - Math.Sin(geographicPoint.Latitude * Math.PI / 180))) - (Math.Sqrt(CoordinateSystem2005.E2) * Math.Log((1 + (Math.Sqrt(CoordinateSystem2005.E2) * Math.Sin(geographicPoint.Latitude * Math.PI / 180))) / (1 - (Math.Sqrt(CoordinateSystem2005.E2) * Math.Sin(geographicPoint.Latitude * Math.PI / 180))))));
            double r = Re / Math.Exp(q * Math.Sin(CoordinateSystem2005.Fi0 * Math.PI / 180));

            double x = CoordinateSystem2005.R0 + CoordinateSystem2005.X0 - (r * Math.Cos(gamma * Math.PI / 180));
            double y = CoordinateSystem2005.Y0 + (r * Math.Sin(gamma * Math.PI / 180));

            XYPoint projectedPoint = new XYPoint(x, y);

            return projectedPoint;
        }

        public static ICollection<XYPoint> OpenFile(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException(string.Format("Файлът {0} не може да бъде намерен!", path));
            }

            ICollection<XYPoint> points = new List<XYPoint>();

            using (StreamReader reader = new StreamReader(path, Encoding.Default))
            {
                while (!reader.EndOfStream)
                {
                    string[] line = Regex.Replace(reader.ReadLine().Trim(), @"\s\s+", " ").Split(' ');

                    if (line.Length < 3)
                    {
                        continue;
                    }

                    XYPoint point = new XYPoint();
                    point.Number = line[0];
                    point.X = double.Parse(line[1]);
                    point.Y = double.Parse(line[2]);
                    point.Description = string.Join(" ", line);

                    points.Add(point);
                }
            }

            return points;
        }

        public static string ExportSheets()
        {
            return CoordinateSystem2005.output.ToString();
        }

        public static void GenerateSheets(IZone zone, int scale)
        {
            CoordinateSystem2005.output = new StringBuilder();

            CoordinateSystem2005.output.AppendLine("OSNAP OFF");
            CoordinateSystem2005.output.AppendLine("-UNITS 2 4 3 4 300 Y");
            CoordinateSystem2005.output.AppendLine("-UNITS 2 4 3 4 300 Y");

            int zoneSize = ZoneHelper.GetGridSizeByMapScale(scale);

            if (zoneSize == 0)
            {
                return;
            }

            double sheetLength = Zone.Length / zoneSize;
            double sheetWidth = Zone.Width / zoneSize;

            CoordinateSystem2005.Sheets.Clear();

            for (var sheetRowIndex = 1; sheetRowIndex <= zoneSize; sheetRowIndex++)
            {
                for (var sheetColumnIndex = 1; sheetColumnIndex <= zoneSize; sheetColumnIndex++)
                {
                    string sheetNumber = Zone.GetSheetNumber(scale, sheetRowIndex, sheetColumnIndex);

                    Sheet sheet = new Sheet(string.Format("K-{0}-{1}", zone.Number, sheetNumber), scale);

                    LatLonPoint topLeftPoint = new LatLonPoint();
                    topLeftPoint.Longitude = zone.StartingLongitude + ((sheetColumnIndex - 1) * sheetLength);
                    topLeftPoint.Latitude = zone.StartingLatitude - ((sheetRowIndex - 1) * sheetWidth);

                    sheet.GeographicPoints[0] = topLeftPoint;
                    sheet.ProjectedPoints[0] = CoordinateSystem2005.Transform(topLeftPoint);

                    LatLonPoint topRightPoint = new LatLonPoint();
                    topRightPoint.Longitude = zone.StartingLongitude + (sheetColumnIndex * sheetLength);
                    topRightPoint.Latitude = zone.StartingLatitude - ((sheetRowIndex - 1) * sheetWidth);

                    sheet.GeographicPoints[1] = topRightPoint;
                    sheet.ProjectedPoints[1] = CoordinateSystem2005.Transform(topRightPoint);

                    LatLonPoint bottomRightPoint = new LatLonPoint();
                    bottomRightPoint.Longitude = zone.StartingLongitude + (sheetColumnIndex * sheetLength);
                    bottomRightPoint.Latitude = zone.StartingLatitude - (sheetRowIndex * sheetWidth);

                    sheet.GeographicPoints[2] = bottomRightPoint;
                    sheet.ProjectedPoints[2] = CoordinateSystem2005.Transform(bottomRightPoint);

                    LatLonPoint bottomLeftPoint = new LatLonPoint();
                    bottomLeftPoint.Longitude = zone.StartingLongitude + ((sheetColumnIndex - 1) * sheetLength);
                    bottomLeftPoint.Latitude = zone.StartingLatitude - (sheetRowIndex * sheetWidth);

                    sheet.GeographicPoints[3] = bottomLeftPoint;
                    sheet.ProjectedPoints[3] = CoordinateSystem2005.Transform(bottomLeftPoint);

                    CoordinateSystem2005.output.AppendFormat("LINE {0},{1} {2},{3} ", sheet.ProjectedPoints[0].Y, sheet.ProjectedPoints[0].X, sheet.ProjectedPoints[3].Y, sheet.ProjectedPoints[3].X);
                    CoordinateSystem2005.output.AppendLine();
                    CoordinateSystem2005.output.AppendFormat("LINE {0},{1} {2},{3} ", sheet.ProjectedPoints[0].Y, sheet.ProjectedPoints[0].X, sheet.ProjectedPoints[1].Y, sheet.ProjectedPoints[1].X);
                    CoordinateSystem2005.output.AppendLine();

                    if (sheetRowIndex == zoneSize)
                    {
                        CoordinateSystem2005.output.AppendFormat("LINE {0},{1} {2},{3} ", sheet.ProjectedPoints[2].Y, sheet.ProjectedPoints[2].X, sheet.ProjectedPoints[3].Y, sheet.ProjectedPoints[3].X);
                        CoordinateSystem2005.output.AppendLine();
                    }

                    if (sheetColumnIndex == zoneSize)
                    {
                        CoordinateSystem2005.output.AppendFormat("LINE {0},{1} {2},{3} ", sheet.ProjectedPoints[1].Y, sheet.ProjectedPoints[1].X, sheet.ProjectedPoints[2].Y, sheet.ProjectedPoints[2].X);
                        CoordinateSystem2005.output.AppendLine();
                    }

                    double textRotationAngle = Math.Atan2(sheet.ProjectedPoints[1].Y - sheet.ProjectedPoints[0].Y, sheet.ProjectedPoints[1].X - sheet.ProjectedPoints[0].X) * 200 / Math.PI;

                    CoordinateSystem2005.output.Append("TEXT ");
                    CoordinateSystem2005.output.Append(string.Format("{0},{1} ", sheet.ProjectedPoints[3].Y + 25, sheet.ProjectedPoints[3].X + 25));
                    CoordinateSystem2005.output.AppendFormat("{0} ", scale / 25);
                    CoordinateSystem2005.output.AppendFormat("{0} ", textRotationAngle);
                    CoordinateSystem2005.output.AppendLine(string.Format("{0}", sheet.Number));

                    CoordinateSystem2005.Sheets.Add(sheet);
                }
            }
        }
    }
}