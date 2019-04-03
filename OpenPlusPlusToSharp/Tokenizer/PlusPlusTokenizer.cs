﻿using System.Collections.Generic;
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

            while (!IsEndOfFile())
            {
                ConsumeWhiteSpace();

                if (IsEndOfFile())
                {
                    break;
                }

                var tokenText = ReadToken();
                var tokenType = DetermineTokenType(tokenText);
                tokens.Add(new Token(tokenText, tokenType));
            }

            return tokens;
        }

        private bool IsEndOfFile()
        {
            return _currentIndex >= _source.Length;
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

        /// <summary>
        /// Reads the next token (until whitespace or semicolon). If the token is a string literal then the white spaces inside the literal are ignored.
        /// </summary>
        /// <returns>The extracted token.</returns>
        private string ReadToken()
        {
            var buffer = new StringBuilder();
            var readContext = new ReadContext();

            var currentChar = GetCurrentChar();
            while (readContext.IsInsideStringLiteral || !IsWhiteSpaceCharacter(currentChar))
            {
                HandleStringLiteralIfNecessary(readContext, currentChar);

                buffer.Append(currentChar);
                if (!AdvanceOne() || currentChar == ';')
                {
                    break;
                }

                currentChar = GetCurrentChar();
            }

            return buffer.ToString();
        }

        private static void HandleStringLiteralIfNecessary(ReadContext readContext, char currentChar)
        {
            if (!readContext.IsEscaped)
            {
                if (currentChar == '"')
                {
                    readContext.IsInsideStringLiteral = !readContext.IsInsideStringLiteral;
                }

                if (currentChar == '\\')
                {
                    readContext.IsEscaped = true;
                }
            }
            else
            {
                readContext.IsEscaped = false;
            }
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
