namespace OpenPlusPlusToSharp.Tokenizer
{
    /// <summary>
    /// A token represents the smallest valid syntactic element.
    /// </summary>
    public class Token
    {
        /// <summary>
        /// The raw content of the token.
        /// </summary>
        public string Content { get;}

        /// <summary>
        /// The type of the token.
        /// </summary>
        public TokenType TokenType { get; }

        /// <summary>
        /// The line number from the source file the token was read from.
        /// </summary>
        public int LineNumber { get; }

        /// <summary>
        /// The column index from the line of the source file the token was read from.
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        /// Creates a token with the provided content and type.
        /// </summary>
        /// <param name="content">The raw content of the token.</param>
        /// <param name="tokenType">The type of the token.</param>
        /// <param name="lineNumber">The line number of the source file</param>
        /// <param name="column">The column of the source file</param>
        public Token(string content, TokenType tokenType, int lineNumber, int column)
        {
            Content = content;
            TokenType = tokenType;
            LineNumber = lineNumber;
            Column = column;
        }

        /// <summary>
        /// Returns a string representation of the token.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Content: [{Content}], Type: {TokenType} ({LineNumber},{Column})";
        }
    }
}
