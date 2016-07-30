namespace Tests
{
    using System;
    using CS2005.Data;
    using CS2005.Data.Point;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Transform_ShouldReturnCorrectResult_WhenArgumentIsValidGeographicPoint()
        {
            LatLonPoint geographicPoint = new LatLonPoint(42.466595, 23.798886);
            XYPoint projectedPoint = new XYPoint(4704874.1632373752, 360113.72085936996);

            XYPoint result = CoordinateSystem2005.Transform(geographicPoint);

            Assert.AreEqual(projectedPoint, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transform_ShouldThrowNullArgumentException_WhenNullArgumentPassed()
        {
            XYPoint result = CoordinateSystem2005.Transform(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void LatLonPoint_ShouldThrowArgumentOutOfRangeException_WhenInvalidLatitude()
        {
            LatLonPoint geographicPoint = new LatLonPoint(-100, 25);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void LatLonPoint_ShouldThrowArgumentOutOfRangeException_WhenInvalidLongitude()
        {
            LatLonPoint geographicPoint = new LatLonPoint(42, -181);
        }
    }
}
