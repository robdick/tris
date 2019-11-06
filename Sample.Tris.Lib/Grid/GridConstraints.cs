namespace Sample.Tris.Lib.Grid
{
    using System;

    /// <summary>
    /// Defines a constrained 2d grid representation to perform spatial query operations upon.
    /// </summary>
    public class GridConstraints
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="colCount"></param>
        /// <param name="cellSpan"></param>
        public GridConstraints(uint rowCount, uint columnCount, uint cellSpan)
        {
            if (rowCount < 1)
            {
                throw new ArgumentException("rowCount should be greater than or equal to 1.", "rowCount");
            }

            if (columnCount < 1)
            {
                throw new ArgumentException("columnCount should be greater than or equal to 1.", "columnCount");
            }

            if (cellSpan < 1)
            {
                throw new ArgumentException("cellSpan should be greater than or equal to 1.", "cellSpan");
            }

            RowCount = rowCount;
            ColumnCount = columnCount;
            CellSpan = cellSpan;

            Width = CellSpan * ColumnCount;
            Height = CellSpan * RowCount;
        }

        /// <summary>
        ///
        /// </summary>
        /// <value></value>
        public uint RowCount { get; }

        /// <summary>
        ///
        /// </summary>
        /// <value></value>
        public uint ColumnCount { get; }

        /// <summary>
        ///
        /// </summary>
        /// <value></value>
        public uint CellSpan { get; }

        /// <summary>
        ///
        /// </summary>
        public uint Width { get; }

        /// <summary>
        ///
        /// </summary>
        public uint Height { get; }
    }
}
