using OpenPlusPlusToSharp.Constants;
using OpenPlusPlusToSharp.Tokenizer;

namespace OpenPlusPlusToSharp.Parser.Parsers
{
    /// <summary>
    /// Parser that can parse include directives.
    /// </summary>
    public class IncludeParser : IParser
    {
        /// <summary>
        /// If the parser is successfull it returns one node containing the file name of the include.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public ParseResult TryParse(ParseContext context)
        {
            var currentToken = context.GetCurrentToken();

            if (currentToken.Content != Directives.IncludeDirective)
            {
                return ParseResult.CouldNotParse();
            }

            if(context.CheckFutureToken(1, TokenType.StringLiteral))
            {
                return ParseStringInclude(context.GetFutureToken(1));
            }

            var isStandardInclude = context.CheckFutureToken(1, TokenType.SpecialCharacter, "<") 
                && context.CheckFutureToken(2, TokenType.Text) 
                && context.CheckFutureToken(3, TokenType.SpecialCharacter, ">");

            if (isStandardInclude)
            {
                return ParseStandardInclude(context.GetFutureToken(2));
            }

            return ParseResult.CouldNotParse();
        }

        private static ParseResult ParseStringInclude(Token nextToken)
        {
            var content = nextToken.Content.Replace("\"", string.Empty);
            var includeNode = new ParseNode(content, NodeType.IncludeDirective);

            return ParseResult.ParseSuccess(includeNode, 2);
        }

        private static ParseResult ParseStandardInclude(Token token)
        {
            var includeNode = new ParseNode(token.Content, NodeType.IncludeDirective);

            return ParseResult.ParseSuccess(includeNode, 4);
        }
    }
}
