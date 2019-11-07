namespace Sample.Tris.WebApi.Configuration
{
    public class TrisApiGridSettings
    {
        public const string SECTION_NAME = "TrisApiGridSettings";

        public const int DEFAULT_MAX_COLUMNS = 12;
        public const int DEFAULT_MAX_ROWS = 6;
        public const int DEFAULT_CELL_SZ = 10;

        public int MaxColumns { get; set; } = DEFAULT_MAX_COLUMNS;
        public int MaxRows { get; set; } = DEFAULT_MAX_ROWS;
        public int CellSizeInPixels { get; set; } = DEFAULT_CELL_SZ;
    }
}
