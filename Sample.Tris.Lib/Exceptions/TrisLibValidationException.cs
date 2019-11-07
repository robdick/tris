namespace Sample.Tris.Lib.Exceptions
{
    using System;

    /// <summary>
    /// Defines Exception for library validation errors
    /// </summary>
    public class TrisLibValidationException : TrisLibException
    {
        public TrisLibValidationException(string message) : base(message) { }
    }
}
