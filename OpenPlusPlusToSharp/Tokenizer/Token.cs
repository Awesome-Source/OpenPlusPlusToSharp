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
        /// Creates a token with the provided content and type.
        /// </summary>
        /// <param name="content">The raw content of the token.</param>
        /// <param name="tokenType">The type of the token.</param>
        public Token(string content, TokenType tokenType)
        {
            Content = content;
            TokenType = tokenType;
        }
    }
}
