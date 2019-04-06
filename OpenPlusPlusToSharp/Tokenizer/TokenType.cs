namespace OpenPlusPlusToSharp.Tokenizer
{
    /// <summary>
    /// The <see cref="TokenType"/> provides meta information about a read token.
    /// </summary>
    public enum TokenType
    {
        /// <summary>
        /// Any token that is not a special charachter or a string literal is of type text.
        /// </summary>
        Text,

        /// <summary>
        /// Special chars are chars that should be extracted into their own token.
        /// </summary>
        SpecialCharacter,

        /// <summary>
        /// String literals are well string literals.
        /// </summary>
        StringLiteral
    }
}
