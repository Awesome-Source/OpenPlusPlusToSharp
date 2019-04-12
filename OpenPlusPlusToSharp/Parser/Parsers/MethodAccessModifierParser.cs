using OpenPlusPlusToSharp.Constants;
using OpenPlusPlusToSharp.Parser.Exceptions;
using OpenPlusPlusToSharp.Tokenizer;
using System;
using System.Collections.Generic;

namespace OpenPlusPlusToSharp.Parser.Parsers
{
    public class MethodAccessModifierParser : IParser
    {
        private List<IParser> _parsers;

        public MethodAccessModifierParser(IParser constructorParser, IParser destructorParser, IParser methodDeclarationParser)
        {
            _parsers = new List<IParser>
            {
                constructorParser,
                destructorParser,
                methodDeclarationParser
            };
        }

        public ParseResult TryParse(ParseContext context)
        {
            var methodListNode = new ParseNode("", NodeType.MethodDeclarationList);
            var readTokens = 0;

            while (!context.VirtuallyAllTokensProcessed(readTokens))
            {
                var accessibilityNode = ReadOrCreateMethodAccessibilityNode(context, ref readTokens);
                var endOffset = DetermineEndOffsetOfMethodAccessibility(context, readTokens);
                ParserRunner.RunAllParsersWhileThereAreTokensLeft(_parsers, context.CreateSubContext(readTokens, endOffset), accessibilityNode);
                methodListNode.Descendents.Add(accessibilityNode);
                readTokens = endOffset;
            }

            return ParseResult.ParseSuccess(methodListNode, readTokens);
        }

        private int DetermineEndOffsetOfMethodAccessibility(ParseContext context, int offset)
        {
            while (!context.VirtuallyAllTokensProcessed(offset))
            {
                if (!context.CheckFutureToken(offset, TokenType.Text) || !context.CheckFutureToken(offset + 1, TokenType.SpecialCharacter, ":"))
                {
                    offset++;
                    continue;
                }

                var possibleAccessModifierToken = context.GetFutureToken(offset);
                if(possibleAccessModifierToken == null || !KeyWords.IsAccessModifier(possibleAccessModifierToken.Content))
                {
                    offset++;
                    continue;
                }

                return offset;
            }

            return offset;
        }

        private ParseNode ReadOrCreateMethodAccessibilityNode(ParseContext context, ref int readTokens)
        {
            if(!context.CheckFutureToken(readTokens, TokenType.Text) || !context.CheckFutureToken(readTokens + 1, TokenType.SpecialCharacter, ":"))
            {
                return new ParseNode(KeyWords.Private, NodeType.AccessModifier);
            }

            var accessModifier = context.GetFutureToken(readTokens);

            if (!KeyWords.IsAccessModifier(accessModifier.Content))
            {
                throw new ParseException($"Expected access modifer in line {accessModifier.LineNumber} column {accessModifier.Column}");
            }

            readTokens += 2;

            return new ParseNode(accessModifier.Content, NodeType.AccessModifier);
        }
    }
}
