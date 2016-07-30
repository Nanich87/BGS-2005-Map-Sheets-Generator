﻿namespace BulgarianGeodeticSystem2005
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