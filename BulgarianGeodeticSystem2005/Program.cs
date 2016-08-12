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

                        string fileContent = CoordinateSystem2005.GenerateSheets(zone, inputMapScale);

                        Console.Write("Въведете име на изходния SCR файл: ");
                        string outputFileName = Console.ReadLine();

                        File.WriteAllText(string.Format("{0}.scr", outputFileName), fileContent.ToString());

                        Console.Write("Въведете име на входен файл с формат PNE(ZD) или натиснете Enter за да продължите: ");
                        string inputFileName = Console.ReadLine();

                        if (File.Exists(inputFileName))
                        {
                            var points = CoordinateSystem2005.OpenFile(inputFileName);

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

                            File.WriteAllText(string.Format("{0}.txt", outputFileName), pointContent);
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