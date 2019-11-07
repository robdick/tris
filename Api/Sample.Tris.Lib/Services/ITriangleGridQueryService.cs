namespace Sample.Tris.Lib.Services
{
    using Sample.Tris.Lib.Geometry;

    /// <summary>
    /// Defines an interface for providing grid
    /// </summary>
    public interface ITriangleGridQueryService
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <returns></returns>
        Triangle GetTriangleForPoints(Point p1, Point p2, Point p3);

        /// <summary>
        ///
        /// </summary>
        /// <param name="gridLabel"></param>
        /// <returns></returns>
        Triangle GetTriangleForGridLabel(string gridLabel);
    }
}
