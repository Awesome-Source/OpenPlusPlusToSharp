using OpenPlusPlusToSharp.Tokenizer;

namespace OpenPlusPlusToSharp.Parser
{
    /// <summary>
    /// Helper class for common parsing methods.
    /// </summary>
    public static class ParserHelper
    {
        /// <summary>
        /// Finds the offset of the closing curly brace starting at the provided offset. Returns -1 if no closing brace is found.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tokenOffset"></param>
        /// <returns></returns>
        public static int FindClosingCurlyBracket(ParseContext context, int tokenOffset)
        {
            return FindClosingToken(context, tokenOffset, "{", "}");
        }

        /// <summary>
        /// Finds the offset of the closing brace starting at the provided offset. Returns -1 if no closing brace is found.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tokenOffset"></param>
        /// <returns></returns>
        public static int FindClosingBracket(ParseContext context, int tokenOffset)
        {
            return FindClosingToken(context, tokenOffset, "(", ")");
        }

        /// <summary>
        /// Finds the offset of the closing angle brace starting at the provided offset. Returns -1 if no closing brace is found.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tokenOffset"></param>
        /// <returns></returns>
        public static int FindClosingAngleBracket(ParseContext context, int tokenOffset)
        {
            return FindClosingToken(context, tokenOffset, "<", ">");
        }

        private static int FindClosingToken(ParseContext context, int tokenOffset, string openingToken, string closingToken)
        {
            var foundClosingBracket = false;
            var openBracketCount = 1;

            while (!foundClosingBracket)
            {
                var nextToken = context.GetFutureToken(++tokenOffset);

                if (nextToken == null)
                {
                    return -1;
                }

                if (nextToken.TokenType != TokenType.SpecialCharacter)
                {
                    continue;
                }

                if (nextToken.Content == openingToken)
                {
                    openBracketCount++;
                }

                if (nextToken.Content == closingToken)
                {
                    openBracketCount--;
                }

                if (openBracketCount == 0)
                {
                    foundClosingBracket = true;
                }
            }

            return tokenOffset;
        }
    }
}
