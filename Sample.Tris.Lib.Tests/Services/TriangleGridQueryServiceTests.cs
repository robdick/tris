namespace Sample.Tris.Lib.Tests.Services
{
    using Xunit;
    using Moq;
    using Sample.Tris.Lib.Grid;
    using Sample.Tris.Lib.Services;
    using Sample.Tris.Lib.Geometry;
    using System;
    using Sample.Tris.Lib.Exceptions;

    public class TriangleGridQueryServiceTests
    {
        private Mock<IGridAddressScheme> _gridAddressSchemeMock;
        private Mock<IGridConstraintsFactory> _gridConstraintsFactoryMock;
        private GridConstraints _constraints = new GridConstraints(2, 2, 10);
        private TriangleGridQueryService _triangleGridQueryService;

        public TriangleGridQueryServiceTests()
        {
            _gridAddressSchemeMock = new Mock<IGridAddressScheme>();
            _gridConstraintsFactoryMock = new Mock<IGridConstraintsFactory>();
            _gridConstraintsFactoryMock.Setup(x => x.GetConstraints())
                .Returns(_constraints)
                .Verifiable();
            _triangleGridQueryService = new TriangleGridQueryService(_gridConstraintsFactoryMock.Object, _gridAddressSchemeMock.Object);
        }

        [Fact]
        public void Constructor_WhenCalled_GetsConstraints()
        {
            _gridConstraintsFactoryMock.Verify(x => x.GetConstraints(), Times.AtMostOnce);
        }

        [Theory]
        [InlineData(-1, -1, -1, -1, -1, -1)]
        [InlineData(1, 2, 3, 4, 5, 6)]
        [InlineData(11, 11, 11, 11, 11, 11)]
        public void GetTriangleForPoints_WithInvalidGridPoints_ThrowsInvalidTrianglePointsException(int p1x, int p1y, int p2x, int p2y, int p3x, int p3y)
        {
            InvalidGridPointsException ex = Assert.Throws<InvalidGridPointsException>(
                () => _triangleGridQueryService.GetTriangleForPoints(new Point(p1x, p1y), new Point(p2x, p2y), new Point(p3x, p3y)));

            Assert.Equal(
                string.Format("Invalid set of points given Point({0},{1}),Point({2},{3}),Point({4},{5})", p1x, p1y, p2x, p2y, p3x, p3y),
                ex.Message
            );
        }

        [Theory]
        [InlineData(0, 0, 0, 10, 10, 10)]
        [InlineData(0, 0, 10, 0, 10, 10)]
        [InlineData(10, 0, 10, 10, 20, 10)]
        [InlineData(10, 0, 20, 0, 20, 10)]
        [InlineData(0, 10, 0, 20, 10, 20)]
        [InlineData(0, 10, 10, 10, 10, 20)]
        [InlineData(10, 10, 10, 20, 20, 20)]
        [InlineData(10, 10, 20, 10, 20, 20)]
        public void GetTriangleForPoints_WithValidPoints_ReturnsTriangle(int p1x, int p1y, int p2x, int p2y, int p3x, int p3y)
        {
            var triangle = _triangleGridQueryService.GetTriangleForPoints(new Point(p1x, p1y), new Point(p2x, p2y), new Point(p3x, p3y));
            Assert.NotNull(triangle);
        }


    }
}
