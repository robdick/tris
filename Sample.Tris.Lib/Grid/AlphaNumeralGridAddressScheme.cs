namespace Sample.Tris.Lib.Grid
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Sample.Tris.Lib.Exceptions;

    /// <summary>
    ///
    /// </summary>
    public class AlphaNumeralGridAddressScheme : IGridAddressScheme
    {
        private const int ALPHABET_MAX_CHARS = 26;
        private const char ALPHA_CHARCODE_ORIGIN = 'A';
        private const string POSITION_REGEX_MATCH = "^([A-Z]{1,7})([1-9][0-9]*)$";

        /// <summary>
        ///
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public bool IsAddressValid(string label)
        {
            return !string.IsNullOrWhiteSpace(label)
                && Regex.IsMatch(label, POSITION_REGEX_MATCH)
                && GetIndexForAlphaEncoding(label) <= int.MaxValue;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public GridAddress GetGridAddressForLabel(string label)
        {
            if (!IsAddressValid(label))
            {
                throw new GridAddressFormatException(label);
            }

            Match match = Regex
                .Matches(
                    label,
                    POSITION_REGEX_MATCH,
                    RegexOptions.CultureInvariant
                )
                .Cast<Match>()
                .Single();

            Group rowMatchGroup = match.Groups[1];
            Group colMatchGroup = match.Groups[2];

            int row = GetIndexForAlphaEncoding(rowMatchGroup.Value) + 1;
            int column = Convert.ToInt32(colMatchGroup.Value);

            if (!IsRowAndColumnInBounds(row, column))
            {
                throw new GridAddressOutOfBoundsException(label);
            }

            return new GridAddress(row, column, label);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public GridAddress GetGridAddressForRowColumn(int row, int column)
        {
            if (!IsRowAndColumnInBounds(row, column))
            {
                throw new GridCoordsOutOfBoundsException(row, column);
            }

            string rowLabel = CreateRowLabel(row);

            return new GridAddress(row, column, $"{rowLabel}{column}");
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public (int UpperRow, int UpperColumn, string UpperAddress) GetExtents()
        {
            return (int.MaxValue, int.MaxValue, $"{CreateRowLabel(int.MaxValue)}{int.MaxValue}");
        }

        private string CreateRowLabel(int row)
        {
            int count = row;
            int remainder;
            string rowLabel = string.Empty;

            while (count > 0)
            {
                remainder = (count - 1) % ALPHABET_MAX_CHARS;
                rowLabel = $"{Convert.ToChar(ALPHA_CHARCODE_ORIGIN + remainder)}{rowLabel}";
                count = ((count - remainder) / ALPHABET_MAX_CHARS);
            }

            return rowLabel;
        }

        private int GetIndexForAlphaEncoding(string label)
        {
            int index = 0;
            int pow = 1;

            for (int charIndex = label.Length - 1; charIndex >= 0; charIndex--)
            {
                index += (int)(label[charIndex] - ALPHA_CHARCODE_ORIGIN) * pow;
                pow *= ALPHABET_MAX_CHARS;
            }

            return index;
        }

        private bool IsRowAndColumnInBounds(int row, int column)
        {
            return (row <= int.MaxValue
                && row > 0
                && column <= int.MaxValue
                && column > 0);
        }
    }
}
