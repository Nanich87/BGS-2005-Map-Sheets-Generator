namespace BulgarianGeodeticSystem2005.Contracts
{
    internal interface IZone
    {
        int Number
        {
            get;
        }

        double StartingLatitude
        {
            get;
        }

        double StartingLongitude
        {
            get;
        }
    }
}