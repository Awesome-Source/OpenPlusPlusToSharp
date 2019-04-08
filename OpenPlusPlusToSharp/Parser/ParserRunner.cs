using OpenPlusPlusToSharp.Parser.Exceptions;
using System.Collections.Generic;

namespace OpenPlusPlusToSharp.Parser
{
    public class ParserRunner
    {
        public static void RunAllParsers(List<IParser> parsers, ParseContext context, ParseNode parentNode)
        {
            foreach (var parser in parsers)
            {
                var result = parser.TryParse(context);

                if (result.Success)
                {
                    parentNode.Descendents.Add(result.ParsedNode);
                    context.CurrentIndex += result.ReadTokens;
                    return;
                }
            }

            ThrowTokenNotRecognizedExceptionIfAllTokensAreProcessed(context);
        }

        private static void ThrowTokenNotRecognizedExceptionIfAllTokensAreProcessed(ParseContext context)
        {
            if (context.AllTokensProcessed())
            {
                return;
            }

            var token = context.GetCurrentToken();
            throw new TokenNotRecognizedException(token);
        }
    }
}
