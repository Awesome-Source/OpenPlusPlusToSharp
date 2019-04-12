using OpenPlusPlusToSharp.Parser.Exceptions;
using OpenPlusPlusToSharp.Tokenizer;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenPlusPlusToSharp.Parser.Parsers
{
    public class MethodDeclarationParser : IParser
    {
        private List<IParser> _parsers;

        public MethodDeclarationParser(IParser argumentListParser)
        {
            _parsers = new List<IParser>
            {
                argumentListParser
            };
        }

        public ParseResult TryParse(ParseContext context)
        {
            if (!context.CheckFutureToken(0, TokenType.Text) || !context.CheckFutureToken(1, TokenType.Text) || !context.CheckFutureToken(2, TokenType.SpecialCharacter, "("))
            {
                return ParseResult.CouldNotParse();
            }

            var returnTypeToken = context.GetFutureToken(0);
            var methodNameToken = context.GetFutureToken(1);

            var offsetOfClosingBracket = ParserHelper.FindClosingBracket(context, 2);
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
            var returnTypeNode = new ParseNode(returnTypeToken.Content, NodeType.ReturnType);
            methodDeclarationNode.Descendents.Add(returnTypeNode);
            ParserRunner.RunAllParsers(_parsers, context.CreateSubContext(2, offsetOfClosingBracket), methodDeclarationNode);
            var readTokens = endOffset + context.CurrentIndex;

            return ParseResult.ParseSuccess(methodDeclarationNode, readTokens);
        }
    }
}
