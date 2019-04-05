using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenPlusPlusToSharp.Tokenizer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace OpenPlusPlusToSharpTests
{
    [TestClass]
    public class TokenizerTests
    {
        [TestMethod]
        public void TokenizeInclude_ReturnsTokensForBothPartsOfTheInclude()
        {
            var source = "#include <string>";
            var tokenizer = new PlusPlusTokenizer(source);
            var tokens = tokenizer.Tokenize();

            Assert.AreEqual(2, tokens.Count);
            CheckToken(tokens[0], "#include", TokenType.Text, 1, 1);
            CheckToken(tokens[1], "<string>", TokenType.Text, 1, 10);
        }

        private void CheckToken(Token token, string expectedContent, TokenType expectedType, int expectedLineNumber, int expectedColumnIndex)
        {
            Assert.AreEqual(expectedContent, token.Content);
            Assert.AreEqual(expectedType, token.TokenType);
            Assert.AreEqual(expectedLineNumber, token.LineNumber);
            Assert.AreEqual(expectedColumnIndex, token.Column);
        }

        [TestMethod]
        public void TokenizeIncludeWithSurroundingWhiteSpaces_ReturnsTokensForBothPartsOfTheInclude()
        {
            var source = " #include <string> ";
            var tokenizer = new PlusPlusTokenizer(source);
            var tokens = tokenizer.Tokenize();

            Assert.AreEqual(2, tokens.Count);
            CheckToken(tokens[0], "#include", TokenType.Text, 1, 2);
            CheckToken(tokens[1], "<string>", TokenType.Text, 1, 11);
        }

        [TestMethod]
        public void TokenizeIntVariableAssignment_ReturnsFourTokensOfTypeTextAndOneSemicolon()
        {
            var source = "int testInt = 42;";
            var tokenizer = new PlusPlusTokenizer(source);
            var tokens = tokenizer.Tokenize();

            Assert.AreEqual(5, tokens.Count);
            CheckToken(tokens[0], "int", TokenType.Text, 1, 1);
            CheckToken(tokens[1], "testInt", TokenType.Text, 1, 5);
            CheckToken(tokens[2], "=", TokenType.Text, 1, 13);
            CheckToken(tokens[3], "42", TokenType.Text, 1, 15);
            CheckToken(tokens[4], ";", TokenType.Semicolon, 1, 17);
        }

        [TestMethod]
        public void TokenizeTwoIntVariableAssignments_OneVariableDeclarationIsDirectlyBehindTheOther_ReturnsEightTokensOfTypeTextAndTwoSemicolons()
        {
            var source = "int testInt = 42;int testInt2 = 1337;";
            var tokenizer = new PlusPlusTokenizer(source);
            var tokens = tokenizer.Tokenize();

            Assert.AreEqual(10, tokens.Count);
            CheckToken(tokens[0], "int", TokenType.Text, 1, 1);
            CheckToken(tokens[1], "testInt", TokenType.Text, 1, 5);
            CheckToken(tokens[2], "=", TokenType.Text, 1, 13);
            CheckToken(tokens[3], "42", TokenType.Text, 1, 15);
            CheckToken(tokens[4], ";", TokenType.Semicolon, 1, 17);
            CheckToken(tokens[5], "int", TokenType.Text, 1, 18);
            CheckToken(tokens[6], "testInt2", TokenType.Text, 1, 22);
            CheckToken(tokens[7], "=", TokenType.Text, 1, 31);
            CheckToken(tokens[8], "1337", TokenType.Text, 1, 33);
            CheckToken(tokens[9], ";", TokenType.Semicolon, 1, 37);
        }

        [TestMethod]
        public void TokenizeTwoIntVariableAssignmentsInDifferentLines_ReturnsTokensWithDifferentSourceLines()
        {
            var source = "int testInt = 42;\nint testInt2 = 1337;";
            var tokenizer = new PlusPlusTokenizer(source);
            var tokens = tokenizer.Tokenize();

            Assert.AreEqual(10, tokens.Count);
            CheckToken(tokens[0], "int", TokenType.Text, 1, 1);
            CheckToken(tokens[1], "testInt", TokenType.Text, 1, 5);
            CheckToken(tokens[2], "=", TokenType.Text, 1, 13);
            CheckToken(tokens[3], "42", TokenType.Text, 1, 15);
            CheckToken(tokens[4], ";", TokenType.Semicolon, 1, 17);
            CheckToken(tokens[5], "int", TokenType.Text, 2, 1);
            CheckToken(tokens[6], "testInt2", TokenType.Text, 2, 5);
            CheckToken(tokens[7], "=", TokenType.Text, 2, 14);
            CheckToken(tokens[8], "1337", TokenType.Text, 2, 16);
            CheckToken(tokens[9], ";", TokenType.Semicolon, 2, 20);
        }

        [TestMethod]
        public void TokenizeString_ReturnsFiveTokensWithOneTokenForTheCompleteStringLiteral()
        {
            var source = "std::string testString = \"awesome string\";";
            var tokenizer = new PlusPlusTokenizer(source);
            var tokens = tokenizer.Tokenize();

            Assert.AreEqual(5, tokens.Count);
            CheckToken(tokens[0], "std::string", TokenType.Text, 1, 1);
            CheckToken(tokens[1], "testString", TokenType.Text, 1, 13);
            CheckToken(tokens[2], "=", TokenType.Text, 1, 24);
            CheckToken(tokens[3], "\"awesome string\"", TokenType.Text, 1, 26);
            CheckToken(tokens[4], ";", TokenType.Semicolon, 1, 42);
        }

        [TestMethod]
        public void TokenizeStringWithEscapedQuote_ReturnsFiveTokensWithOneTokenForTheCompleteStringLiteralContainingTheEscapedQuote()
        {
            var source = "std::string testString = \"awesome \\\" string\";";
            var tokenizer = new PlusPlusTokenizer(source);
            var tokens = tokenizer.Tokenize();

            Assert.AreEqual(5, tokens.Count);
            CheckToken(tokens[0], "std::string", TokenType.Text, 1, 1);
            CheckToken(tokens[1], "testString", TokenType.Text, 1, 13);
            CheckToken(tokens[2], "=", TokenType.Text, 1, 24);
            CheckToken(tokens[3], "\"awesome \\\" string\"", TokenType.Text, 1, 26);
            CheckToken(tokens[4], ";", TokenType.Semicolon, 1, 45);
        }

        [TestMethod]
        public void TokenizeStringWithEscapedBackSlash_ReturnsFiveTokensWithOneTokenForTheCompleteStringLiteralContainingTheEscapedBackSlash()
        {
            var source = "std::string testString = \"awesome \\\\ string\";";
            var tokenizer = new PlusPlusTokenizer(source);
            var tokens = tokenizer.Tokenize();

            Assert.AreEqual(5, tokens.Count);
            CheckToken(tokens[0], "std::string", TokenType.Text, 1, 1);
            CheckToken(tokens[1], "testString", TokenType.Text, 1, 13);
            CheckToken(tokens[2], "=", TokenType.Text, 1, 24);
            CheckToken(tokens[3], "\"awesome \\\\ string\"", TokenType.Text, 1, 26);
            CheckToken(tokens[4], ";", TokenType.Semicolon, 1, 45);
        }

        [TestMethod]
        public void TokenizeStringWithEscapedNewLine_ReturnsFiveTokensWithOneTokenForTheCompleteStringLiteralContainingTheEscapedNewLine()
        {
            var source = "std::string testString = \"awesome \\n string\";";
            var tokenizer = new PlusPlusTokenizer(source);
            var tokens = tokenizer.Tokenize();

            Assert.AreEqual(5, tokens.Count);
            CheckToken(tokens[0], "std::string", TokenType.Text, 1, 1);
            CheckToken(tokens[1], "testString", TokenType.Text, 1, 13);
            CheckToken(tokens[2], "=", TokenType.Text, 1, 24);
            CheckToken(tokens[3], "\"awesome \\n string\"", TokenType.Text, 1, 26);
            CheckToken(tokens[4], ";", TokenType.Semicolon, 1, 45);
        }

        [TestMethod]
        public void TokenizeForLoop_Return17TokensWithTokenTypesForBracketsAndBlocksAndSemicolons()
        {
            var source = "for(int i = 0; i < 42; i++){d++;}";
            var tokenizer = new PlusPlusTokenizer(source);
            var tokens = tokenizer.Tokenize();

            Assert.AreEqual(17, tokens.Count);
            CheckToken(tokens[0], "for", TokenType.Text, 1, 1);
            CheckToken(tokens[1], "(", TokenType.OpeningBracket, 1, 4);
            CheckToken(tokens[2], "int", TokenType.Text, 1, 5);
            CheckToken(tokens[3], "i", TokenType.Text, 1, 9);
            CheckToken(tokens[4], "=", TokenType.Text, 1, 11);
            CheckToken(tokens[5], "0", TokenType.Text, 1, 13);
            CheckToken(tokens[6], ";", TokenType.Semicolon, 1, 14);
            CheckToken(tokens[7], "i", TokenType.Text, 1, 16);
            CheckToken(tokens[8], "<", TokenType.Text, 1, 18);
            CheckToken(tokens[9], "42", TokenType.Text, 1, 20);
            CheckToken(tokens[10], ";", TokenType.Semicolon, 1, 22);
            CheckToken(tokens[11], "i++", TokenType.Text, 1, 24);
            CheckToken(tokens[12], ")", TokenType.ClosingBracket, 1, 27);
            CheckToken(tokens[13], "{", TokenType.BeginBlock, 1, 28);
            CheckToken(tokens[14], "d++", TokenType.Text, 1, 29);
            CheckToken(tokens[15], ";", TokenType.Semicolon, 1, 32);
            CheckToken(tokens[16], "}", TokenType.EndBlock, 1, 33);
        }
    }
}
