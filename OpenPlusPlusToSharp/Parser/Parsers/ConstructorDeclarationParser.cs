using OpenPlusPlusToSharp.Parser.Exceptions;
using OpenPlusPlusToSharp.Tokenizer;
using System;
using System.Collections.Generic;

namespace OpenPlusPlusToSharp.Parser.Parsers
{
    public class ConstructorDeclarationParser : IParser
    {
        private List<IParser> _parsers;

        public ConstructorDeclarationParser(IParser argumentListParser)
        {
            _parsers = new List<IParser>
            {
                argumentListParser
            };
        }

        public ParseResult TryParse(ParseContext context)
        {
            if(!context.CheckFutureToken(0, TokenType.Text) || context.CheckFutureToken(1, TokenType.SpecialCharacter, "("))
            {
                return ParseResult.CouldNotParse();
            }

            var constructorNameToken = context.GetCurrentToken();

            var offsetOfClosingBracket = ParserHelper.FindClosingBracket(context, 2);
            if(offsetOfClosingBracket < 0)
            {
                throw new ParseException($"Could not find closing bracket for constructor declaration of {constructorNameToken.Content} (opening bracket in line {constructorNameToken.LineNumber})");
            }

            var endIndex = offsetOfClosingBracket + 1;
            if (!context.CheckFutureToken(endIndex, TokenType.SpecialCharacter, ";"))
            {
                return ParseResult.CouldNotParse();
            }

            var constructorNode = new ParseNode(constructorNameToken.Content, NodeType.ConstructorDeclaration);
            var argumentList = new ParseNode("", NodeType.ArgumentList);
            constructorNode.Descendents.Add(argumentList);

            ParserRunner.RunAllParsers(_parsers, context.CreateSubContext(2, offsetOfClosingBracket), argumentList);


            var readTokens = endIndex - context.CurrentIndex;

            return ParseResult.ParseSuccess(constructorNode, readTokens);
        }
    }
}
