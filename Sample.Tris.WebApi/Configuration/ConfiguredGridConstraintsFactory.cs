namespace Sample.Tris.WebApi.Providers
{
    using Sample.Tris.Lib.Grid;
    using Sample.Tris.WebApi.Configuration;

    /// <summary>
    /// Implements a <see cref="Sample.Tris.Lib.Grid.IGridContraintsProvider">IGridContraintsProvider</see> to provide
    /// a configured <see cref="Sample.Tris.Lib.Grid.GridConstraints">TriangleGridConstraints</see> isntance.
    /// </summary>
    public class ConfiguredGridConstraintsFactory : IGridConstraintsFactory
    {
        private readonly TrisApiGridSettings _triangleGridSettings;

        /// <summary>
        ///
        /// </summary>
        /// <param name="trisApiGridSettings"></param>
        public ConfiguredGridConstraintsFactory(TrisApiGridSettings trisApiGridSettings)
            => (_triangleGridSettings) = (trisApiGridSettings);

        /// <summary>
        ///
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="colCount"></param>
        /// <param name="cellSpan"></param>
        /// <returns></returns>
        public GridConstraints GetConstraints()
        {
            return new GridConstraints(_triangleGridSettings.MaxRows, _triangleGridSettings.MaxColumns, _triangleGridSettings.CellSizeInPixels);
        }
    }
}
