using OpenPlusPlusToSharp.Tokenizer;

namespace OpenPlusPlusToSharp.Parser
{
    public static class ParserHelper
    {
        public static int FindClosingCurlyBracket(ParseContext context, int tokenOffset)
        {
            return FindClosingToken(context, tokenOffset, "{", "}");
        }

        public static int FindClosingBracket(ParseContext context, int tokenOffset)
        {
            return FindClosingToken(context, tokenOffset, "(", ")");
        }

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
