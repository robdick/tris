namespace Sample.Tris.Lib.Geometry
{
    using System;
    using Sample.Tris.Lib.Grid;

    /// <summary>
    /// Defines an immutable triangle
    /// </summary>
    public class Triangle
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        public Triangle(Point p1, Point p2, Point p3, GridAddress gridAddress)
            => (P1, P2, P3, GridAddress) = (p1, p2, p3, gridAddress);

        /// <summary>
        ///
        /// </summary>
        /// <value></value>
        public GridAddress GridAddress { get; }

        /// <summary>
        ///
        /// </summary>
        /// <value></value>
        public Point P1 { get; }

        /// <summary>
        ///
        /// </summary>
        /// <value></value>
        public Point P2 { get; }

        /// <summary>
        ///
        /// </summary>
        /// <value></value>
        public Point P3 { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
            => obj is Triangle triangle
                && triangle.P1.Equals(P1)
                && triangle.P2.Equals(P2)
                && triangle.P3.Equals(P3);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
            => HashCode.Combine(P1.GetHashCode(), P2.GetHashCode(), P3.GetHashCode());

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => $"Triangle(Point({P1.X},{P1.Y}), Point({P2.X},{P2.Y}), Point({P3.X},{P3.Y}))";
    }
}
