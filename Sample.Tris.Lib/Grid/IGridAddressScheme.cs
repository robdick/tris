namespace Sample.Tris.Lib.Grid
{
    /// <summary>
    ///
    /// </summary>
    public interface IGridAddressScheme
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="addressStr"></param>
        /// <returns></returns>
        bool IsAddressValid(string addressStr);

        /// <summary>
        ///
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="colCount"></param>
        /// <returns></returns>
        GridAddress GetGridAddressForRowColumn(uint row, uint column);

        /// <summary>
        ///
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        GridAddress GetGridAddressForLabel(string label);
    }
}
