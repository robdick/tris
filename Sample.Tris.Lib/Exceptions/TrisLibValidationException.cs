namespace Sample.Tris.Lib.Exceptions
{
    using System;

    /// <summary>
    /// Defines Exception for library validation errors
    /// </summary>
    public abstract class TrisLibValidationException : TrisLibException
    {
        public TrisLibValidationException(string message) : base(message) { }
    }
}
