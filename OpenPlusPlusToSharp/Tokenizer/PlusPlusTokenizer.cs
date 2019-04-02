using System.Collections.Generic;
using System.Text;

namespace OpenPlusPlusToSharp.Tokenizer
{
    /// <summary>
    /// The <see cref="PlusPlusTokenizer"/> is used to process a C++ source file and return its tokens.
    /// </summary>
    public class PlusPlusTokenizer
    {
        private string _source;
        private int _currentIndex = 0;

        /// <summary>
        /// Creates a tokenizer for the provided source.
        /// </summary>
        /// <param name="source">The content of the C++ source file.</param>
        public PlusPlusTokenizer(string source)
        {
            _source = source;
        }

        /// <summary>
        /// Runs the tokenizer and returns all tokens in the order they are found.
        /// </summary>
        /// <returns>All tokens of the source file.</returns>
        public List<Token> Tokenize()
        {
            var tokens = new List<Token>();

            while (_currentIndex < _source.Length)
            {
                ConsumeWhiteSpace();
                var tokenText = ReadUntilWhiteSpace();
                var tokenType = DetermineTokenType(tokenText);
                tokens.Add(new Token(tokenText, tokenType));
            }

            return tokens;
        }

        private TokenType DetermineTokenType(string tokenText)
        {
            switch (tokenText)
            {
                case "{": return TokenType.BeginBlock;
                case "}": return TokenType.EndBlock;
                default: return TokenType.Text;
            }
        }

        private string ReadUntilWhiteSpace()
        {
            var buffer = new StringBuilder();
            var insideStringLiteral = false;

            var currentChar = GetCurrentChar();
            while (insideStringLiteral || !IsWhiteSpaceCharacter(currentChar))
            {
                if (currentChar == '"')
                {
                    insideStringLiteral = !insideStringLiteral;
                }

                buffer.Append(currentChar);
                if (!AdvanceOne())
                {
                    break;
                }

                currentChar = GetCurrentChar();
            }

            return buffer.ToString();
        }

        private void ConsumeWhiteSpace()
        {
            var currentChar = GetCurrentChar();
            while (IsWhiteSpaceCharacter(currentChar))
            {
                if (!AdvanceOne())
                {
                    break;
                }

                currentChar = GetCurrentChar();
            }
        }

        private bool IsWhiteSpaceCharacter(char character)
        {
            return character == ' ' || character == '\t' || character == '\r' || character == '\n';
        }

        private char GetCurrentChar()
        {
            return _source[_currentIndex];
        }

        private bool AdvanceOne()
        {
            _currentIndex++;

            return _currentIndex < _source.Length;
        }
    }
}
