namespace BulgarianGeodeticSystem2005.Helpers
{
    using System;

    internal static class ZoneHelper
    {
        public static int GetGridSizeByMapScale(int mapScale)
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

        public static bool InsideGrid(int mapScale, int gridPosition)
        {
            switch (mapScale)
            {
                case 1000:
                    return gridPosition > 0 && gridPosition <= 960;
                case 5000:
                    return gridPosition > 0 && gridPosition <= 192;
                case 100000:
                    return gridPosition > 0 && gridPosition <= 12;
                default:
                    return false;
            }
        }

        public static int GetParentField(int fieldIndex, int gridSize)
        {
            return (int)Math.Ceiling(fieldIndex / (double)gridSize);
        }

        public static int GetChildField(int fieldIndex, int gridSize)
        {
            return fieldIndex % gridSize > 0 ? fieldIndex % gridSize : gridSize;
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
    }
}