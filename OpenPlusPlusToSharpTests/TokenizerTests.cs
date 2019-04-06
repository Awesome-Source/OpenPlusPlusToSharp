using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenPlusPlusToSharp.Tokenizer;

namespace OpenPlusPlusToSharpTests
{
    [TestClass]
    public class TokenizerTests
    {
        [TestMethod]
        public void TokenizeInclude_ReturnsFourTokens()
        {
            var source = "#include <string>";
            var tokenizer = new PlusPlusTokenizer(source);
            var tokens = tokenizer.Tokenize();

            Assert.AreEqual(4, tokens.Count);
            CheckToken(tokens[0], "#include", TokenType.Text, 1, 1);
            CheckToken(tokens[1], "<", TokenType.SpecialCharacter, 1, 10);
            CheckToken(tokens[2], "string", TokenType.Text, 1, 11);
            CheckToken(tokens[3], ">", TokenType.SpecialCharacter, 1, 17);
        }

        private void CheckToken(Token token, string expectedContent, TokenType expectedType, int expectedLineNumber, int expectedColumnIndex)
        {
            Assert.AreEqual(expectedContent, token.Content);
            Assert.AreEqual(expectedType, token.TokenType);
            Assert.AreEqual(expectedLineNumber, token.LineNumber);
            Assert.AreEqual(expectedColumnIndex, token.Column);
        }

        [TestMethod]
        public void TokenizeIncludeWithSurroundingWhiteSpaces_ReturnsFourTokens()
        {
            var source = " #include <string> ";
            var tokenizer = new PlusPlusTokenizer(source);
            var tokens = tokenizer.Tokenize();

            Assert.AreEqual(4, tokens.Count);
            CheckToken(tokens[0], "#include", TokenType.Text, 1, 2);
            CheckToken(tokens[1], "<", TokenType.SpecialCharacter, 1, 11);
            CheckToken(tokens[2], "string", TokenType.Text, 1, 12);
            CheckToken(tokens[3], ">", TokenType.SpecialCharacter, 1, 18);
        }

        [TestMethod]
        public void TokenizeIntVariableAssignment_ReturnsFiveTokens()
        {
            var source = "int testInt = 42;";
            var tokenizer = new PlusPlusTokenizer(source);
            var tokens = tokenizer.Tokenize();

            Assert.AreEqual(5, tokens.Count);
            CheckToken(tokens[0], "int", TokenType.Text, 1, 1);
            CheckToken(tokens[1], "testInt", TokenType.Text, 1, 5);
            CheckToken(tokens[2], "=", TokenType.SpecialCharacter, 1, 13);
            CheckToken(tokens[3], "42", TokenType.Text, 1, 15);
            CheckToken(tokens[4], ";", TokenType.SpecialCharacter, 1, 17);
        }

        [TestMethod]
        public void TokenizeTwoIntVariableAssignments_OneVariableDeclarationIsDirectlyBehindTheOther_ReturnsTenTokens()
        {
            var source = "int testInt = 42;int testInt2 = 1337;";
            var tokenizer = new PlusPlusTokenizer(source);
            var tokens = tokenizer.Tokenize();

            Assert.AreEqual(10, tokens.Count);
            CheckToken(tokens[0], "int", TokenType.Text, 1, 1);
            CheckToken(tokens[1], "testInt", TokenType.Text, 1, 5);
            CheckToken(tokens[2], "=", TokenType.SpecialCharacter, 1, 13);
            CheckToken(tokens[3], "42", TokenType.Text, 1, 15);
            CheckToken(tokens[4], ";", TokenType.SpecialCharacter, 1, 17);
            CheckToken(tokens[5], "int", TokenType.Text, 1, 18);
            CheckToken(tokens[6], "testInt2", TokenType.Text, 1, 22);
            CheckToken(tokens[7], "=", TokenType.SpecialCharacter, 1, 31);
            CheckToken(tokens[8], "1337", TokenType.Text, 1, 33);
            CheckToken(tokens[9], ";", TokenType.SpecialCharacter, 1, 37);
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
            CheckToken(tokens[2], "=", TokenType.SpecialCharacter, 1, 13);
            CheckToken(tokens[3], "42", TokenType.Text, 1, 15);
            CheckToken(tokens[4], ";", TokenType.SpecialCharacter, 1, 17);
            CheckToken(tokens[5], "int", TokenType.Text, 2, 1);
            CheckToken(tokens[6], "testInt2", TokenType.Text, 2, 5);
            CheckToken(tokens[7], "=", TokenType.SpecialCharacter, 2, 14);
            CheckToken(tokens[8], "1337", TokenType.Text, 2, 16);
            CheckToken(tokens[9], ";", TokenType.SpecialCharacter, 2, 20);
        }

        [TestMethod]
        public void TokenizeString_ReturnsEightTokensWithOneTokenForTheCompleteStringLiteral()
        {
            var source = "std::string testString = \"awesome string\";";
            var tokenizer = new PlusPlusTokenizer(source);
            var tokens = tokenizer.Tokenize();

            Assert.AreEqual(8, tokens.Count);
            CheckToken(tokens[0], "std", TokenType.Text, 1, 1);
            CheckToken(tokens[1], ":", TokenType.SpecialCharacter, 1, 4);
            CheckToken(tokens[2], ":", TokenType.SpecialCharacter, 1, 5);
            CheckToken(tokens[3], "string", TokenType.Text, 1, 6);
            CheckToken(tokens[4], "testString", TokenType.Text, 1, 13);
            CheckToken(tokens[5], "=", TokenType.SpecialCharacter, 1, 24);
            CheckToken(tokens[6], "\"awesome string\"", TokenType.StringLiteral, 1, 26);
            CheckToken(tokens[7], ";", TokenType.SpecialCharacter, 1, 42);
        }

