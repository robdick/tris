namespace Sample.Tris.Lib.Exceptions
{
    using System;

    /// <summary>
    /// Base class for library exceptions
    /// </summary>
    public abstract class TrisLibException : Exception
    {
        public TrisLibException(string message) : base(message) { }
    }
}
