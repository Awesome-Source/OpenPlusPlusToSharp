using OpenPlusPlusToSharp.Constants;
using OpenPlusPlusToSharp.Tokenizer;

namespace OpenPlusPlusToSharp.Parser.Parsers
{
    /// <summary>
    /// Parser that can parse forward declarations.
    /// </summary>
    public class ForwardDeclarationParser : IParser
    {
        /// <summary>
        /// Returns a node with the class name of the forward declaration if the parse process was successfull.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public ParseResult TryParse(ParseContext context)
        {
            var currentToken = context.GetCurrentToken();

            if(currentToken.Content != KeyWords.Class)
            {
                return ParseResult.CouldNotParse();
            }

            var isValidForwardDeclaration = context.CheckFutureToken(1, TokenType.Text) && context.CheckFutureToken(2, TokenType.SpecialCharacter, ";");
            if (!isValidForwardDeclaration)
            {
                return ParseResult.CouldNotParse();
            }

            var className = context.GetFutureToken(1).Content;
            var forwardDeclarationNode = new ParseNode(className, NodeType.ForwardDeclaration);

            return ParseResult.ParseSuccess(forwardDeclarationNode, 3);
        }
    }
}
