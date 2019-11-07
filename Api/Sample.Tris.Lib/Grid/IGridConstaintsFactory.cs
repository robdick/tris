namespace Sample.Tris.Lib.Grid
{
    /// <summary>
    ///
    /// </summary>
    public interface IGridConstraintsFactory
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="colCount"></param>
        /// <param name="cellSpan"></param>
        /// <returns></returns>
        GridConstraints GetConstraints();
    }
}
