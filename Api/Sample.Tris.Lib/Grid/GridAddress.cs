namespace Sample.Tris.Lib.Grid
{
    using System;

    /// <summary>
    ///
    /// </summary>
    public class GridAddress
    {
        const string POSITION_REGEX_MATCH = "^([A-Z]{1,3})([1-9][0-9]*)$";

        /// <summary>
        /// Defines an address label and abstract row,col for addressing grid elements.
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
