using System;
using OpenPlusPlusToSharp.Parser.Exceptions;
using OpenPlusPlusToSharp.Tokenizer;

namespace OpenPlusPlusToSharp.Parser.Parsers
{
    public class TypeParser : IParser
    {
        public ParseResult TryParse(ParseContext context)
        {
            var readTokens = -1;
            var topLevelTypeNameToken = context.GetFutureToken(++readTokens);

            if (topLevelTypeNameToken == null || topLevelTypeNameToken.TokenType != TokenType.Text)
            {
                return ParseResult.CouldNotParse();
            }

            var typeNameNode = new ParseNode(topLevelTypeNameToken.Content, NodeType.TypeName);

            HandleTemplateType(context, ref readTokens, typeNameNode);
            HandlePointerAndReferenceTypes(context, ref readTokens, typeNameNode);

            return ParseResult.ParseSuccess(typeNameNode, readTokens + 1);
        }

        private void HandleTemplateType(ParseContext context, ref int readTokens, ParseNode typeNameNode)
        {
            if (!context.CheckFutureToken(readTokens + 1, TokenType.SpecialCharacter, "<"))
            {
                return;
            }

            readTokens++;
            var offsetOfClosingBracket = ParserHelper.FindClosingAngleBracket(context, readTokens + 1);

            if (offsetOfClosingBracket == -1)
            {
                throw new ParseException($"Could not find closing angle bracket for type name {typeNameNode.Content}");
            }

            var templateTypeDefinitionNode = new ParseNode("", NodeType.TemplateType);
            typeNameNode.Descendents.Add(templateTypeDefinitionNode);
            ParseAllTemplateTypeParameters(context, ref readTokens, typeNameNode, offsetOfClosingBracket, templateTypeDefinitionNode);
        }

        private void ParseAllTemplateTypeParameters(ParseContext context, ref int readTokens, ParseNode typeNameNode, int offsetOfClosingBracket, ParseNode templateTypeDefinitionNode)
        {
            var allTemplateTypeParametersRead = false;
            var currentStartOffset = readTokens + 1;
            while (!allTemplateTypeParametersRead)
            {
                var currentEndOffset = ReadToNextCommaOrEnd(context, currentStartOffset, offsetOfClosingBracket);
                var subcontext = context.CreateSubContext(currentStartOffset, currentEndOffset);
                var parseResult = TryParse(subcontext);

                if (!parseResult.Success)
                {
                    throw new ParseException($"Could not parse template type {typeNameNode.Content}");
                }

                readTokens += parseResult.ReadTokens + 2;
                templateTypeDefinitionNode.Descendents.Add(parseResult.ParsedNode);

                currentStartOffset = currentEndOffset + 1;
                if (currentEndOffset == offsetOfClosingBracket)
                {
                    allTemplateTypeParametersRead = true;
                }
            }
        }

        private int ReadToNextCommaOrEnd(ParseContext context, int currentStartOffset, int offsetOfClosingBracket)
        {
            for(var i = currentStartOffset; i < offsetOfClosingBracket; i++)
            {
                var currentToken = context.GetFutureToken(i);
                if (currentToken != null && currentToken.TokenType == TokenType.SpecialCharacter && currentToken.Content == ",")
                {
                    return i;
                }
            }

            return offsetOfClosingBracket;
        }

        private static void HandlePointerAndReferenceTypes(ParseContext context, ref int readTokens, ParseNode typeNameNode)
        {
            while (context.CheckFutureToken(readTokens + 1, TokenType.SpecialCharacter))
            {
                var decisionToken = context.GetFutureToken(++readTokens);

                switch (decisionToken.Content)
                {
                    case "*":
                        typeNameNode.Descendents.Add(new ParseNode("*", NodeType.PointerType));
                        break;
                    case "&":
                        typeNameNode.Descendents.Add(new ParseNode("&", NodeType.ReferenceType));
                        break;
                    default:
                        throw new ParseException($"Unexpected token in type name: {decisionToken.Content}");
                }
            }
        }
    }
}
