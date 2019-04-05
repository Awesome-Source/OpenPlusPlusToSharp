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
            Assert.AreEqual("#include", tokens[0].Content);
            Assert.AreEqual("<string>", tokens[1].Content);
            Assert.IsTrue(tokens.All(t => t.TokenType == TokenType.Text));
            CheckLineNumberAndColumnIndex(tokens[0], 1, 1);
            CheckLineNumberAndColumnIndex(tokens[1], 1, 10);
        }

        private void CheckLineNumberAndColumnIndex(Token token, int expectedLineNumber, int expectedColumnIndex)
        {
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
            Assert.AreEqual("#include", tokens[0].Content);
            Assert.AreEqual("<string>", tokens[1].Content);
            Assert.IsTrue(tokens.All(t => t.TokenType == TokenType.Text));
            CheckLineNumberAndColumnIndex(tokens[0], 1, 2);
            CheckLineNumberAndColumnIndex(tokens[1], 1, 11);
        }

        [TestMethod]
        public void TokenizeIntVariableAssignment_ReturnsFourTokensOfTypeText()
        {
            var source = "int testInt = 42;";
            var tokenizer = new PlusPlusTokenizer(source);
            var tokens = tokenizer.Tokenize();

            Assert.AreEqual(4, tokens.Count);
            Assert.AreEqual("int", tokens[0].Content);
            Assert.AreEqual("testInt", tokens[1].Content);
            Assert.AreEqual("=", tokens[2].Content);
            Assert.AreEqual("42;", tokens[3].Content);
            Assert.IsTrue(tokens.All(t => t.TokenType == TokenType.Text));
            CheckLineNumberAndColumnIndex(tokens[0], 1, 1);
            CheckLineNumberAndColumnIndex(tokens[1], 1, 5);
            CheckLineNumberAndColumnIndex(tokens[2], 1, 13);
            CheckLineNumberAndColumnIndex(tokens[3], 1, 15);
        }

        [TestMethod]
        public void TokenizeTwoIntVariableAssignments_OneVariableDeclarationIsDirectlyBehindTheOther_ReturnsEightTokensOfTypeText()
        {
            var source = "int testInt = 42;int testInt2 = 1337;";
            var tokenizer = new PlusPlusTokenizer(source);
            var tokens = tokenizer.Tokenize();

            Assert.AreEqual(8, tokens.Count);
            Assert.AreEqual("int", tokens[0].Content);
            Assert.AreEqual("testInt", tokens[1].Content);
            Assert.AreEqual("=", tokens[2].Content);
            Assert.AreEqual("42;", tokens[3].Content);
            Assert.AreEqual("int", tokens[4].Content);
            Assert.AreEqual("testInt2", tokens[5].Content);
            Assert.AreEqual("=", tokens[6].Content);
            Assert.AreEqual("1337;", tokens[7].Content);
            Assert.IsTrue(tokens.All(t => t.TokenType == TokenType.Text));
            CheckLineNumberAndColumnIndex(tokens[0], 1, 1);
            CheckLineNumberAndColumnIndex(tokens[1], 1, 5);
            CheckLineNumberAndColumnIndex(tokens[2], 1, 13);
            CheckLineNumberAndColumnIndex(tokens[3], 1, 15);
            CheckLineNumberAndColumnIndex(tokens[4], 1, 18);
            CheckLineNumberAndColumnIndex(tokens[5], 1, 22);
            CheckLineNumberAndColumnIndex(tokens[6], 1, 31);
            CheckLineNumberAndColumnIndex(tokens[7], 1, 33);
        }

        [TestMethod]
        public void TokenizeTwoIntVariableAssignmentsInDifferentLines_ReturnsEightTokensOfTypeTextWithDifferentSourceLines()
        {
            var source = "int testInt = 42;\nint testInt2 = 1337;";
            var tokenizer = new PlusPlusTokenizer(source);
            var tokens = tokenizer.Tokenize();

            Assert.AreEqual(8, tokens.Count);
            Assert.AreEqual("int", tokens[0].Content);
            Assert.AreEqual("testInt", tokens[1].Content);
            Assert.AreEqual("=", tokens[2].Content);
            Assert.AreEqual("42;", tokens[3].Content);
            Assert.AreEqual("int", tokens[4].Content);
            Assert.AreEqual("testInt2", tokens[5].Content);
            Assert.AreEqual("=", tokens[6].Content);
            Assert.AreEqual("1337;", tokens[7].Content);
            Assert.IsTrue(tokens.All(t => t.TokenType == TokenType.Text));
            CheckLineNumberAndColumnIndex(tokens[0], 1, 1);
            CheckLineNumberAndColumnIndex(tokens[1], 1, 5);
            CheckLineNumberAndColumnIndex(tokens[2], 1, 13);
            CheckLineNumberAndColumnIndex(tokens[3], 1, 15);
            CheckLineNumberAndColumnIndex(tokens[4], 2, 1);
            CheckLineNumberAndColumnIndex(tokens[5], 2, 5);
            CheckLineNumberAndColumnIndex(tokens[6], 2, 14);
            CheckLineNumberAndColumnIndex(tokens[7], 2, 16);
        }

        [TestMethod]
        public void TokenizeString_ReturnsFourTokensWithOneTokenForTheCompleteStringLiteral()
        {
            var source = "std::string testString = \"awesome string\";";
            var tokenizer = new PlusPlusTokenizer(source);
            var tokens = tokenizer.Tokenize();

            Assert.AreEqual(4, tokens.Count);
            Assert.AreEqual("std::string", tokens[0].Content);
            Assert.AreEqual("testString", tokens[1].Content);
            Assert.AreEqual("=", tokens[2].Content);
            Assert.AreEqual("\"awesome string\";", tokens[3].Content);
            Assert.IsTrue(tokens.All(t => t.TokenType == TokenType.Text));
            CheckLineNumberAndColumnIndex(tokens[0], 1, 1);
            CheckLineNumberAndColumnIndex(tokens[1], 1, 13);
            CheckLineNumberAndColumnIndex(tokens[2], 1, 24);
            CheckLineNumberAndColumnIndex(tokens[3], 1, 26);
        }

        [TestMethod]
        public void TokenizeStringWithEscapedQuote_ReturnsFourTokensWithOneTokenForTheCompleteStringLiteralContainingTheEscapedQuote()
        {
            var source = "std::string testString = \"awesome \\\" string\";";
            var tokenizer = new PlusPlusTokenizer(source);
            var tokens = tokenizer.Tokenize();

            Assert.AreEqual(4, tokens.Count);
            Assert.AreEqual("std::string", tokens[0].Content);
            Assert.AreEqual("testString", tokens[1].Content);
            Assert.AreEqual("=", tokens[2].Content);
            Assert.AreEqual("\"awesome \\\" string\";", tokens[3].Content);
            Assert.IsTrue(tokens.All(t => t.TokenType == TokenType.Text));
            CheckLineNumberAndColumnIndex(tokens[0], 1, 1);
            CheckLineNumberAndColumnIndex(tokens[1], 1, 13);
            CheckLineNumberAndColumnIndex(tokens[2], 1, 24);
            CheckLineNumberAndColumnIndex(tokens[3], 1, 26);
        }

        [TestMethod]
        public void TokenizeStringWithEscapedBackSlash_ReturnsFourTokensWithOneTokenForTheCompleteStringLiteralContainingTheEscapedBackSlash()
        {
            var source = "std::string testString = \"awesome \\\\ string\";";
            var tokenizer = new PlusPlusTokenizer(source);
            var tokens = tokenizer.Tokenize();

            Assert.AreEqual(4, tokens.Count);
            Assert.AreEqual("std::string", tokens[0].Content);
            Assert.AreEqual("testString", tokens[1].Content);
            Assert.AreEqual("=", tokens[2].Content);
            Assert.AreEqual("\"awesome \\\\ string\";", tokens[3].Content);
            Assert.IsTrue(tokens.All(t => t.TokenType == TokenType.Text));
            CheckLineNumberAndColumnIndex(tokens[0], 1, 1);
            CheckLineNumberAndColumnIndex(tokens[1], 1, 13);
            CheckLineNumberAndColumnIndex(tokens[2], 1, 24);
            CheckLineNumberAndColumnIndex(tokens[3], 1, 26);
        }

        [TestMethod]
        public void TokenizeStringWithEscapedNewLine_ReturnsFourTokensWithOneTokenForTheCompleteStringLiteralContainingTheEscapedNewLine()
        {
            var source = "std::string testString = \"awesome \\n string\";";
            var tokenizer = new PlusPlusTokenizer(source);
            var tokens = tokenizer.Tokenize();

            Assert.AreEqual(4, tokens.Count);
            Assert.AreEqual("std::string", tokens[0].Content);
            Assert.AreEqual("testString", tokens[1].Content);
            Assert.AreEqual("=", tokens[2].Content);
            Assert.AreEqual("\"awesome \\n string\";", tokens[3].Content);
            Assert.IsTrue(tokens.All(t => t.TokenType == TokenType.Text));
            CheckLineNumberAndColumnIndex(tokens[0], 1, 1);
            CheckLineNumberAndColumnIndex(tokens[1], 1, 13);
            CheckLineNumberAndColumnIndex(tokens[2], 1, 24);
            CheckLineNumberAndColumnIndex(tokens[3], 1, 26);
        }
    }
}
