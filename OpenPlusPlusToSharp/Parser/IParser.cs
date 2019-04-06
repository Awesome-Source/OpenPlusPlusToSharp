namespace OpenPlusPlusToSharp.Parser
{
    /// <summary>
    /// A <see cref="IParser"/> reads one or more tokens from the <see cref="ParseContext"/>. Each <see cref="IParser"/> looks for a specific token structure.
    /// The parser returns a <see cref="ParseResult"/> which contains information whether it was able to extrace the specific structure or not.
    /// </summary>
    public interface IParser
    {
        /// <summary>
        /// Tries to parse one or more tokens from the context. The result of the parse process is returned.
        /// </summary>
        /// <param name="context">The current parse context.</param>
        /// <returns>Whether it has succeeded or failed. If it has succeeded then the parsed node is also returned.</returns>
        ParseResult TryParse(ParseContext context);
    }
}
