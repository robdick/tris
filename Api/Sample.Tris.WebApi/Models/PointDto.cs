namespace Sample.Tris.WebApi.Models
{
    using System.ComponentModel;
    using Sample.Tris.WebApi.TypeConverters;

    /// <summary>
    /// Defines an dto representing a 2d point.
    /// </summary>
    [TypeConverter(typeof(PointDtoTypeConverter))]
    public class PointDto
    {
        public PointDto(int x, int y)
            => (X, Y) = (x, y);

        public int X { get; set; }
        public int Y { get; set; }
    }
}
