namespace CS2005
{
    using System;
    using System.IO;
    using System.Linq;
    using CS2005.Contracts;
    using CS2005.Factories;
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

                        int zone;

                        if (!int.TryParse(Console.ReadLine(), out zone))
                        {
                            Console.WriteLine("Невалиден номер на зона!");

                            break;
                        }

                        if (!CoordinateSystem2005.SupportedZones.Contains(zone))
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

                        if (!CoordinateSystem2005.SupportedScales.Contains(scale))
                        {
                            Console.WriteLine("Мащабът не се поддържа!");

                            break;
                        }

                        IZone mapZone = ZoneFactory.CreateZone(zone);
                        string fileContent = CoordinateSystem2005.GenerateSheets(mapZone, scale);

                        Console.Write("Въведете име на изходния SCR файл: ");
                        string fileName = Console.ReadLine();

                        File.WriteAllText(string.Format("{0}.scr", fileName), fileContent.ToString());

                        break;
                    case "e":
                        waitForInput = false;

                        break;
                }
            }
        }
    }
}