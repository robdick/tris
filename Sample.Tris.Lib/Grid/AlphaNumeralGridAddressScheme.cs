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
            if (string.IsNullOrWhiteSpace(label))
            {
                throw new ArgumentNullException("label");
            }

            return Regex.IsMatch(label, POSITION_REGEX_MATCH);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public GridAddress GetGridAddressForLabel(string label)
        {
            if (string.IsNullOrWhiteSpace(label))
            {
                throw new ArgumentNullException("label");
            }

            if (!IsAddressValid(label))
            {
                throw new InvalidGridReferenceException(label);
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

            uint row = GetIndexForAlphaEncoding(rowMatchGroup.Value);
            uint column = Convert.ToUInt32(colMatchGroup.Value);

            return new GridAddress(row + 1, column, label);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public GridAddress GetGridAddressForRowColumn(uint row, uint column)
        {
            uint count = row;
            uint remainder;
            string rowLabel = string.Empty;

            while (count > 0)
            {
                remainder = (count - 1) % ALPHABET_MAX_CHARS;
                rowLabel = $"{Convert.ToChar(ALPHA_CHARCODE_ORIGIN + remainder)}{rowLabel}";
                count = ((count - remainder) / ALPHABET_MAX_CHARS);
            }

            return new GridAddress(row, column, $"{rowLabel}{column}");
        }

        private uint GetIndexForAlphaEncoding(string label)
        {
            uint index = 0;
            uint pow = 1;

            for (int charIndex = label.Length - 1; charIndex >= 0; charIndex--)
            {
                index += (uint)(label[charIndex] - ALPHA_CHARCODE_ORIGIN + 1) * pow;
                pow *= ALPHABET_MAX_CHARS;
            }

            return index - 1;
        }
    }
}
