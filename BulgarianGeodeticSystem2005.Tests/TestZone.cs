namespace Tests
{
    using System;
    using BulgarianGeodeticSystem2005.Data.Map;
    using NUnit.Framework;
    using BulgarianGeodeticSystem2005.Helpers;

    internal class TestZone
    {
        [TestCase(-1, 4)]
        [TestCase(0, 4)]
        [TestCase(8, -1)]
        [TestCase(8, 0)]
        [TestCase(17, 4)]
        public void GetRowByGridIndex_ThrowsArgumentOutOfRangeException_WhenInvalidArgumentPassed(int gridIndex, int gridSize)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ZoneHelper.GetRowByGridIndex(gridIndex, gridSize));
        }

        [TestCase(-1, 4)]
        [TestCase(0, 4)]
        [TestCase(8, -1)]
        [TestCase(8, 0)]
        [TestCase(17, 4)]
        public void GetColumnByGridIndex_ThrowsArgumentOutOfRangeException_WhenInvalidArgumentPassed(int gridIndex, int gridSize)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ZoneHelper.GetColumnByGridIndex(gridIndex, gridSize));
        }

        [TestCase(1000, 960)]
        [TestCase(5000, 192)]
        [TestCase(25000, 0)]
        [TestCase(100000, 12)]
        public void GetZoneSizeByMapScale_ShouldReturnCorrectResult(int mapScale, int zoneSize)
        {
            Assert.AreEqual(zoneSize, ZoneHelper.GetZoneSizeByMapScale(mapScale));
        }

        [TestCase(1000, 1, 1, "1-1-I")]
        [TestCase(1000, 1, 960, "12-16-V")]
        [TestCase(1000, 81, 960, "24-16-V")]
        [TestCase(1000, 960, 960, "144-256-XXV")]
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

        [TestCase(-5, 5, 5)]
        [TestCase(0, 5, 5)]
        [TestCase(6, 5, 5)]
        [TestCase(5, -1, 10)]
        [TestCase(5, 0, 10)]
        [TestCase(5, 11, 10)]
        [TestCase(5, 4, -2)]
        [TestCase(5, 4, 0)]
        public void GetGridIndex_ThrowsArgumentOutOfRangeException_WhenInvalidArgumentPassed(int row, int column, int gridSize)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ZoneHelper.GetGridIndex(row, column, gridSize));
        }
    }
}