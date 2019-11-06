namespace Sample.Tris.Lib.Exceptions
{
    /// <summary>
    /// Exception type representing an invalid grid reference
    /// </summary>
    public class InvalidGridReferenceException : TrisLibException
    {
        public InvalidGridReferenceException(string gridReference) : base($"Grid reference '{gridReference}' is invalid.") { }
    }
}
