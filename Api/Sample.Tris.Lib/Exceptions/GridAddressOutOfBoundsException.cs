namespace Sample.Tris.Lib.Exceptions
{
    /// <summary>
    /// Exception type representing a grid reference out of bounds
    /// </summary>
    public class GridAddressOutOfBoundsException : TrisLibValidationException
    {
        public GridAddressOutOfBoundsException(string address) : base($"Grid address '{address}' is out of bounds.") { }
    }
}
