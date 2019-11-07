namespace Sample.Tris.Lib.Tests.Grid
{
    using System;
    using Xunit;
    using Sample.Tris.Lib.Grid;
    using Sample.Tris.Lib.Exceptions;

    public class GridConstraintsTests
    {
        [Fact]
        public void Constructor_WithRowCountLessThan1_ThrowsException()
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(() => new GridConstraints(0, 1, 1));
            Assert.Equal("rowCount should be greater than or equal to 1. (Parameter 'rowCount')", ex.Message);
            Assert.Equal("rowCount", ex.ParamName);
        }

        [Fact]
        public void Constructor_WithColumnCountLessThan1_ThrowsException()
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(() => new GridConstraints(1, 0, 1));
            Assert.Equal("columnCount should be greater than or equal to 1. (Parameter 'columnCount')", ex.Message);
            Assert.Equal("columnCount", ex.ParamName);
        }

        [Fact]
        public void Constructor_WithCellSpanLessThan1_ThrowsException()
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(() => new GridConstraints(1, 1, 0));
            Assert.Equal("cellSpan should be greater than or equal to 1. (Parameter 'cellSpan')", ex.Message);
            Assert.Equal("cellSpan", ex.ParamName);
        }

        [Theory]
        [InlineData(1, 1, 1, 1, 1)]
        [InlineData(1, 2, 3, 6, 3)]
        public void Constructor_WithValidParameters_SetsValidWidthAndHeightProps(int rowCount, int columnCount, int cellSpan, int expectedWidth, int expectedHeight)
        {
            var constraints = new GridConstraints(rowCount, columnCount, cellSpan);
            Assert.Equal(expectedWidth, constraints.Width);
            Assert.Equal(expectedHeight, constraints.Height);
        }
    }
}
