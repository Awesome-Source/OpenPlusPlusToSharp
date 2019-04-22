using OpenPlusPlusToSharp.Parser.Exceptions;
using OpenPlusPlusToSharp.Tokenizer;

namespace OpenPlusPlusToSharp.Parser.Parsers
{
    /// <summary>
    /// Parser that can parse a parameter list
    /// </summary>
    public class ParameterListParser : IParser
    {
        private readonly IParser _typeParser;

        /// <summary>
        /// Creates a new instance of the <see cref="ParameterListParser"/> class.
        /// </summary>
        /// <param name="typeParser"></param>
        public ParameterListParser(IParser typeParser)
        {
            _typeParser = typeParser;
        }

        public ParseResult TryParse(ParseContext context)
        {
            var parameterListNode = new ParseNode("", NodeType.ParameterList);
            var tokenOffset = 0;

            while (!context.VirtuallyAllTokensProcessed(tokenOffset))
            {
                var parseResult = _typeParser.TryParse(context.CreateSubContext(tokenOffset));
                if (!parseResult.Success)
                {
                    var currentToken = context.GetFutureToken(tokenOffset);
                    throw new ParseException($"Could not parse parameter type near line {currentToken.LineNumber} column {currentToken.Column}");
                }

                tokenOffset += parseResult.ReadTokens;
                var parameterTypeNode = parseResult.ParsedNode;
                var parameterNameToken = context.GetFutureToken(tokenOffset++);

                if(parameterNameToken == null || parameterNameToken.TokenType != TokenType.Text)
                {
                    return ParseResult.CouldNotParse();
                }

                var parameterNode = new ParseNode("", NodeType.Paramter);
                
                var parameterNameNode = new ParseNode(parameterNameToken.Content, NodeType.SymbolName);
                parameterNode.Descendents.Add(parameterTypeNode);
                parameterNode.Descendents.Add(parameterNameNode);
                parameterListNode.Descendents.Add(parameterNode);

                if(context.CheckFutureToken(tokenOffset, TokenType.SpecialCharacter, "="))
                {
                    AssignDefaultValue(context, ref tokenOffset, parameterNode);
                }

                if (context.CheckFutureToken(tokenOffset, TokenType.SpecialCharacter, ","))
                {
                    ++tokenOffset;
                }
            }

            return ParseResult.ParseSuccess(parameterListNode, tokenOffset + 1);
        }

        private static void AssignDefaultValue(ParseContext context, ref int tokenOffset, ParseNode parameterNode)
        {
            var assignmentToken = context.GetFutureToken(tokenOffset++);
            var defaultValueToken = context.GetFutureToken(tokenOffset++);
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
