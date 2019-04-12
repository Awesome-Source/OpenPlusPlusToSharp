using OpenPlusPlusToSharp.Parser.Exceptions;
using OpenPlusPlusToSharp.Tokenizer;
using System;
using System.Collections.Generic;

namespace OpenPlusPlusToSharp.Parser.Parsers
{
    public class DestructorDeclarationParser : IParser
    {
        private List<IParser> _parsers;

        public DestructorDeclarationParser(IParser argumentListParser)
        {
            _parsers = new List<IParser>
            {
                argumentListParser
            };
        }

        public ParseResult TryParse(ParseContext context)
        {
            //TODO check for virtual keyword before destructor

            if (!context.CheckFutureToken(0, TokenType.Text) || context.CheckFutureToken(1, TokenType.SpecialCharacter, "("))
            {
                return ParseResult.CouldNotParse();
            }

            var destructorNameToken = context.GetCurrentToken();

            if (!destructorNameToken.Content.StartsWith('~'))
            {
                return ParseResult.CouldNotParse();
            }

            var offsetOfClosingBracket = ParserHelper.FindClosingBracket(context, 2);
            if (offsetOfClosingBracket < 0)
            {
                throw new ParseException($"Could not find closing bracket for destructor declaration of {destructorNameToken.Content} (opening bracket in line {destructorNameToken.LineNumber})");
            }

            var endOffset = offsetOfClosingBracket + 1;
            if (!context.CheckFutureToken(endOffset, TokenType.SpecialCharacter, ";"))
            {
                return ParseResult.CouldNotParse();
            }

            var destructorNode = new ParseNode(destructorNameToken.Content, NodeType.DestructorDeclaration);
            ParserRunner.RunAllParsers(_parsers, context.CreateSubContext(2, offsetOfClosingBracket), destructorNode);
            var readTokens = endOffset + context.CurrentIndex;

            return ParseResult.ParseSuccess(destructorNode, readTokens);
        }
    }
}
