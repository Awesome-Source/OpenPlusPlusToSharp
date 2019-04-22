using OpenPlusPlusToSharp.Parser.Exceptions;
using OpenPlusPlusToSharp.Tokenizer;
using System;
using System.Collections.Generic;

namespace OpenPlusPlusToSharp.Parser.Parsers
{
    /// <summary>
    /// Parser that can parse a constructor declaration.
    /// </summary>
    public class ConstructorDeclarationParser : IParser
    {
        private readonly List<IParser> _parsers;

        public ConstructorDeclarationParser(IParser argumentListParser)
        {
            _parsers = new List<IParser>
            {
                argumentListParser
            };
        }

        public ParseResult TryParse(ParseContext context)
        {
            if(!context.CheckFutureToken(0, TokenType.Text) || !context.CheckFutureToken(1, TokenType.SpecialCharacter, "("))
            {
                return ParseResult.CouldNotParse();
            }

            var constructorNameToken = context.GetFutureToken(0);

            if (constructorNameToken.Content.StartsWith('~'))
            {
                return ParseResult.CouldNotParse();
            }

            var offsetOfClosingBracket = ParserHelper.FindClosingBracket(context, 2);
            if(offsetOfClosingBracket < 0)
            {
                throw new ParseException($"Could not find closing bracket for constructor declaration of {constructorNameToken.Content} (opening bracket in line {constructorNameToken.LineNumber})");
            }

            var endOffset = offsetOfClosingBracket + 1;
            if (!context.CheckFutureToken(endOffset, TokenType.SpecialCharacter, ";"))
            {
                return ParseResult.CouldNotParse();
            }

            var constructorNode = new ParseNode(constructorNameToken.Content, NodeType.ConstructorDeclaration);
            ParserRunner.RunAllParsers(_parsers, context.CreateSubContext(2, offsetOfClosingBracket), constructorNode);
            var readTokens = endOffset + context.CurrentIndex + 1;

            return ParseResult.ParseSuccess(constructorNode, readTokens);
        }
    }
}
