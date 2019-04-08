using System.Collections.Generic;
using OpenPlusPlusToSharp.Constants;
using OpenPlusPlusToSharp.Parser.Exceptions;
using OpenPlusPlusToSharp.Tokenizer;

namespace OpenPlusPlusToSharp.Parser.Parsers
{
    public class ClassParser : IParser
    {
        private List<IParser> _subParsers;

        public ClassParser(IParser methodDeclarationParser)
        {
            _subParsers = new List<IParser>
            {
                methodDeclarationParser
            };
        }

        public ParseResult TryParse(ParseContext context)
        {
            var currentToken = context.GetCurrentToken();

            if(currentToken.Content != KeyWords.Class)
            {
                return ParseResult.CouldNotParse();
            }

            var classNameToken = context.GetFutureToken(1);
            if(classNameToken.TokenType != TokenType.Text)
            {
                return ParseResult.CouldNotParse();
            }

            var classDeclarationNode = new ParseNode(classNameToken.Content, NodeType.ClassDeclaration);

            if(context.CheckFutureToken(2, TokenType.SpecialCharacter, ":"))
            {
                return TryParseClassWithSuperClasses(classDeclarationNode, context);
            }

            if (context.CheckFutureToken(2, TokenType.SpecialCharacter, "{"))
            {
                return TryParseClass(classDeclarationNode, context, 2);
            }

            return ParseResult.CouldNotParse();
        }

        private ParseResult TryParseClass(ParseNode classNode, ParseContext context, int tokenOffset)
        {
            var tokenOffsetOfClosingCurlyBracket = FindClosingCurlyBracket(context, classNode, tokenOffset);
            ParserRunner.RunAllParsers(_subParsers, context.CreateSubContext(tokenOffset + 1, tokenOffsetOfClosingCurlyBracket), classNode);

            return ParseResult.ParseSuccess(classNode, tokenOffsetOfClosingCurlyBracket);
        }

        private int FindClosingCurlyBracket(ParseContext context, ParseNode classNode, int tokenOffset)
        {
            var foundClosingBracket = false;
            var openBracketCount = 1;

            while (!foundClosingBracket)
            {
                var nextToken = context.GetFutureToken(++tokenOffset);

                if(nextToken == null)
                {
                    throw new ParseException($"Missing closing bracket for class {classNode.Content}");
                }

                if(nextToken.TokenType != TokenType.SpecialCharacter)
                {
                    continue;
                }

                if(nextToken.Content == "{")
                {
                    openBracketCount++;
                }

                if (nextToken.Content == "}")
                {
                    openBracketCount--;
                }

                if(openBracketCount == 0)
                {
                    foundClosingBracket = true;
                }
            }

            return tokenOffset;
        }

        private ParseResult TryParseClassWithSuperClasses(ParseNode classNode, ParseContext context)
        {
            var canParseClassInheritance = true;
            var tokenOffset = 2;

            while (canParseClassInheritance)
            {
                ParseSingleClassInheritance(classNode, context, ref tokenOffset);
                canParseClassInheritance = ConsumeCommaToken(context, ref tokenOffset);
            }            

            return TryParseClass(classNode, context, tokenOffset);
        }

        private bool ConsumeCommaToken(ParseContext context, ref int tokenOffset)
        {
            var nextToken = context.GetFutureToken(tokenOffset + 1);

            if(nextToken != null && nextToken.TokenType == TokenType.SpecialCharacter && nextToken.Content == ",")
            {
                tokenOffset++;
                return true;
            }

            return false;
        }

        private static void ParseSingleClassInheritance(ParseNode classNode, ParseContext context,  ref int tokenOffset)
        {
            var classNameOrAccessModifierToken = context.GetFutureToken(++tokenOffset);
            ThrowIfTokenIsNullOrNoText(classNode, classNameOrAccessModifierToken);

            var accessModifier = KeyWords.Private;

            if (KeyWords.IsAccessModifier(classNameOrAccessModifierToken.Content))
            {
                accessModifier = classNameOrAccessModifierToken.Content;
                var superClassNameToken = context.GetFutureToken(++tokenOffset);
                ThrowIfTokenIsNullOrNoText(classNode, superClassNameToken);
                AddClassInheritanceToClassNode(classNode, accessModifier, superClassNameToken);
            }
            else
            {
                AddClassInheritanceToClassNode(classNode, accessModifier, classNameOrAccessModifierToken);
            }
        }

        private static void AddClassInheritanceToClassNode(ParseNode classNode, string accessModifier, Token superClassNameToken)
        {
            var superClassNode = new ParseNode(superClassNameToken.Content, NodeType.ClassInheritanceDeclaration);
            var accessModifierNode = new ParseNode(accessModifier, NodeType.AccessModifier);
            superClassNode.Descendents.Add(accessModifierNode);
            classNode.Descendents.Add(superClassNode);
        }

        private static void ThrowIfTokenIsNullOrNoText(ParseNode classNode, Token superClassNameToken)
        {
            if (superClassNameToken == null || superClassNameToken.TokenType != TokenType.Text)
            {
                throw new ParseException($"Expected class name of the class that should be inherited from. Check the inherited classes of the class {classNode.Content}.");
            }
        }
    }
}
