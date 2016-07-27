namespace Tests
{
    using System;
    using CS2005.Data.Point;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Transform_ShouldReturnCorrectPointCoordinates()
        {
            LatLonPoint geographicPoint = new LatLonPoint(42.466595, 23.798886);
            XYPoint projectedPoint = new XYPoint(4704874.1632373752, 360113.72085936996);

            XYPoint result = CS2005.Data.CS2005.Transform(geographicPoint);

            Assert.AreEqual(projectedPoint, result);
        }
    }
}
