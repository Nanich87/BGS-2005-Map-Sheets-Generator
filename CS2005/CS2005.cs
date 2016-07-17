namespace CS2005
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    internal static class CS2005
    {
        private const double E2 = 0.0066943800229;

        private const double Fi0 = 42.6678756833333;
        private const double Lambda0 = 25.5;

        private const double Re = 12083793.8966;
        private const double R0 = 6929897.5566;

        private const double X0 = 4725824.3591;
        private const double Y0 = 500000;

        public static XYPoint Transform(LatLonPoint geographicPoint)
        {
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

        public static string CreateMapSheets(int sheetSize, int sheetScale, Sheet parentSheet, double parentSheetWidth, double parentSheetLength, IDictionary<int, string> sheetNomenclature = null)
        {
            StringBuilder output = new StringBuilder();

            for (var sheetWidthIndex = 1; sheetWidthIndex <= sheetSize; sheetWidthIndex++)
            {
                for (var sheetLengthIndex = 1; sheetLengthIndex <= sheetSize; sheetLengthIndex++)
                {
                    var sheetNumber = (sheetSize * sheetWidthIndex) + sheetLengthIndex - sheetSize;

                    Sheet sheet = new Sheet(string.Format("{0}-{1}", parentSheet.Number, sheetNomenclature != null ? sheetNomenclature[sheetNumber] : sheetNumber.ToString()), sheetScale, sheetSize);

                    LatLonPoint topLeftPointSheet = new LatLonPoint();
                    topLeftPointSheet.Longitude = parentSheet.GeographicPoints[0].Longitude + (sheetLengthIndex - 1) * parentSheetLength / sheetSize;
                    topLeftPointSheet.Latitude = parentSheet.GeographicPoints[0].Latitude - (sheetWidthIndex - 1) * parentSheetWidth / sheetSize;

                    sheet.GeographicPoints[0] = topLeftPointSheet;
                    sheet.ProjectedPoints[0] = CS2005.Transform(topLeftPointSheet);

                    LatLonPoint topRightPointSheet = new LatLonPoint();
                    topRightPointSheet.Longitude = parentSheet.GeographicPoints[0].Longitude + sheetLengthIndex * parentSheetLength / sheetSize;
                    topRightPointSheet.Latitude = parentSheet.GeographicPoints[0].Latitude - (sheetWidthIndex - 1) * parentSheetWidth / sheetSize;

                    sheet.GeographicPoints[1] = topRightPointSheet;
                    sheet.ProjectedPoints[1] = CS2005.Transform(topRightPointSheet);

                    LatLonPoint bottomRightPointSheet = new LatLonPoint();
                    bottomRightPointSheet.Longitude = parentSheet.GeographicPoints[0].Longitude + sheetLengthIndex * parentSheetLength / sheetSize;
                    bottomRightPointSheet.Latitude = parentSheet.GeographicPoints[0].Latitude - sheetWidthIndex * parentSheetWidth / sheetSize;

                    sheet.GeographicPoints[2] = bottomRightPointSheet;
                    sheet.ProjectedPoints[2] = CS2005.Transform(bottomRightPointSheet);

                    LatLonPoint bottomLeftPointSheet = new LatLonPoint();
                    bottomLeftPointSheet.Longitude = parentSheet.GeographicPoints[0].Longitude + (sheetLengthIndex - 1) * parentSheetLength / sheetSize;
                    bottomLeftPointSheet.Latitude = parentSheet.GeographicPoints[0].Latitude - sheetWidthIndex * parentSheetWidth / sheetSize;

                    sheet.GeographicPoints[3] = bottomLeftPointSheet;
                    sheet.ProjectedPoints[3] = CS2005.Transform(bottomLeftPointSheet);

                    output.AppendFormat("LINE {0},{1} {2},{3} ", sheet.ProjectedPoints[0].Y, sheet.ProjectedPoints[0].X, sheet.ProjectedPoints[3].Y, sheet.ProjectedPoints[3].X);
                    output.AppendLine();
                    output.AppendFormat("LINE {0},{1} {2},{3} ", sheet.ProjectedPoints[0].Y, sheet.ProjectedPoints[0].X, sheet.ProjectedPoints[1].Y, sheet.ProjectedPoints[1].X);
                    output.AppendLine();

                    if (sheetWidthIndex == sheetSize)
                    {
                        output.AppendFormat("LINE {0},{1} {2},{3} ", sheet.ProjectedPoints[2].Y, sheet.ProjectedPoints[2].X, sheet.ProjectedPoints[3].Y, sheet.ProjectedPoints[3].X);
                        output.AppendLine();
                    }

                    if (sheetLengthIndex == sheetSize)
                    {
                        output.AppendFormat("LINE {0},{1} {2},{3} ", sheet.ProjectedPoints[1].Y, sheet.ProjectedPoints[1].X, sheet.ProjectedPoints[2].Y, sheet.ProjectedPoints[2].X);
                        output.AppendLine();
                    }

                    //output.Append("PLINE ");
                    //output.Append(string.Format("{0},{1} ", sheet1000.ProjectedPoints[0].Y, sheet1000.ProjectedPoints[0].X));
                    //output.Append(string.Format("{0},{1} ", sheet1000.ProjectedPoints[1].Y, sheet1000.ProjectedPoints[1].X));
                    //output.Append(string.Format("{0},{1} ", sheet1000.ProjectedPoints[2].Y, sheet1000.ProjectedPoints[2].X));
                    //output.Append(string.Format("{0},{1} ", sheet1000.ProjectedPoints[3].Y, sheet1000.ProjectedPoints[3].X));
                    //output.AppendLine("C");

                    double textRotationAngle = Math.Atan2(sheet.ProjectedPoints[1].Y - sheet.ProjectedPoints[0].Y, sheet.ProjectedPoints[1].X - sheet.ProjectedPoints[0].X) * 200 / Math.PI;

                    output.Append("TEXT ");
                    output.Append(string.Format("{0},{1} ", sheet.ProjectedPoints[3].Y + 25, sheet.ProjectedPoints[3].X + 25));
                    output.Append("5 ");
                    output.AppendFormat("{0} ", textRotationAngle);
                    output.AppendLine(string.Format("{0}", sheet.Number));
                }
            }

            return output.ToString();
        }
    }
}