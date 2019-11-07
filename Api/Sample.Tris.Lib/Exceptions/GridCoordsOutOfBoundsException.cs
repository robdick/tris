namespace Sample.Tris.Lib.Exceptions
{
    /// <summary>
    /// Exception type representing grid coords out of bounds
    /// </summary>
    public class GridCoordsOutOfBoundsException : TrisLibValidationException
    {
        public GridCoordsOutOfBoundsException(int row, int column) : base($"Grid cords '{row},{column}' are out of bounds.") { }
    }
}
