using OpenPlusPlusToSharp.Parser.Exceptions;
using System;
using System.Collections.Generic;

namespace OpenPlusPlusToSharp.Parser
{
    public class ParserRunner
    {
        public static void RunAllParsersWhileThereAreTokensLeft(List<IParser> parsers, ParseContext context, ParseNode parentNode)
        {
            while (!context.AllTokensProcessed())
            {
                RunAllParsers(parsers, context, parentNode);
            }
        }

        public static void RunAllParsers(List<IParser> parsers, ParseContext context, ParseNode parentNode)
        {
            foreach (var parser in parsers)
            {
                var result = parser.TryParse(context);

                if (result.Success)
                {
                    parentNode.Descendents.Add(result.ParsedNode);
                    context.CurrentIndex += result.ReadTokens;
                    Console.WriteLine($" - parsed {result.ParsedNode.NodeType}: {result.ParsedNode.Content}");
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

            var token = context.GetFutureToken(0);
            throw new TokenNotRecognizedException(token);
        }
    }
}
