using OpenPlusPlusToSharp.Constants;
using OpenPlusPlusToSharp.Tokenizer;

namespace OpenPlusPlusToSharp.Parser.Parsers
{
    /// <summary>
    /// Parser that can parse pragma directives
    /// </summary>
    public class PragmaParser : IParser
    {
        /// <summary>
        /// Returns a parsed node with the pragma content if the parser is successfull.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public ParseResult TryParse(ParseContext context)
        {
            var currentToken = context.GetCurrentToken();
            if(currentToken.Content != Directives.PragmaDirective)
            {
                return ParseResult.CouldNotParse();
            }

            return TryParsePragmaComment(context);
        }

        private ParseResult TryParsePragmaComment(ParseContext context)
        {
            var isValidPragmaComment = context.CheckFutureToken(1, TokenType.Text, "comment")
                && context.CheckFutureToken(2, TokenType.SpecialCharacter, "(")
                && context.CheckFutureToken(3, TokenType.Text)
                && context.CheckFutureToken(4, TokenType.SpecialCharacter, ",")
                && context.CheckFutureToken(5, TokenType.StringLiteral)
                && context.CheckFutureToken(6, TokenType.SpecialCharacter, ")");

            if (!isValidPragmaComment)
            {
                return ParseResult.CouldNotParse();
            }

            var commentTypeToken = context.GetFutureToken(1);
            var commentStringToken = context.GetFutureToken(5);

            var pragmaNode = new ParseNode($"comment({commentTypeToken.Content},{commentStringToken.Content})", NodeType.PragmaDirective);

            return ParseResult.ParseSuccess(pragmaNode, 7);
        }
    }
}
