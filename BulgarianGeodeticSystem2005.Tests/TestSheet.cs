namespace Tests
{
    using System;
    using System.Linq;
    using BulgarianGeodeticSystem2005.Data.Map;
    using BulgarianGeodeticSystem2005.Data.Point;
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

        [TestCase(4815061.6152, 8593.9951, true)]
        [TestCase(4813820.1790, 9590.1300, true)]
        [TestCase(4813613.5016, 12238.7067, false)]
        public void ContainsPoint_ShouldReturnCorrectResult(double x, double y, bool result)
        {
            Sheet sheet = new Sheet("K-34-27-31", 5000, 192);
            sheet.ProjectedPoints[0] = new XYPoint(4815061.6152, 8593.9951);
            sheet.ProjectedPoints[1] = new XYPoint(4814880.4324, 11122.7055);
            sheet.ProjectedPoints[2] = new XYPoint(4812571.7799, 10957.7188);
            sheet.ProjectedPoints[3] = new XYPoint(4812753.0238, 8428.1550);

            Assert.AreEqual(result, sheet.ContainsPoint(new XYPoint(x, y)));
        }
    }
}