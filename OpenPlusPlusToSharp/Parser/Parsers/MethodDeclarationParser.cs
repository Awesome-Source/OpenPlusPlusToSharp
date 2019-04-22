using OpenPlusPlusToSharp.Parser.Exceptions;
using OpenPlusPlusToSharp.Tokenizer;
using System.Collections.Generic;

namespace OpenPlusPlusToSharp.Parser.Parsers
{
    /// <summary>
    /// Parser that can parse a method declaration.
    /// </summary>
    public class MethodDeclarationParser : IParser
    {
        private readonly IParser _typeParser;
        private readonly List<IParser> _parsers;

        public MethodDeclarationParser(IParser parameterListParser, IParser typeParser)
        {
            _parsers = new List<IParser>
            {
                parameterListParser
            };
            _typeParser = typeParser;
        }

        public ParseResult TryParse(ParseContext context)
        {
            var tokenOffset = 0;
            var parseResult = _typeParser.TryParse(context.CreateSubContext(tokenOffset));
            if (!parseResult.Success)
            {
                var currentToken = context.GetFutureToken(tokenOffset);
                throw new ParseException($"Could not parse method return type near line {currentToken.LineNumber} column {currentToken.Column}");
            }

            tokenOffset += parseResult.ReadTokens;

            if (!context.CheckFutureToken(tokenOffset, TokenType.Text) || !context.CheckFutureToken(tokenOffset + 1, TokenType.SpecialCharacter, "("))
            {
                return ParseResult.CouldNotParse();
            }

            var methodNameToken = context.GetFutureToken(tokenOffset);

            var offsetOfClosingBracket = ParserHelper.FindClosingBracket(context, tokenOffset + 1);
            if (offsetOfClosingBracket < 0)
            {
                throw new ParseException($"Could not find closing bracket for method declaration of {methodNameToken.Content} (opening bracket in line {methodNameToken.LineNumber})");
            }

            var endOffset = offsetOfClosingBracket + 1;
            if (!context.CheckFutureToken(endOffset, TokenType.SpecialCharacter, ";"))
            {
                return ParseResult.CouldNotParse();
            }

            var methodDeclarationNode = new ParseNode(methodNameToken.Content, NodeType.MethodDeclaration);
            var returnTypeNode = parseResult.ParsedNode;
            methodDeclarationNode.Descendents.Add(returnTypeNode);
            ParserRunner.RunAllParsers(_parsers, context.CreateSubContext(tokenOffset + 2, offsetOfClosingBracket), methodDeclarationNode);
            var readTokens = endOffset + tokenOffset - 1;

            return ParseResult.ParseSuccess(methodDeclarationNode, readTokens + 1);
        }
    }
}
