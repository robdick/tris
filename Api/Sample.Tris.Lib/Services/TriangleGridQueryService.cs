namespace Sample.Tris.Lib.Services
{
    using System.Linq;
    using Sample.Tris.Lib.Exceptions;
    using Sample.Tris.Lib.Geometry;
    using Sample.Tris.Lib.Grid;

    /// <summary>
    ///
    /// </summary>
    public class TriangleGridQueryService : ITriangleGridQueryService
    {
        private readonly GridConstraints _gridConstraints;
        private readonly IGridAddressScheme _gridAddressScheme;

        public TriangleGridQueryService(
            IGridConstraintsFactory gridConstraintsFactory,
            IGridAddressScheme gridAddressScheme)
        {
            _gridConstraints = gridConstraintsFactory.GetConstraints();
            _gridAddressScheme = gridAddressScheme;
        }

        #region public

        /// <summary>
        ///
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <returns></returns>
        public Triangle GetTriangleForPoints(Point p1, Point p2, Point p3)
        {
            if (!IsValidPoint(p1) || !IsValidPoint(p2) || !IsValidPoint(p3))
            {
                throw new InvalidGridPointsException($"Invalid set of points given {p1.ToString()},{p2.ToString()},{p3.ToString()}");
            }

            int row, col;
            Point topLeftPoint, bottomRightPoint, topRightPoint, bottomLeftPoint;
            var pointsArr = new Point[] { p1, p2, p3 };

            topLeftPoint = GetTopLeftPoint(pointsArr);
            if (topLeftPoint == null)
            {
                return null;
            }

            bottomRightPoint = FindPoint(topLeftPoint.X + (int)_gridConstraints.CellSpan, topLeftPoint.Y + (int)_gridConstraints.CellSpan, pointsArr);
            if (bottomRightPoint == null)
            {
                return null;
            }

            row = topLeftPoint.Y / _gridConstraints.CellSpan + 1;
            col = (topLeftPoint.X / _gridConstraints.CellSpan * 2) + 1;

            bottomLeftPoint = FindPoint(topLeftPoint.X, topLeftPoint.Y + (int)_gridConstraints.CellSpan, pointsArr);
            if (bottomLeftPoint != null)
            {
                return new Triangle(p1, p2, p3, _gridAddressScheme.GetGridAddressForRowColumn(row, col));
            }

            topRightPoint = FindPoint(topLeftPoint.X + (int)_gridConstraints.CellSpan, topLeftPoint.Y, pointsArr);
            if (topRightPoint != null)
            {
                return new Triangle(p1, p2, p3, _gridAddressScheme.GetGridAddressForRowColumn(row, col + 1));
            }

            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="gridLabel"></param>
        /// <returns></returns>
        public Triangle GetTriangleForGridLabel(string gridLabel)
        {
            if (!_gridAddressScheme.IsAddressValid(gridLabel))
            {
                throw new GridAddressFormatException(gridLabel);
            }

            var address = _gridAddressScheme.GetGridAddressForLabel(gridLabel);

            if (address.Row > _gridConstraints.RowCount
                || address.Column > _gridConstraints.ColumnCount * 2)
            {
                throw new GridAddressOutOfBoundsException(gridLabel);
            }

            var isLeftTri = address.Column % 2 == 1;
            var column = (address.Column - 1) / 2;
            var row = address.Row - 1;

            var topLeftPoint = new Point(
                column * _gridConstraints.CellSpan,
                row * _gridConstraints.CellSpan
            );

            var bottomRightPoint = new Point(
                (column + 1) * _gridConstraints.CellSpan,
                (row + 1) * _gridConstraints.CellSpan
            );

            var adjacentPoint = new Point(
                isLeftTri ? column * _gridConstraints.CellSpan : (column + 1) * _gridConstraints.CellSpan,
                isLeftTri ? (row + 1) * _gridConstraints.CellSpan : row * _gridConstraints.CellSpan
            );

            return new Triangle(topLeftPoint, bottomRightPoint, adjacentPoint, address);
        }

        #endregion

        #region private

        /// <summary>
        ///
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        private bool IsValidPoint(Point point)
        {
            return point != null
                && point.X % _gridConstraints.CellSpan == 0
                && point.Y % _gridConstraints.CellSpan == 0
                && point.X >= 0
                && point.X <= _gridConstraints.Width
                && point.Y >= 0
                && point.Y <= _gridConstraints.Height;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        private Point GetTopLeftPoint(Point[] points)
        {
            return points.OrderBy(p => p.X + p.Y * _gridConstraints.Width).FirstOrDefault();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="points"></param>
        /// <returns></returns>
        private Point FindPoint(int x, int y, Point[] points)
        {
            return points.Where(point =>
                   point.X == x
                   && point.Y == y).FirstOrDefault();
        }

        #endregion
    }
}
