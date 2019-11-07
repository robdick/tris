namespace Sample.Tris.Lib.Exceptions
{
    /// <summary>
    /// Exception type representing an invalid grid address
    /// </summary>
    public class GridAddressFormatException : TrisLibValidationException
    {
        public GridAddressFormatException(string address) : base($"Grid address '{address}' has an invalid format.") { }
    }
}
