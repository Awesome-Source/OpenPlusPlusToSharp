using OpenPlusPlusToSharp.Parser.Exceptions;
using OpenPlusPlusToSharp.Tokenizer;

namespace OpenPlusPlusToSharp.Parser.Parsers
{
    public class ParameterListParser : IParser
    {
        public ParseResult TryParse(ParseContext context)
        {
            var parameterListNode = new ParseNode("", NodeType.ParameterList);
            var tokenOffset = -1;

            while (!context.VirtuallyAllTokensProcessed(tokenOffset))
            {
                var typeToken = context.GetFutureToken(++tokenOffset);
                var parameterNameToken = context.GetFutureToken(++tokenOffset);

                if(typeToken == null || parameterNameToken == null || typeToken.TokenType != TokenType.Text || parameterNameToken.TokenType != TokenType.Text)
                {
                    return ParseResult.CouldNotParse();
                }

                var parameterNode = new ParseNode("", NodeType.Paramter);
                var parameterTypeNode = new ParseNode(typeToken.Content, NodeType.TypeName);
                var parameterNameNode = new ParseNode(parameterNameToken.Content, NodeType.SymbolName);
                parameterNode.Descendents.Add(parameterTypeNode);
                parameterNode.Descendents.Add(parameterNameNode);
                parameterListNode.Descendents.Add(parameterNode);

                if(context.CheckFutureToken(tokenOffset + 1, TokenType.SpecialCharacter, "="))
                {
                    AssignDefaultValue(context, ref tokenOffset, parameterNode);
                }

                if (context.CheckFutureToken(tokenOffset + 1, TokenType.SpecialCharacter, ","))
                {
                    ++tokenOffset;
                }
            }

            return ParseResult.ParseSuccess(parameterListNode, tokenOffset);
        }

        private static void AssignDefaultValue(ParseContext context, ref int tokenOffset, ParseNode parameterNode)
        {
            var assignmentToken = context.GetFutureToken(++tokenOffset);
            var defaultValueToken = context.GetFutureToken(++tokenOffset);
            if (defaultValueToken == null)
            {
                throw new ParseException($"Missing default value near line {assignmentToken.LineNumber} column {assignmentToken.Column}");
            }

            if (defaultValueToken.TokenType == TokenType.SpecialCharacter)
            {
                throw new ParseException($"Unexpected token {defaultValueToken.Content} in line {defaultValueToken.LineNumber} column {defaultValueToken.Column}");
            }

            var defaultValueNode = new ParseNode(defaultValueToken.Content, NodeType.DefaultValue);
            parameterNode.Descendents.Add(defaultValueNode);
        }
    }
}
