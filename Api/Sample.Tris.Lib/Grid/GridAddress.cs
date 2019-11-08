namespace Sample.Tris.Lib.Grid
{
    using System;

    /// <summary>
    /// Defines a triangle grid reference position
    /// </summary>
    /// <example>
    /// Triangle references are limited to row positions [A..ZZZ] (note upper case) and column positions [1.99].
    /// <code>
    /// // Valid references
    /// var ref = TriangleGridRef.Create("A1");
    /// ref.Row; // 'A'
    /// ref.Column; // 1
    ///
    /// var ref = TriangleGridRef.Create("a1"); // throws InvalidCompositeKeyFormatException
    /// </code>
    /// </example>
    public class GridAddress
    {
        const string POSITION_REGEX_MATCH = "^([A-Z]{1,3})([1-9][0-9]*)$";

        /// <summary>
        ///
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="label"></param>
        public GridAddress(int row, int column, string label)
        {
            if (row < 1)
            {
                throw new ArgumentException("row must be 1 or greater", "row");
            }

            if (column < 1)
            {
                throw new ArgumentException("column must be 1 or greater", "column");
            }

            if (string.IsNullOrWhiteSpace(label))
            {
                throw new ArgumentNullException("label");
            }

            Row = row;
            Column = column;
            Label = label;
        }

        /// <summary>
        ///
        /// </summary>
        /// <value></value>
        public string Label { get; }

        /// <summary>
        ///
        /// </summary>
        /// <value></value>
        public int Row { get; }

        /// <summary>
        ///
        /// </summary>
        /// <value></value>
        public int Column { get; }
    }
}