        [TestMethod]
        public void TokenizeStringWithEscapedQuote_ReturnsEightTokensWithOneTokenForTheCompleteStringLiteralContainingTheEscapedQuote()
        {
            var source = "std::string testString = \"awesome \\\" string\";";
            var tokenizer = new PlusPlusTokenizer(source);
            var tokens = tokenizer.Tokenize();

            Assert.AreEqual(8, tokens.Count);
            CheckToken(tokens[0], "std", TokenType.Text, 1, 1);
            CheckToken(tokens[1], ":", TokenType.SpecialCharacter, 1, 4);
            CheckToken(tokens[2], ":", TokenType.SpecialCharacter, 1, 5);
            CheckToken(tokens[3], "string", TokenType.Text, 1, 6);
            CheckToken(tokens[4], "testString", TokenType.Text, 1, 13);
            CheckToken(tokens[5], "=", TokenType.SpecialCharacter, 1, 24);
            CheckToken(tokens[6], "\"awesome \\\" string\"", TokenType.StringLiteral, 1, 26);
            CheckToken(tokens[7], ";", TokenType.SpecialCharacter, 1, 45);
        }

        [TestMethod]
        public void TokenizeStringWithEscapedBackSlash_ReturnsEightTokensWithOneTokenForTheCompleteStringLiteralContainingTheEscapedBackSlash()
        {
            var source = "std::string testString = \"awesome \\\\ string\";";
            var tokenizer = new PlusPlusTokenizer(source);
            var tokens = tokenizer.Tokenize();

            Assert.AreEqual(8, tokens.Count);
            CheckToken(tokens[0], "std", TokenType.Text, 1, 1);
            CheckToken(tokens[1], ":", TokenType.SpecialCharacter, 1, 4);
            CheckToken(tokens[2], ":", TokenType.SpecialCharacter, 1, 5);
            CheckToken(tokens[3], "string", TokenType.Text, 1, 6);
            CheckToken(tokens[4], "testString", TokenType.Text, 1, 13);
            CheckToken(tokens[5], "=", TokenType.SpecialCharacter, 1, 24);
            CheckToken(tokens[6], "\"awesome \\\\ string\"", TokenType.StringLiteral, 1, 26);
            CheckToken(tokens[7], ";", TokenType.SpecialCharacter, 1, 45);
        }

        [TestMethod]
        public void TokenizeStringWithEscapedNewLine_ReturnsEightTokensWithOneTokenForTheCompleteStringLiteralContainingTheEscapedNewLine()
        {
            var source = "std::string testString = \"awesome \\n string\";";
            var tokenizer = new PlusPlusTokenizer(source);
            var tokens = tokenizer.Tokenize();

            Assert.AreEqual(8, tokens.Count);
            CheckToken(tokens[0], "std", TokenType.Text, 1, 1);
            CheckToken(tokens[1], ":", TokenType.SpecialCharacter, 1, 4);
            CheckToken(tokens[2], ":", TokenType.SpecialCharacter, 1, 5);
            CheckToken(tokens[3], "string", TokenType.Text, 1, 6);
            CheckToken(tokens[4], "testString", TokenType.Text, 1, 13);
            CheckToken(tokens[5], "=", TokenType.SpecialCharacter, 1, 24);
            CheckToken(tokens[6], "\"awesome \\n string\"", TokenType.StringLiteral, 1, 26);
            CheckToken(tokens[7], ";", TokenType.SpecialCharacter, 1, 45);
        }

        [TestMethod]
        public void TokenizeForLoop_Return21TokensWithTokenTypesForBracketsAndBlocksAndSemicolons()
        {
            var source = "for(int i = 0; i < 42; i++){d++;}";
            var tokenizer = new PlusPlusTokenizer(source);
            var tokens = tokenizer.Tokenize();

            Assert.AreEqual(21, tokens.Count);
            CheckToken(tokens[0], "for", TokenType.Text, 1, 1);
            CheckToken(tokens[1], "(", TokenType.SpecialCharacter, 1, 4);
            CheckToken(tokens[2], "int", TokenType.Text, 1, 5);
            CheckToken(tokens[3], "i", TokenType.Text, 1, 9);
            CheckToken(tokens[4], "=", TokenType.SpecialCharacter, 1, 11);
            CheckToken(tokens[5], "0", TokenType.Text, 1, 13);
            CheckToken(tokens[6], ";", TokenType.SpecialCharacter, 1, 14);
            CheckToken(tokens[7], "i", TokenType.Text, 1, 16);
            CheckToken(tokens[8], "<", TokenType.SpecialCharacter, 1, 18);
            CheckToken(tokens[9], "42", TokenType.Text, 1, 20);
            CheckToken(tokens[10], ";", TokenType.SpecialCharacter, 1, 22);
            CheckToken(tokens[11], "i", TokenType.Text, 1, 24);
            CheckToken(tokens[12], "+", TokenType.SpecialCharacter, 1, 25);
            CheckToken(tokens[13], "+", TokenType.SpecialCharacter, 1, 26);
            CheckToken(tokens[14], ")", TokenType.SpecialCharacter, 1, 27);
            CheckToken(tokens[15], "{", TokenType.SpecialCharacter, 1, 28);
            CheckToken(tokens[16], "d", TokenType.Text, 1, 29);
            CheckToken(tokens[17], "+", TokenType.SpecialCharacter, 1, 30);
            CheckToken(tokens[18], "+", TokenType.SpecialCharacter, 1, 31);
            CheckToken(tokens[19], ";", TokenType.SpecialCharacter, 1, 32);
            CheckToken(tokens[20], "}", TokenType.SpecialCharacter, 1, 33);
        }
    }
}
