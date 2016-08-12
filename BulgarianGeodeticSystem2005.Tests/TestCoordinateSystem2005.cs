namespace Tests
{
    using System;
    using System.IO;
    using BulgarianGeodeticSystem2005.Data;
    using BulgarianGeodeticSystem2005.Data.Point;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MSTestExtensions;

    [TestClass]
    public class TestCoordinateSystem2005
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
        public void Transform_ShouldThrowNullArgumentException_WhenNullArgumentPassed()
        {
            ThrowsAssert.Throws<ArgumentNullException>(() => CoordinateSystem2005.Transform(null));
        }

        [TestMethod]
        public void OpenFile_ShouldThrowFileNotFoundException_WhenFileDoesNotExists()
        {
            ThrowsAssert.Throws<FileNotFoundException>(() => CoordinateSystem2005.OpenFile(string.Empty));
        }
    }
}