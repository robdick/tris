namespace Sample.Tris.Lib.Tests.Grid
{
    using System;
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
            ArgumentException ex = Assert.Throws<ArgumentException>(() => _gridAddressScheme.GetGridAddressForRowColumn(0, 1));
            Assert.Equal("row", ex.ParamName);
            Assert.Equal("row must be 1 or greater (Parameter 'row')", ex.Message);
        }

        [Fact]
        public void GetGridAddressForRowColumn_WithColumnLessThan1_ThrowsException()
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(() => _gridAddressScheme.GetGridAddressForRowColumn(1, 0));
            Assert.Equal("column", ex.ParamName);
            Assert.Equal("column must be 1 or greater (Parameter 'column')", ex.Message);
        }

        [Theory]
        [InlineData(1, 1, "A1")]
        [InlineData(26, 1, "Z1")]
        [InlineData(27, 1, "AA1")]
        [InlineData(uint.MaxValue, 1, "MWLQKWU1")]
        public void GetGridAddressForRowColumn_WithValidArguments_ReturnsValidGridAddress(uint row, uint column, string expectedAddress)
        {
            var address = _gridAddressScheme.GetGridAddressForRowColumn(row, column);
            Assert.Equal(row, address.Row);
            Assert.Equal(column, address.Column);
        }

        #endregion

        #region IsAddressValid

        [Fact]
        public void IsAddressValid_WithNonAlphaStartingChar_ReturnsFalse()
        {
            Assert.False(_gridAddressScheme.IsAddressValid("1A"));
        }

        [Fact]
        public void IsAddressValid_WithNullArg_ThrowsArgumentNullException()
        {
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => _gridAddressScheme.IsAddressValid(null));
            Assert.Equal("label", ex.ParamName);
        }

        [Fact]
        public void IsAddressValid_WithEmptyArg_ThrowsArgumentNullException()
        {
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => _gridAddressScheme.IsAddressValid(""));
            Assert.Equal("label", ex.ParamName);
        }

        [Theory]
        [InlineData("A1")]
        [InlineData("MWLQKWU1")]
        public void IsAddressValid_WithValidArguments_ReturnsTrue(string address)
        {
            Assert.True(_gridAddressScheme.IsAddressValid(address));
        }

        #endregion

        #region GetGridAddressForLabel

        [Fact]
        public void GetGridAddressForLabel_WithNullArg_ThrowsArgumentNullException()
        {
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => _gridAddressScheme.GetGridAddressForLabel(null));
            Assert.Equal("label", ex.ParamName);
        }

        [Fact]
        public void GetGridAddressForLabel_WithEmptyArg_ThrowsArgumentNullException()
        {
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => _gridAddressScheme.GetGridAddressForLabel(""));
            Assert.Equal("label", ex.ParamName);
        }

        [Theory]
        [InlineData("1Z")]
        [InlineData("0")]
        [InlineData("1")]
        public void GetGridAddressForLabel_WithInvalidLabel_ThrowsInvalidGridReferenceException(string label)
        {
            InvalidGridReferenceException ex = Assert.Throws<InvalidGridReferenceException>(() => _gridAddressScheme.GetGridAddressForLabel(label));
            Assert.Equal(string.Format("Grid reference '{0}' is invalid.", label), ex.Message);
        }

        [Theory]
        [InlineData("B1", 2, 1)]
        [InlineData("Z1", 26, 1)]
        [InlineData("AA1", 27, 1)]
        [InlineData("MWLQKWU1", uint.MaxValue, 1)]
        public void GetGridAddressForLabel_ReturnsAddress(string label, uint expectedRow, uint expectedColumn)
        {
            var address = _gridAddressScheme.GetGridAddressForLabel(label);
            Assert.Equal(expectedRow, address.Row);
            Assert.Equal(expectedColumn, address.Column);
            Assert.Equal(label, address.Label);
        }

        #endregion
    }
}
