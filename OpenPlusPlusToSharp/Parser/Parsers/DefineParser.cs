using OpenPlusPlusToSharp.Constants;
using OpenPlusPlusToSharp.Parser.Exceptions;
using OpenPlusPlusToSharp.Tokenizer;

namespace OpenPlusPlusToSharp.Parser.Parsers
{
    /// <summary>
    /// Parser that can parse define directives.
    /// </summary>
    public class DefineParser : IParser
    {
        /// <summary>
        /// Returns a parsed node for the define directive.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public ParseResult TryParse(ParseContext context)
        {
            var currentToken = context.GetCurrentToken();

            switch (currentToken.Content)
            {
                case Directives.DefineDirective:
                    return TryParseDefineDirective(context);
                case Directives.IfNotDefinedDirective:
                    return TryParseIfNotDefinedDirective(context);
                case Directives.EndIfDirective:
                    return TryParseEndIfDirective();
            }

            return ParseResult.CouldNotParse();
        }

        private ParseResult TryParseEndIfDirective()
        {
            var endIfNode = new ParseNode("", NodeType.EndIfDirective);

            return ParseResult.ParseSuccess(endIfNode, 1);
        }

        private ParseResult TryParseIfNotDefinedDirective(ParseContext context)
        {
            var nextToken = context.GetFutureToken(1);

            if(nextToken.TokenType != TokenType.Text)
            {
                return ParseResult.CouldNotParse();
            }

            VerifyExistenceOfMatchingEndIf(context, nextToken.Content);
            var ifNotDefinedNode = new ParseNode(nextToken.Content, NodeType.IfNotDefinedDirective);

            return ParseResult.ParseSuccess(ifNotDefinedNode, 2);
        }

        private void VerifyExistenceOfMatchingEndIf(ParseContext context, string defineName)
        {
            var foundMatchingEndIf = false;
            var offset = 2; //the first two tokens are for the ifndef <name>
            var openIfStatements = 1;

            while (!foundMatchingEndIf)
            {
                var futureToken = context.GetFutureToken(offset++);
                if(futureToken == null)
                {
                    throw new ParseException($"Could not find a matching #endif for the #ifndef {defineName}");
                }

                if(futureToken.Content == Directives.IfNotDefinedDirective)
                {
                    openIfStatements++;
                }

                if(futureToken.Content == Directives.EndIfDirective)
                {
                    openIfStatements--;
                }

                if(openIfStatements == 0)
                {
                    foundMatchingEndIf = true;
                }
            }
        }

        private ParseResult TryParseDefineDirective(ParseContext context)
        {
            var nextToken = context.GetFutureToken(1);

            if (nextToken.TokenType != TokenType.Text)
            {
                return ParseResult.CouldNotParse();
            }

            var defineNode = new ParseNode(nextToken.Content, NodeType.DefineDirective);

            return ParseResult.ParseSuccess(defineNode, 2);
        }
    }
}
