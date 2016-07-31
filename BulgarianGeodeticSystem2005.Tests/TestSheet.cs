namespace Tests
{
    using System;
    using System.Linq;
    using BulgarianGeodeticSystem2005.Data.Map;
    using NUnit.Framework;

    internal class TestSheet
    {
        [TestCase(1000, 960)]
        [TestCase(5000, 192)]
        [TestCase(25000, 0)]
        [TestCase(100000, 12)]
        public void GetRowSizeByScale_ShouldReturnCorrectResult(int scale, int rowSize)
        {
            Assert.AreEqual(rowSize, Sheet.GetRowSizeByScale(scale));
        }

        [TestCase(1000, 5, 5, "1-1-XXV")]
        [TestCase(5000, 1, 32, "2-16")]
        [TestCase(100000, 12, 12, "144")]
        public void GetSheetNumber_ShouldReturnCorrectResult(int scale, int row, int column, string number)
        {
            Assert.AreEqual(number, Sheet.GetSheetNumber(scale, row, column));
        }

        [TestCase(25000, 5, 5)]
        [TestCase(5000, 0, 32)]
        [TestCase(1000, 12, 961)]
        public void GetSheetNumber_ThrowsArgumentOutOfRangeException_WhenInvalidArgumentPassed(int scale, int row, int column)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Sheet.GetSheetNumber(scale, row, column));
        }
    }
}