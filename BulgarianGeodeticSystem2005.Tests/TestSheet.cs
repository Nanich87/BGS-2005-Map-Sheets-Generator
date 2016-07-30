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
        public void SheetGetRowSizeByScale_ShouldReturnCorrectResult(int scale, int rowSize)
        {
            Assert.AreEqual(rowSize, Sheet.GetRowSizeByScale(scale));
        }
    }
}