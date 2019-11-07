namespace Sample.Tris.WebApi.Models
{
    using System.ComponentModel;
    using Sample.Tris.WebApi.TypeConverters;

    /// <summary>
    /// Defines an dto representing a triangle.
    /// </summary>
    public class TriangleDto
    {
        public string GridLabel { get; set; }
        public int GridRow { get; set; }
        public int GridColumn { get; set; }
        public PointDto P1 { get; set; }
        public PointDto P2 { get; set; }
        public PointDto P3 { get; set; }
    }
}
