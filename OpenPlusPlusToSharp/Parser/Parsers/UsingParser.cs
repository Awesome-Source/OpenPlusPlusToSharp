using OpenPlusPlusToSharp.Constants;
using OpenPlusPlusToSharp.Tokenizer;

namespace OpenPlusPlusToSharp.Parser.Parsers
{
    /// <summary>
    /// Parser that can parse using statements
    /// </summary>
    public class UsingParser : IParser
    {
        /// <summary>
        /// Returns a parsed node for the using statement.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public ParseResult TryParse(ParseContext context)
        {
            var currentToken = context.GetFutureToken(0);

            if(currentToken.Content != KeyWords.Using)
            {
                return ParseResult.CouldNotParse();
            }

            return TryParseUsingNamespaceStatement(context);
        }

        private ParseResult TryParseUsingNamespaceStatement(ParseContext context)
        {
            var isValidUsingNamespaceStatement = context.CheckFutureToken(1, TokenType.Text, KeyWords.Namespace)
                && context.CheckFutureToken(2, TokenType.Text)
                && context.CheckFutureToken(3, TokenType.SpecialCharacter, ";");

            if (!isValidUsingNamespaceStatement)
            {
                return ParseResult.CouldNotParse();
            }

            var namespaceNameToken = context.GetFutureToken(2);
            var usingNamespaceNode = new ParseNode(namespaceNameToken.Content, NodeType.UsingNamespaceStatement);

            return ParseResult.ParseSuccess(usingNamespaceNode, 4);
        }
    }
}
