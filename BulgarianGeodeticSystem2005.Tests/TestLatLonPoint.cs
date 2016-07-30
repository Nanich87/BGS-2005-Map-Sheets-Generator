namespace Tests
{
    using System;
    using System.Linq;
    using BulgarianGeodeticSystem2005.Data.Point;
    using NUnit.Framework;

    internal class TestLatLonPoint
    {
        [Test]
        public void LatLonPoint_ShouldThrowArgumentOutOfRangeException_WhenInvalidLatitude()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new LatLonPoint(-100, 25));
        }

        [Test]
        public void LatLonPoint_ShouldThrowArgumentOutOfRangeException_WhenInvalidLongitude()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new LatLonPoint(42, -181));
        }
    }
}