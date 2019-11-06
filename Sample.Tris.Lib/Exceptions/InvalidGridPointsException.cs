namespace Sample.Tris.Lib.Exceptions
{
    /// <summary>
    /// Exception type representing an invalid composite key format
    /// </summary>
    public class InvalidGridPointsException : TrisLibValidationException
    {
        public InvalidGridPointsException(string message) : base(message) { }
    }
}
