namespace OpenPlusPlusToSharp.Tokenizer
{
    /// <summary>
    /// Contains information about the current context during tokenization.
    /// </summary>
    public class ReadContext
    {
        /// <summary>
        /// Determines whether the last read character was an escape symbol.
        /// </summary>
        public bool IsEscaped { get; set; }

        /// <summary>
        /// Determines whether the tokenization process is currently reading a string literal.
        /// </summary>
        public bool IsInsideStringLiteral { get; set; }
    }
}
