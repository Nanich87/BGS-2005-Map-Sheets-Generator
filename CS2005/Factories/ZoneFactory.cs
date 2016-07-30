namespace CS2005.Factories
{
    using System;
    using System.Linq;
    using CS2005.Contracts;
    using CS2005.Data.Map;

    internal static class ZoneFactory
    {
        public static IZone CreateZone(int number)
        {
            switch (number)
            {
                case 34:
                    return new Zone(34, 44.0, 18.0);
                case 35:
                    return new Zone(35, 44.0, 24.0);
                default:
                    return null;
            }
        }
    }
}