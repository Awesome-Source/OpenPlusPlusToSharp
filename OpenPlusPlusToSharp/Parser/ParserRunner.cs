using OpenPlusPlusToSharp.Parser.Exceptions;
using System;
using System.Collections.Generic;

namespace OpenPlusPlusToSharp.Parser
{
    /// <summary>
    /// Contains methods to run a list of parsers over a provided context.
    /// </summary>
    public class ParserRunner
    {
        /// <summary>
        /// As long as there are tokens left the parsers are run over the context. All results are appended to the children of the parentNode.
        /// </summary>
        /// <param name="parsers"></param>
        /// <param name="context"></param>
        /// <param name="parentNode"></param>
        public static void RunAllParsersWhileThereAreTokensLeft(List<IParser> parsers, ParseContext context, ParseNode parentNode)
        {
            while (!context.AllTokensProcessed())
            {
                RunAllParsers(parsers, context, parentNode);
            }
        }

        /// <summary>
        /// The first parser that is able to parse 1 or more of the tokens from the current context is used.
        /// The result will be appended as child to the parend node.
        /// </summary>
        /// <param name="parsers"></param>
        /// <param name="context"></param>
        /// <param name="parentNode"></param>
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
