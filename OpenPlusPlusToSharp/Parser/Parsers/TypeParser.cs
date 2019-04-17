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
            if(!context.CheckFutureToken(readTokens + 1, TokenType.SpecialCharacter, "<"))
            {
                return;
            }

            readTokens++;
            var offsetOfClosingBracket = ParserHelper.FindClosingAngleBracket(context, readTokens + 1);

            if(offsetOfClosingBracket == -1)
            {
                throw new ParseException($"Could not find closing angle bracket for type name {typeNameNode.Content}");
            }

            var templateTypeDefinitionNode = new ParseNode("", NodeType.TemplateType);
            typeNameNode.Descendents.Add(templateTypeDefinitionNode);

            var subcontext = context.CreateSubContext(readTokens + 1, offsetOfClosingBracket);
            var parseResult = TryParse(subcontext);

            if (!parseResult.Success)
            {
                throw new ParseException($"Could not parse template type {typeNameNode.Content}");
            }

            readTokens += parseResult.ReadTokens;
            typeNameNode.Descendents.Add(parseResult.ParsedNode);
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
