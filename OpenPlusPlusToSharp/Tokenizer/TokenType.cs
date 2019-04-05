namespace OpenPlusPlusToSharp.Tokenizer
{
    /// <summary>
    /// The <see cref="TokenType"/> provides meta information about a read token.
    /// </summary>
    public enum TokenType
    {
        Text,
        BeginBlock,
        EndBlock,
        OpeningBracket,
        ClosingBracket,
        Semicolon,
        Comma
    }
}
