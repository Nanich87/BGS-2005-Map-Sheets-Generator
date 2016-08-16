namespace BulgarianGeodeticSystem2005.Helpers
{
    using System;

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

        public static int GetRowByGridIndex(int gridIndex, int gridSize)
        {
            if (gridSize <= 0)
            {
                throw new ArgumentOutOfRangeException("gridSize", "Invalid grid size!");
            }

            if (gridIndex <= 0)
            {
                throw new ArgumentOutOfRangeException("gridIndex", "Invalid grid index!");
            }

            if (gridIndex > Math.Pow(gridSize, 2))
            {
                throw new ArgumentOutOfRangeException("gridIndex", "Invalid grid index!");
            }

            int row = (int)Math.Ceiling(gridIndex / (double)gridSize);

            return row;
        }

        public static int GetColumnByGridIndex(int gridIndex, int gridSize)
        {
            if (gridSize <= 0)
            {
                throw new ArgumentOutOfRangeException("gridSize", "Invalid grid size!");
            }

            if (gridIndex <= 0)
            {
                throw new ArgumentOutOfRangeException("gridIndex", "Invalid grid index!");
            }

            if (gridIndex > Math.Pow(gridSize, 2))
            {
                throw new ArgumentOutOfRangeException("gridIndex", "Invalid grid index!");
            }

            int column = gridIndex % gridSize > 0 ? gridIndex % gridSize : gridSize;

            return column;
        }

        public static int GetGridIndex(int row, int column, int gridSize)
        {
            if (gridSize <= 0)
            {
                throw new ArgumentOutOfRangeException("gridSize", "Invalid grid size!");
            }

            if (row <= 0 || row > gridSize)
            {
                throw new ArgumentOutOfRangeException("row", "Invalid row!");
            }

            if (column <= 0 || column > gridSize)
            {
                throw new ArgumentOutOfRangeException("column", "Invalid column!");
            }

            int gridIndex = (row * gridSize) + column - gridSize;

            return gridIndex;
        }

        public static int GetChildGridIndex(int parentRow, int parentColumn, int childGridSize)
        {
            if (childGridSize <= 0)
            {
                throw new ArgumentOutOfRangeException("childGridSize", "Invalid child grid size!");
            }

            if (parentRow <= 0)
            {
                throw new ArgumentOutOfRangeException("parentRow", "Invalid parent row!");
            }

            if (parentColumn <= 0)
            {
                throw new ArgumentOutOfRangeException("parentColumn", "Invalid parent column!");
            }

            double reducedChildRow = parentRow / childGridSize;
            reducedChildRow = Math.Ceiling(reducedChildRow);

            int childRow = (int)(parentRow - (childGridSize * (reducedChildRow - 1)));

            double reducedChildColumn = parentColumn / childGridSize;
            reducedChildColumn = Math.Ceiling(reducedChildColumn);

            int childColumn = (int)(parentColumn - (childGridSize * (reducedChildColumn - 1)));

            int gridIndex = (childRow * childGridSize) + childColumn - childGridSize;

            return gridIndex;
        }
    }
}
