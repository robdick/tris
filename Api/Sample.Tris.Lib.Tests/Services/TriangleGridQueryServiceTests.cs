namespace Sample.Tris.Lib.Tests.Services
{
    using Xunit;
    using Moq;
    using Sample.Tris.Lib.Grid;
    using Sample.Tris.Lib.Services;
    using Sample.Tris.Lib.Geometry;
    using Sample.Tris.Lib.Exceptions;

    public class TriangleGridQueryServiceTests
    {
        private const int CONSTRAINTS_MAX_GRID_ROWS = 6;
        private const int CONSTRAINTS_MAX_TRI_ROWS = CONSTRAINTS_MAX_GRID_ROWS;
        private const int CONSTRAINTS_MAX_GRID_COLS = 6;
        private const int CONSTRAINTS_MAX_TRI_COLS = CONSTRAINTS_MAX_GRID_COLS * 2;
        private const int CONSTRAINTS_CELL_SPAN = 10;

        private const string TEST_LABEL_VALID_MIN = "A1";
        private const string TEST_LABEL_VALID_MAX = "F12";
        private const string TEST_LABEL_INVALID = "1A";

        private GridConstraints _constraints;

        private Mock<IGridAddressScheme> _gridAddressSchemeMock;
        private Mock<IGridConstraintsFactory> _gridConstraintsFactoryMock;
        private TriangleGridQueryService _triangleGridQueryService;

        public TriangleGridQueryServiceTests()
        {
            _constraints = new GridConstraints(
                CONSTRAINTS_MAX_GRID_ROWS,
                CONSTRAINTS_MAX_GRID_COLS,
                CONSTRAINTS_CELL_SPAN
            );

            _gridAddressSchemeMock = new Mock<IGridAddressScheme>();
            _gridAddressSchemeMock.Setup(x => x.IsAddressValid(It.IsAny<string>()))
               .Returns(true)
               .Verifiable();
            _gridAddressSchemeMock.Setup(x => x.GetGridAddressForLabel(It.IsAny<string>()))
                .Returns(new GridAddress(1, 1, "FAKE"))
                .Verifiable();

            _gridConstraintsFactoryMock = new Mock<IGridConstraintsFactory>();
            _gridConstraintsFactoryMock.Setup(x => x.GetConstraints())
                .Returns(_constraints)
                .Verifiable();

            _triangleGridQueryService = new TriangleGridQueryService(_gridConstraintsFactoryMock.Object, _gridAddressSchemeMock.Object);
        }

        [Fact]
        public void Constructor_WhenCalled_GetsConstraints()
        {
            _gridConstraintsFactoryMock.Verify(x => x.GetConstraints(), Times.Once);
        }

        #region GetTriangleForPoints
        [Theory]
        [InlineData(-1, -1, -1, -1, -1, -1)]
        [InlineData(1, 2, 3, 4, 5, 6)]
        [InlineData(11, 11, 11, 11, 11, 11)]
        public void GetTriangleForPoints_WithInvalidGridPoints_ThrowsInvalidTrianglePointsException(int p1x, int p1y, int p2x, int p2y, int p3x, int p3y)
        {
            var ex = Assert.Throws<InvalidGridPointsException>(
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

        [Fact]
        public void GetTriangleForPoints_WithValidPoints_ReturnsTriangleWithFakeAddress()
        {
            _gridAddressSchemeMock.Setup(x => x.GetGridAddressForRowColumn(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new GridAddress(1, 1, "FAKE"))
                .Verifiable();

            var triangle = _triangleGridQueryService.GetTriangleForPoints(new Point(0, 0), new Point(0, 10), new Point(10, 10));

            _gridAddressSchemeMock.Verify(x => x.GetGridAddressForRowColumn(1, 1));

            Assert.Equal("FAKE", triangle.GridAddress.Label);
        }

        #endregion

        #region GetTriangleForGridLabel

        [Fact]
        public void GetTriangleForGridLabel_WithInvalidLabel_ThrowsInvalidGridReferenceException()
        {
            _gridAddressSchemeMock
                .Setup(x => x.IsAddressValid(It.IsAny<string>()))
                .Returns(false)
                .Verifiable();

            Assert.Throws<GridAddressFormatException>(
               () => _triangleGridQueryService.GetTriangleForGridLabel(TEST_LABEL_VALID_MIN));

            _gridAddressSchemeMock.Verify(x => x.IsAddressValid(TEST_LABEL_VALID_MIN), Times.Once);
        }

        [Theory]
        [InlineData(CONSTRAINTS_MAX_TRI_ROWS + 1, CONSTRAINTS_MAX_TRI_COLS)]
        [InlineData(CONSTRAINTS_MAX_TRI_ROWS, CONSTRAINTS_MAX_TRI_COLS + 1)]
        public void GetTriangleForGridLabel_AddressExceedsConstraints_ThrowsInvalidGridReferenceException(int row, int column)
        {
            _gridAddressSchemeMock
                .Setup(x => x.GetGridAddressForLabel(TEST_LABEL_VALID_MIN))
                .Returns(new GridAddress(row, column, TEST_LABEL_VALID_MIN));

            Assert.Throws<GridAddressFormatException>(
               () => _triangleGridQueryService.GetTriangleForGridLabel(TEST_LABEL_VALID_MIN));

            _gridAddressSchemeMock.Verify(x => x.GetGridAddressForLabel(TEST_LABEL_VALID_MIN), Times.Once);
        }

        [Fact]
        public void GetTriangleForGridLabel_ValidatesLabel()
        {
            var triangle = _triangleGridQueryService.GetTriangleForGridLabel(TEST_LABEL_VALID_MIN);

            _gridAddressSchemeMock.Verify(x => x.IsAddressValid(TEST_LABEL_VALID_MIN), Times.Once);
        }

        [Theory]
        [InlineData(1, 1, 0, 0, 10, 10, 0, 10)]
        [InlineData(CONSTRAINTS_MAX_TRI_ROWS / 2, CONSTRAINTS_MAX_TRI_COLS / 2, 20, 20, 30, 30, 30, 20)]
        [InlineData(CONSTRAINTS_MAX_TRI_ROWS, CONSTRAINTS_MAX_TRI_COLS, 50, 50, 60, 60, 60, 50)]
        public void GetTriangleForGridLabel_WithValidLabel_ReturnsTriangle(int row, int column, int p1x, int p1y, int p2x, int p2y, int p3x, int p3y)
        {
            _gridAddressSchemeMock
                .Setup(x => x.GetGridAddressForLabel(TEST_LABEL_VALID_MIN))
                .Returns(new GridAddress(row, column, TEST_LABEL_VALID_MIN));

            var triangle = _triangleGridQueryService.GetTriangleForGridLabel(TEST_LABEL_VALID_MIN);

            Assert.Equal(new Point(p1x, p1y), triangle.P1);
            Assert.Equal(new Point(p2x, p2y), triangle.P2);
            Assert.Equal(new Point(p3x, p3y), triangle.P3);

            Assert.Equal(TEST_LABEL_VALID_MIN, triangle.GridAddress.Label);
            Assert.Equal(row, triangle.GridAddress.Row);
            Assert.Equal(column, triangle.GridAddress.Column);
        }

        #endregion
    }
}
