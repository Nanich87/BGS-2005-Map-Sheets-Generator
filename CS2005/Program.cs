namespace CS2005
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Data;
    using Data.Map;
    using Data.Point;
    
    internal class Program
    {
        private static readonly Dictionary<int, string> sheetNomenclatureScale = new Dictionary<int, string>()
        {
            { 1, "I" }, { 2, "II" }, { 3, "III" }, { 4, "IV" }, { 5, "V" }, { 6, "VI" }, { 7, "VII" }, { 8, "VIII" }, { 9, "IX" }, { 10, "X" },
            { 11, "XI" }, { 12, "XII" }, { 13, "XIII" }, { 14, "XIV" }, { 15, "XV" }, { 16, "XVI" }, { 17, "XVII" }, { 18, "XVIII" }, { 19, "XIX" }, { 20, "XX" },
            { 21, "XXI" }, { 22, "XXII" }, { 23, "XXIII" }, { 24, "XXIV" }, { 25, "XXV" }
        };

        private static void Main(string[] args)
        {
            Console.Title = "БГС 2005 - генератор на картни листове";

            bool waitForInput = true;

            while (waitForInput)
            {
                Console.Write("Въведете N за генериране на картни листове или Е за изход: ");

                string choice = Console.ReadLine().ToLower();

                switch (choice)
                {
                    case "n":
                        Console.Write("Въведете номер на зона (34, 35): ");

                        int zone;

                        if (!int.TryParse(Console.ReadLine(), out zone))
                        {
                            Console.WriteLine("Невалиден номер на зона!");

                            break;
                        }

                        if (!CS2005.SupportedZones.Contains(zone))
                        {
                            Console.WriteLine("Зоната не се поддържа!");

                            break;
                        }

                        Console.Write("Въведете мащаб (100000, 5000, 1000): ");

                        int scale;

                        if (!int.TryParse(Console.ReadLine(), out scale))
                        {
                            Console.WriteLine("Невалиден мащаб на картните листове!");

                            break;
                        }

                        if (!CS2005.SupportedScales.Contains(scale))
                        {
                            Console.WriteLine("Мащабът не се поддържа!");

                            break;
                        }

                        string fileContent = GenerateSheets(zone, scale);

                        Console.Write("Въведете име на изходния SCR файл: ");
                        string fileName = Console.ReadLine();

                        File.WriteAllText(fileName, fileContent.ToString());

                        break;
                    case "e":
                        waitForInput = false;

                        break;
                }
            }
        }

        private static string GenerateSheets(int zone, int scale)
        {
            StringBuilder output = new StringBuilder();

            output.AppendLine("OSNAP OFF");
            output.AppendLine("-UNITS 2 4 3 4 300 Y");

            int sheetZone = 34;
            double globalStartingLatitude = 44;
            double globalStartingLongitude = 18;

            switch (zone)
            {
                case 34:

                    sheetZone = 34;
                    globalStartingLatitude = 44;
                    globalStartingLongitude = 18;

                    break;
                case 35:

                    sheetZone = 35;
                    globalStartingLatitude = 44;
                    globalStartingLongitude = 24;

                    break;
            }

            int sheetSize100000 = 12;

            double globalSheetWidth = 4.0;
            double globalSheetLength = 6.0;

            double sheetLength100000 = globalSheetLength / sheetSize100000;
            double sheetWidth100000 = globalSheetWidth / sheetSize100000;

            for (var sheetWidthIndex100000 = 1; sheetWidthIndex100000 <= sheetSize100000; sheetWidthIndex100000++)
            {
                for (var sheetLengthIndex100000 = 1; sheetLengthIndex100000 <= sheetSize100000; sheetLengthIndex100000++)
                {
                    int sheetNumber100000 = (sheetSize100000 * sheetWidthIndex100000) + sheetLengthIndex100000 - sheetSize100000;

                    Sheet sheet100000 = new Sheet(string.Format("K-{1}-{0}", sheetNumber100000, sheetZone), 100000, sheetSize100000);

                    LatLonPoint topLeftPointSheet100000 = new LatLonPoint();
                    topLeftPointSheet100000.Longitude = globalStartingLongitude + (sheetLengthIndex100000 - 1) * sheetLength100000;
                    topLeftPointSheet100000.Latitude = globalStartingLatitude - (sheetWidthIndex100000 - 1) * sheetWidth100000;

                    sheet100000.GeographicPoints[0] = topLeftPointSheet100000;
                    sheet100000.ProjectedPoints[0] = CS2005.Transform(topLeftPointSheet100000);

                    LatLonPoint topRightPointSheet100000 = new LatLonPoint();
                    topRightPointSheet100000.Longitude = globalStartingLongitude + sheetLengthIndex100000 * sheetLength100000;
                    topRightPointSheet100000.Latitude = globalStartingLatitude - (sheetWidthIndex100000 - 1) * sheetWidth100000;

                    sheet100000.GeographicPoints[1] = topRightPointSheet100000;
                    sheet100000.ProjectedPoints[1] = CS2005.Transform(topRightPointSheet100000);

                    LatLonPoint bottomRightPointSheet100000 = new LatLonPoint();
                    bottomRightPointSheet100000.Longitude = globalStartingLongitude + sheetLengthIndex100000 * sheetLength100000;
                    bottomRightPointSheet100000.Latitude = globalStartingLatitude - sheetWidthIndex100000 * sheetWidth100000;

                    sheet100000.GeographicPoints[2] = bottomRightPointSheet100000;
                    sheet100000.ProjectedPoints[2] = CS2005.Transform(bottomRightPointSheet100000);

                    LatLonPoint bottomLeftPointSheet100000 = new LatLonPoint();
                    bottomLeftPointSheet100000.Longitude = globalStartingLongitude + (sheetLengthIndex100000 - 1) * sheetLength100000;
                    bottomLeftPointSheet100000.Latitude = globalStartingLatitude - sheetWidthIndex100000 * sheetWidth100000;

                    sheet100000.GeographicPoints[3] = bottomLeftPointSheet100000;
                    sheet100000.ProjectedPoints[3] = CS2005.Transform(bottomLeftPointSheet100000);

                    if (scale == 100000)
                    {
                        output.AppendFormat("LINE {0},{1} {2},{3} ", sheet100000.ProjectedPoints[0].Y, sheet100000.ProjectedPoints[0].X, sheet100000.ProjectedPoints[3].Y, sheet100000.ProjectedPoints[3].X);
                        output.AppendLine();
                        output.AppendFormat("LINE {0},{1} {2},{3} ", sheet100000.ProjectedPoints[0].Y, sheet100000.ProjectedPoints[0].X, sheet100000.ProjectedPoints[1].Y, sheet100000.ProjectedPoints[1].X);
                        output.AppendLine();

                        if (sheetWidthIndex100000 == sheetSize100000)
                        {
                            output.AppendFormat("LINE {0},{1} {2},{3} ", sheet100000.ProjectedPoints[2].Y, sheet100000.ProjectedPoints[2].X, sheet100000.ProjectedPoints[3].Y, sheet100000.ProjectedPoints[3].X);
                            output.AppendLine();
                        }

                        if (sheetLengthIndex100000 == sheetSize100000)
                        {
                            output.AppendFormat("LINE {0},{1} {2},{3} ", sheet100000.ProjectedPoints[1].Y, sheet100000.ProjectedPoints[1].X, sheet100000.ProjectedPoints[2].Y, sheet100000.ProjectedPoints[2].X);
                            output.AppendLine();
                        }

                        //output.Append("PLINE ");
                        //output.Append(string.Format("{0},{1} ", sheet100000.ProjectedPoints[0].Y, sheet100000.ProjectedPoints[0].X));
                        //output.Append(string.Format("{0},{1} ", sheet100000.ProjectedPoints[1].Y, sheet100000.ProjectedPoints[1].X));
                        //output.Append(string.Format("{0},{1} ", sheet100000.ProjectedPoints[2].Y, sheet100000.ProjectedPoints[2].X));
                        //output.Append(string.Format("{0},{1} ", sheet100000.ProjectedPoints[3].Y, sheet100000.ProjectedPoints[3].X));
                        //output.AppendLine("C");
                    }
                    else if (scale == 5000 || scale == 1000)
                    {
                        int sheetSize5000 = 16;

                        double sheetLength5000 = sheetLength100000 / sheetSize5000;
                        double sheetWidth5000 = sheetWidth100000 / sheetSize5000;

                        for (var sheetWidthIndex5000 = 1; sheetWidthIndex5000 <= sheetSize5000; sheetWidthIndex5000++)
                        {
                            for (var sheetLengthIndex5000 = 1; sheetLengthIndex5000 <= sheetSize5000; sheetLengthIndex5000++)
                            {
                                var sheetNumber5000 = (sheetSize5000 * sheetWidthIndex5000) + sheetLengthIndex5000 - sheetSize5000;

                                Sheet sheet5000 = new Sheet(string.Format("K-{2}-{0}-({1})", sheetNumber100000, sheetNumber5000, sheetZone), 5000, sheetSize5000);

                                LatLonPoint topLeftPointSheet5000 = new LatLonPoint();
                                topLeftPointSheet5000.Longitude = sheet100000.GeographicPoints[0].Longitude + (sheetLengthIndex5000 - 1) * sheetLength100000 / sheetSize5000;
                                topLeftPointSheet5000.Latitude = sheet100000.GeographicPoints[0].Latitude - (sheetWidthIndex5000 - 1) * sheetWidth100000 / sheetSize5000;

                                sheet5000.GeographicPoints[0] = topLeftPointSheet5000;
                                sheet5000.ProjectedPoints[0] = CS2005.Transform(topLeftPointSheet5000);

                                LatLonPoint topRightPointSheet5000 = new LatLonPoint();
                                topRightPointSheet5000.Longitude = sheet100000.GeographicPoints[0].Longitude + sheetLengthIndex5000 * sheetLength100000 / sheetSize5000;
                                topRightPointSheet5000.Latitude = sheet100000.GeographicPoints[0].Latitude - (sheetWidthIndex5000 - 1) * sheetWidth100000 / sheetSize5000;

                                sheet5000.GeographicPoints[1] = topRightPointSheet5000;
                                sheet5000.ProjectedPoints[1] = CS2005.Transform(topRightPointSheet5000);

                                LatLonPoint bottomRightPointSheet5000 = new LatLonPoint();
                                bottomRightPointSheet5000.Longitude = sheet100000.GeographicPoints[0].Longitude + sheetLengthIndex5000 * sheetLength100000 / sheetSize5000;
                                bottomRightPointSheet5000.Latitude = sheet100000.GeographicPoints[0].Latitude - sheetWidthIndex5000 * sheetWidth100000 / sheetSize5000;

                                sheet5000.GeographicPoints[2] = bottomRightPointSheet5000;
                                sheet5000.ProjectedPoints[2] = CS2005.Transform(bottomRightPointSheet5000);

                                LatLonPoint bottomLeftPointSheet5000 = new LatLonPoint();
                                bottomLeftPointSheet5000.Longitude = sheet100000.GeographicPoints[0].Longitude + (sheetLengthIndex5000 - 1) * sheetLength100000 / sheetSize5000;
                                bottomLeftPointSheet5000.Latitude = sheet100000.GeographicPoints[0].Latitude - sheetWidthIndex5000 * sheetWidth100000 / sheetSize5000;

                                sheet5000.GeographicPoints[3] = bottomLeftPointSheet5000;
                                sheet5000.ProjectedPoints[3] = CS2005.Transform(bottomLeftPointSheet5000);

                                if (scale == 5000)
                                {
                                    output.AppendFormat("LINE {0},{1} {2},{3} ", sheet5000.ProjectedPoints[0].Y, sheet5000.ProjectedPoints[0].X, sheet5000.ProjectedPoints[3].Y, sheet5000.ProjectedPoints[3].X);
                                    output.AppendLine();
                                    output.AppendFormat("LINE {0},{1} {2},{3} ", sheet5000.ProjectedPoints[0].Y, sheet5000.ProjectedPoints[0].X, sheet5000.ProjectedPoints[1].Y, sheet5000.ProjectedPoints[1].X);
                                    output.AppendLine();

                                    if (sheetWidthIndex5000 == sheetSize5000)
                                    {
                                        output.AppendFormat("LINE {0},{1} {2},{3} ", sheet5000.ProjectedPoints[2].Y, sheet5000.ProjectedPoints[2].X, sheet5000.ProjectedPoints[3].Y, sheet5000.ProjectedPoints[3].X);
                                        output.AppendLine();
                                    }

                                    if (sheetLengthIndex5000 == sheetSize5000)
                                    {
                                        output.AppendFormat("LINE {0},{1} {2},{3} ", sheet5000.ProjectedPoints[1].Y, sheet5000.ProjectedPoints[1].X, sheet5000.ProjectedPoints[2].Y, sheet5000.ProjectedPoints[2].X);
                                        output.AppendLine();
                                    }

                                    //output.Append("PLINE ");
                                    //output.Append(string.Format("{0},{1} ", sheet5000.ProjectedPoints[0].Y, sheet5000.ProjectedPoints[0].X));
                                    //output.Append(string.Format("{0},{1} ", sheet5000.ProjectedPoints[1].Y, sheet5000.ProjectedPoints[1].X));
                                    //output.Append(string.Format("{0},{1} ", sheet5000.ProjectedPoints[2].Y, sheet5000.ProjectedPoints[2].X));
                                    //output.Append(string.Format("{0},{1} ", sheet5000.ProjectedPoints[3].Y, sheet5000.ProjectedPoints[3].X));
                                    //output.AppendLine("C");

                                    double textRotationAngle = Math.Atan2(sheet5000.ProjectedPoints[1].Y - sheet5000.ProjectedPoints[0].Y, sheet5000.ProjectedPoints[1].X - sheet5000.ProjectedPoints[0].X) * 200 / Math.PI;

                                    output.Append("TEXT ");
                                    output.Append(string.Format("{0},{1} ", sheet5000.ProjectedPoints[3].Y + 25, sheet5000.ProjectedPoints[3].X + 25));
                                    output.Append("100 ");
                                    output.AppendFormat("{0} ", textRotationAngle);
                                    output.AppendLine(string.Format("{0}", sheet5000.Number));
                                }
                                else if (scale == 1000)
                                {
                                    int sheetSize1000 = 5;
                                    for (var sheetWidthIndex1000 = 1; sheetWidthIndex1000 <= sheetSize1000; sheetWidthIndex1000++)
                                    {
                                        for (var sheetLengthIndex1000 = 1; sheetLengthIndex1000 <= sheetSize1000; sheetLengthIndex1000++)
                                        {
                                            var sheetNumber1000 = (sheetSize1000 * sheetWidthIndex1000) + sheetLengthIndex1000 - sheetSize1000;

                                            Sheet sheet1000 = new Sheet(string.Format("K-{2}-{0}-({1})-{3}", sheetNumber100000, sheetNumber5000, sheetZone, sheetNomenclatureScale[sheetNumber1000]), 1000, sheetSize1000);

                                            LatLonPoint topLeftPointSheet1000 = new LatLonPoint();
                                            topLeftPointSheet1000.Longitude = sheet5000.GeographicPoints[0].Longitude + (sheetLengthIndex1000 - 1) * sheetLength5000 / sheetSize1000;
                                            topLeftPointSheet1000.Latitude = sheet5000.GeographicPoints[0].Latitude - (sheetWidthIndex1000 - 1) * sheetWidth5000 / sheetSize1000;

                                            sheet1000.GeographicPoints[0] = topLeftPointSheet1000;
                                            sheet1000.ProjectedPoints[0] = CS2005.Transform(topLeftPointSheet1000);

                                            LatLonPoint topRightPointSheet1000 = new LatLonPoint();
                                            topRightPointSheet1000.Longitude = sheet5000.GeographicPoints[0].Longitude + sheetLengthIndex1000 * sheetLength5000 / sheetSize1000;
                                            topRightPointSheet1000.Latitude = sheet5000.GeographicPoints[0].Latitude - (sheetWidthIndex1000 - 1) * sheetWidth5000 / sheetSize1000;

                                            sheet1000.GeographicPoints[1] = topRightPointSheet1000;
                                            sheet1000.ProjectedPoints[1] = CS2005.Transform(topRightPointSheet1000);

                                            LatLonPoint bottomRightPointSheet1000 = new LatLonPoint();
                                            bottomRightPointSheet1000.Longitude = sheet5000.GeographicPoints[0].Longitude + sheetLengthIndex1000 * sheetLength5000 / sheetSize1000;
                                            bottomRightPointSheet1000.Latitude = sheet5000.GeographicPoints[0].Latitude - sheetWidthIndex1000 * sheetWidth5000 / sheetSize1000;

                                            sheet1000.GeographicPoints[2] = bottomRightPointSheet1000;
                                            sheet1000.ProjectedPoints[2] = CS2005.Transform(bottomRightPointSheet1000);

                                            LatLonPoint bottomLeftPointSheet1000 = new LatLonPoint();
                                            bottomLeftPointSheet1000.Longitude = sheet5000.GeographicPoints[0].Longitude + (sheetLengthIndex1000 - 1) * sheetLength5000 / sheetSize1000;
                                            bottomLeftPointSheet1000.Latitude = sheet5000.GeographicPoints[0].Latitude - sheetWidthIndex1000 * sheetWidth5000 / sheetSize1000;

                                            sheet1000.GeographicPoints[3] = bottomLeftPointSheet1000;
                                            sheet1000.ProjectedPoints[3] = CS2005.Transform(bottomLeftPointSheet1000);

                                            output.AppendFormat("LINE {0},{1} {2},{3} ", sheet1000.ProjectedPoints[0].Y, sheet1000.ProjectedPoints[0].X, sheet1000.ProjectedPoints[3].Y, sheet1000.ProjectedPoints[3].X);
                                            output.AppendLine();
                                            output.AppendFormat("LINE {0},{1} {2},{3} ", sheet1000.ProjectedPoints[0].Y, sheet1000.ProjectedPoints[0].X, sheet1000.ProjectedPoints[1].Y, sheet1000.ProjectedPoints[1].X);
                                            output.AppendLine();

                                            if (sheetWidthIndex1000 == sheetSize1000)
                                            {
                                                output.AppendFormat("LINE {0},{1} {2},{3} ", sheet1000.ProjectedPoints[2].Y, sheet1000.ProjectedPoints[2].X, sheet1000.ProjectedPoints[3].Y, sheet1000.ProjectedPoints[3].X);
                                                output.AppendLine();
                                            }

                                            if (sheetLengthIndex1000 == sheetSize1000)
                                            {
                                                output.AppendFormat("LINE {0},{1} {2},{3} ", sheet1000.ProjectedPoints[1].Y, sheet1000.ProjectedPoints[1].X, sheet1000.ProjectedPoints[2].Y, sheet1000.ProjectedPoints[2].X);
                                                output.AppendLine();
                                            }

                                            //output.Append("PLINE ");
                                            //output.Append(string.Format("{0},{1} ", sheet1000.ProjectedPoints[0].Y, sheet1000.ProjectedPoints[0].X));
                                            //output.Append(string.Format("{0},{1} ", sheet1000.ProjectedPoints[1].Y, sheet1000.ProjectedPoints[1].X));
                                            //output.Append(string.Format("{0},{1} ", sheet1000.ProjectedPoints[2].Y, sheet1000.ProjectedPoints[2].X));
                                            //output.Append(string.Format("{0},{1} ", sheet1000.ProjectedPoints[3].Y, sheet1000.ProjectedPoints[3].X));
                                            //output.AppendLine("C");

                                            double textRotationAngle = Math.Atan2(sheet1000.ProjectedPoints[1].Y - sheet1000.ProjectedPoints[0].Y, sheet1000.ProjectedPoints[1].X - sheet1000.ProjectedPoints[0].X) * 200 / Math.PI;

                                            output.Append("TEXT ");
                                            output.Append(string.Format("{0},{1} ", sheet1000.ProjectedPoints[3].Y + 25, sheet1000.ProjectedPoints[3].X + 25));
                                            output.Append("5 ");
                                            output.AppendFormat("{0} ", textRotationAngle);
                                            output.AppendLine(string.Format("{0}", sheet1000.Number));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return output.ToString();
        }
    }
}