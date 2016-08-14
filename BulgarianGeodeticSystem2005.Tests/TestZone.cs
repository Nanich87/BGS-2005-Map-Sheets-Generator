namespace Tests
{
    using System;
    using System.Linq;
    using BulgarianGeodeticSystem2005.Data.Map;
    using NUnit.Framework;

    internal class TestZone
    {
        [TestCase(1000, 960)]
        [TestCase(5000, 192)]
        [TestCase(25000, 0)]
        [TestCase(100000, 12)]
        public void GetZoneSizeByMapScale_ShouldReturnCorrectResult(int mapScale, int zoneSize)
        {
            Assert.AreEqual(zoneSize, Zone.GetZoneSizeByMapScale(mapScale));
        }

        [TestCase(1000, 5, 5, "1-1-XXV")]
        [TestCase(5000, 1, 32, "2-16")]
        [TestCase(100000, 12, 12, "144")]
        public void GetSheetNumber_ShouldReturnCorrectResult(int mapScale, int row, int column, string sheetNumber)
        {
            Assert.AreEqual(sheetNumber, Zone.GetSheetNumber(mapScale, row, column));
        }

        [TestCase(25000, 5, 5)]
        [TestCase(5000, 0, 32)]
        [TestCase(1000, 12, 961)]
        public void GetSheetNumber_ThrowsArgumentOutOfRangeException_WhenInvalidArgumentPassed(int mapScale, int row, int column)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Zone.GetSheetNumber(mapScale, row, column));
        }
    }
}