namespace BulgarianGeodeticSystem2005
{
    using System;
    using System.IO;
    using System.Linq;
    using BulgarianGeodeticSystem2005.Contracts;
    using BulgarianGeodeticSystem2005.Factories;
    using Data;

    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.Title = "Bulgarian Geodetic System 2005";

            bool waitForInput = true;

            while (waitForInput)
            {
                Console.Write("Въведете N за генериране на картни листове или Е за изход: ");

                string choice = Console.ReadLine().ToLower();

                switch (choice)
                {
                    case "n":
                        Console.Write("Въведете номер на зона (34, 35): ");

                        int inputZoneNumber;

                        if (!int.TryParse(Console.ReadLine(), out inputZoneNumber))
                        {
                            Console.WriteLine("Невалиден номер на зона!");

                            break;
                        }

                        if (!CoordinateSystem2005.SupportedZones.Contains(inputZoneNumber))
                        {
                            Console.WriteLine("Зоната не се поддържа!");

                            break;
                        }

                        Console.Write("Въведете мащаб (100000, 5000, 1000): ");

                        int inputMapScale;

                        if (!int.TryParse(Console.ReadLine(), out inputMapScale))
                        {
                            Console.WriteLine("Невалиден мащаб на картните листове!");

                            break;
                        }

                        if (!CoordinateSystem2005.SupportedScales.Contains(inputMapScale))
                        {
                            Console.WriteLine("Мащабът не се поддържа!");

                            break;
                        }

                        IZone zone = ZoneFactory.CreateZone(inputZoneNumber);

                        CoordinateSystem2005.GenerateSheets(zone, inputMapScale);

                        Console.Write("Въведете име на изходния SCR файл или натиснете Enter за да продължите: ");
                        string scriptFileName = Console.ReadLine();

                        if (scriptFileName != string.Empty)
                        {
                            string fileContent = CoordinateSystem2005.ExportSheets();

                            File.WriteAllText(string.Format("{0}.scr", scriptFileName), fileContent.ToString());

                            Console.WriteLine(string.Format("Файлът {0}.scr беше записан успешно на диска.", scriptFileName));
                        }

                        Console.WriteLine("ПРИВЪРЗВАНЕ НА ТОЧКИ КЪМ КАРТНИ ЛИСТОВЕ");
                        Console.Write("Въведете име на входен файл с формат PNE(ZD) или натиснете Enter за да продължите: ");
                        string pointFileName = Console.ReadLine();

                        if (File.Exists(pointFileName))
                        {
                            var points = CoordinateSystem2005.OpenFile(pointFileName);

                            foreach (var point in points)
                            {
                                foreach (var sheet in CoordinateSystem2005.Sheets)
                                {
                                    if (sheet.ContainsPoint(point))
                                    {
                                        point.Description = string.Format("{0} {1}", point.Description, sheet.Number);
                                    }
                                }
                            }

                            string pointContent = string.Join(Environment.NewLine, points.Select(p => p.Description));

                            string newPointFileName = string.Format("{0}_sheets.txt", Path.GetFileNameWithoutExtension(pointFileName));

                            File.WriteAllText(string.Format("{0}.txt", newPointFileName), pointContent);

                            Console.WriteLine(string.Format("Файлът {0} беше записан успешно на диска.", newPointFileName));
                        }

                        break;
                    case "e":
                        waitForInput = false;

                        break;
                }
            }
        }
    }
}