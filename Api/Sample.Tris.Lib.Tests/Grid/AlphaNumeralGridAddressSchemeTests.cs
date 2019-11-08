namespace Sample.Tris.Lib.Tests.Grid
{
    using Xunit;
    using Sample.Tris.Lib.Grid;
    using Sample.Tris.Lib.Exceptions;

    public class AlphaNumeralGridAddressSchemeTests
    {
        private IGridAddressScheme _gridAddressScheme;

        public AlphaNumeralGridAddressSchemeTests()
        {
            _gridAddressScheme = new AlphaNumeralGridAddressScheme();
        }

        #region GetGridAddressForRowColumn

        [Fact]
        public void GetGridAddressForRowColumn_WithRowLessThan1_ThrowsException()
        {
            GridCoordsOutOfBoundsException ex = Assert.Throws<GridCoordsOutOfBoundsException>(() => _gridAddressScheme.GetGridAddressForRowColumn(0, 1));
        }

        [Fact]
        public void GetGridAddressForRowColumn_WithColumnLessThan1_ThrowsException()
        {
            var ex = Assert.Throws<GridCoordsOutOfBoundsException>(() => _gridAddressScheme.GetGridAddressForRowColumn(1, 0));
        }

        [Theory]
        [InlineData(1, 1, "A1")]
        [InlineData(26, 1, "Z1")]
        [InlineData(27, 1, "AA1")]
        [InlineData(int.MaxValue, 1, "FXSHRXW1")]
        public void GetGridAddressForRowColumn_WithValidArguments_ReturnsValidGridAddress(int row, int column, string expectedAddress)
        {
            var address = _gridAddressScheme.GetGridAddressForRowColumn(row, column);

            Assert.Equal(row, address.Row);
            Assert.Equal(column, address.Column);
            Assert.Equal(expectedAddress, address.Label);

        }

        #endregion

        #region IsAddressValid

        [Fact]
        public void IsAddressValid_WithNonAlphaStartingChar_ReturnsFalse()
        {
            Assert.False(_gridAddressScheme.IsAddressValid("1A"));
        }

        [Fact]
        public void IsAddressValid_WithNoAlphaStartingChar_ReturnsFalse()
        {
            Assert.False(_gridAddressScheme.IsAddressValid("1"));
        }

        [Fact]
        public void IsAddressValid_WithNullArg_ReturnsFalse()
        {
            Assert.False(_gridAddressScheme.IsAddressValid(null));
        }

        [Fact]
        public void IsAddressValid_WithEmptyArg_ReturnsFalse()
        {
            Assert.False(_gridAddressScheme.IsAddressValid(""));
        }

        [Theory]
        [InlineData("A1")]
        [InlineData("F12")]
        public void IsAddressValid_WithValidArguments_ReturnsTrue(string address)
        {
            Assert.True(_gridAddressScheme.IsAddressValid(address));
        }

        #endregion

        #region GetGridAddressForLabel

        [Fact]
        public void GetGridAddressForLabel_WithNullArg_ThrowsGridAddressFormatException()
        {
            GridAddressFormatException ex = Assert.Throws<GridAddressFormatException>(() => _gridAddressScheme.GetGridAddressForLabel(null));
            Assert.Equal("Grid address '' has an invalid format.", ex.Message);
        }

        [Fact]
        public void GetGridAddressForLabel_WithEmptyArg_ThrowsGridAddressFormatException()
        {
            GridAddressFormatException ex = Assert.Throws<GridAddressFormatException>(() => _gridAddressScheme.GetGridAddressForLabel(""));
            Assert.Equal("Grid address '' has an invalid format.", ex.Message);
        }

        [Theory]
        [InlineData("1Z")]
        [InlineData("0")]
        [InlineData("1")]
        public void GetGridAddressForLabel_WithInvalidLabel_ThrowsInvalidGridReferenceException(string label)
        {
            GridAddressFormatException ex = Assert.Throws<GridAddressFormatException>(() => _gridAddressScheme.GetGridAddressForLabel(label));
            Assert.Equal(string.Format("Grid address '{0}' has an invalid format.", label), ex.Message);
        }

        [Theory]
        [InlineData("A1", 1, 1)]
        [InlineData("F12", 6, 12)]
        public void GetGridAddressForLabel_ReturnsAddress(string label, int expectedRow, int expectedColumn)
        {
            var address = _gridAddressScheme.GetGridAddressForLabel(label);
            Assert.Equal(expectedRow, address.Row);
            Assert.Equal(expectedColumn, address.Column);
            Assert.Equal(label, address.Label);
        }

        #endregion

        #region GetExtents

        [Fact]
        public void GetExtents_ReturnsMaxIntValue()
        {
            var extents = _gridAddressScheme.GetExtents();

            Assert.Equal(int.MaxValue, extents.UpperRow);
            Assert.Equal(int.MaxValue, extents.UpperColumn);

            var address = _gridAddressScheme.GetGridAddressForRowColumn(int.MaxValue, int.MaxValue);
            Assert.Equal(address.Label, extents.UpperAddress);
        }

        #endregion
    }
}
