using System;

namespace OpenPlusPlusToSharp.Parser
{
    /// <summary>
    /// The <see cref="ParseResult"/> contains the information what a parser has done after processing a <see cref="ParseContext"/>.
    /// </summary>
    public class ParseResult
    {
        /// <summary>
        /// Determines whether the parser could parse the one or more tokens from the <see cref="ParseContext"/>.
        /// </summary>
        public bool Success { get; }

        /// <summary>
        /// If the parser was successfull then the ParsedNode contains the parsed information.
        /// </summary>
        public ParseNode ParsedNode { get; }

        /// <summary>
        /// If the parser was successfull then this property contains the number of the tokens that have been read.
        /// These tokens won't be processed by a future parser.
        /// </summary>
        public int ReadTokens { get; }

        private ParseResult(bool success, ParseNode parsedNode, int readTokens)
        {
            Success = success;
            ParsedNode = parsedNode;
            ReadTokens = readTokens;
        }

        /// <summary>
        /// Creates a successfull parse result.
        /// </summary>
        /// <param name="parsedNode">The node that has been parsed by the parser.</param>
        /// <param name="readTokens">The number of tokens that have been read by the parser to create the parse node.</param>
        /// <returns>A <see cref="ParseResult"/> containing all the provided information.</returns>
        public static ParseResult ParseSuccess(ParseNode parsedNode, int readTokens)
        {
            if (parsedNode == null)
            {
                throw new ArgumentException("The parse process was marked as success but the parsed node is null.");
            }

            if (readTokens < 1)
            {
                throw new ArgumentException("The parse process was markes as success but the number of read tokens is not specified.");
            }

            return new ParseResult(true, parsedNode, readTokens);
        }

        /// <summary>
        /// Creates a unsuccessfull result.
        /// </summary>
        /// <returns>A <see cref="ParseResult"/> without a success flag, without a parse node and with no read tokens.</returns>
        public static ParseResult CouldNotParse()
        {
            return new ParseResult(false, null, 0);
        }
    }
}