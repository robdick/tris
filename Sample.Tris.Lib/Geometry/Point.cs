namespace Sample.Tris.Lib.Geometry
{
    using System;

    /// <summary>
    /// Defines a 2d point
    /// </summary>
    public class Point
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Point(int x = 0, int y = 0)
            => (X, Y) = (x, y);

        /// <summary>
        ///
        /// </summary>
        /// <value></value>
        public int X { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <value></value>
        public int Y { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static Point operator /(Point point, int divisor)
            => new Point(point.X / divisor, point.Y / divisor);

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
            => obj is Point point && point.X == X && point.Y == Y;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
            => HashCode.Combine(X, Y);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => $"Point({X},{Y})";
    }
}
