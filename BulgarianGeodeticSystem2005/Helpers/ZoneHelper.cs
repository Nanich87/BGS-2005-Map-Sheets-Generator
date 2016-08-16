namespace BulgarianGeodeticSystem2005.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal static class ZoneHelper
    {
        public static int GetZoneSizeByMapScale(int mapScale)
        {
            switch (mapScale)
            {
                case 1000:
                    return 960;
                case 5000:
                    return 192;
                case 100000:
                    return 12;
                default:
                    // throw new ArgumentOutOfRangeException("mapScale", "Invalid map scale!");

                    return 0;
            }
        }

        public static bool IsValidSheetPosition(int mapScale, int sheetPosition)
        {
            switch (mapScale)
            {
                case 1000:
                    return sheetPosition > 0 && sheetPosition <= 960;
                case 5000:
                    return sheetPosition > 0 && sheetPosition <= 192;
                case 100000:
                    return sheetPosition > 0 && sheetPosition <= 12;
                default:
                    return false;
            }
        }

        public static int ReduceParentField(int field, int value)
        {
            return (int)Math.Ceiling(field / (double)value);
        }

        public static int ReduceChildField(int field, int value)
        {
            return field % value > 0 ? field % value : value;
        }

        public static int GetRowBySheetIndex(int sheetIndex, int sheetSize)
        {
            int row = (int)Math.Ceiling(sheetIndex / (double)sheetSize);

            return row;
        }

        public static int GetColumnBySheetIndex(int sheetIndex, int sheetSize)
        {
            int column = sheetIndex % sheetSize > 0 ? sheetIndex % sheetSize : sheetSize;

            return column;
        }

        public static int GetSheetIndex(int row, int column, int sheetSize)
        {
            if (sheetSize <= 0)
            {
                throw new ArgumentOutOfRangeException("sheetSize", "Invalid sheet size!");
            }

            if (row <= 0 || row > sheetSize)
            {
                throw new ArgumentOutOfRangeException("row", "Invalid row!");
            }

            if (column <= 0 || column > sheetSize)
            {
                throw new ArgumentOutOfRangeException("column", "Invalid column!");
            }

            int sheetIndex = (row * sheetSize) + column - sheetSize;

            return sheetIndex;
        }

        public static int GetSheetSubIndex(int parentRow, int parentColumn, int childSheetSize)
        {
            if (childSheetSize <= 0)
            {
                throw new ArgumentOutOfRangeException("sheetSize", "Invalid sheet size!");
            }

            if (parentRow <= 0)
            {
                throw new ArgumentOutOfRangeException("row", "Invalid row!");
            }

            if (parentColumn <= 0)
            {
                throw new ArgumentOutOfRangeException("column", "Invalid column!");
            }

            double reducedChildRow = parentRow / childSheetSize;
            reducedChildRow = Math.Ceiling(reducedChildRow);

            int childRow = (int)(parentRow - (childSheetSize * (reducedChildRow - 1)));

            double reducedChildColumn = parentColumn / childSheetSize;
            reducedChildColumn = Math.Ceiling(reducedChildColumn);

            int childColumn = (int)(parentColumn - (childSheetSize * (reducedChildColumn - 1)));

            int sheetIndex = (childRow * childSheetSize) + childColumn - childSheetSize;

            return sheetIndex;
        }
    }
}
