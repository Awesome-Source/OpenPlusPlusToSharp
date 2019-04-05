﻿using System;
using System.Collections.Generic;
using System.Linq;
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
        private int _lineNumber = 0;
        private int _columnIndex = 0;
        private static List<char> _specialChars = new List<char>
        {
            '{',
            '}',
            '(',
            ')',
            ';',
            ',',
        };

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

                ProcessTextToken(tokens);
                ProcessSpecialToken(tokens);
            }

            return tokens;
        }

        private void ProcessSpecialToken(List<Token> tokens)
        {
            var tokenText = ReadSpecialToken();

            if(string.IsNullOrEmpty(tokenText))
            {
                return;
            }

            AddToken(tokens, tokenText);
        }

        private string ReadSpecialToken()
        {
            if (IsEndOfFile())
            {
                return null;
            }

            var currentChar = GetCurrentChar();
            if (_specialChars.Any(c => c == currentChar))
            {
                AdvanceOne();
                return currentChar.ToString();
            }

            return null;
        }

        private void ProcessTextToken(List<Token> tokens)
        {
            var tokenText = ReadTextToken();

            if (string.IsNullOrEmpty(tokenText))
            {
                return;
            }

            AddToken(tokens, tokenText);
        }
        private void AddToken(List<Token> tokens, string tokenText)
        {
            var tokenType = DetermineTokenType(tokenText);
            var columnIndexOfTokenStart = _columnIndex - tokenText.Length;
            tokens.Add(new Token(tokenText, tokenType, _lineNumber + 1, columnIndexOfTokenStart + 1));
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
                case "(": return TokenType.OpeningBracket;
                case ")": return TokenType.ClosingBracket;
                case ";": return TokenType.Semicolon;
                case ",": return TokenType.Comma;
                default: return TokenType.Text;
            }
        }

        /// <summary>
        /// Reads the next token (until whitespace or semicolon). If the token is a string literal then the white spaces inside the literal are ignored.
        /// </summary>
        /// <returns>The extracted token.</returns>
        private string ReadTextToken()
        {
            var buffer = new StringBuilder();
            var readContext = new ReadContext();

            var currentChar = GetCurrentChar();
            while (readContext.IsInsideStringLiteral || !IsWhiteSpaceCharacter(currentChar))
            {
                HandleStringLiteralIfNecessary(readContext, currentChar);

                if(!readContext.IsInsideStringLiteral && _specialChars.Any(c => c == currentChar))
                {
                    break;
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
            var isWhiteSpaceChar = IsWhiteSpaceCharacter(currentChar);

            while (isWhiteSpaceChar)
            {
                HandleNewLine(currentChar);

                if (!AdvanceOne())
                {
                    break;
                }

                currentChar = GetCurrentChar();
                isWhiteSpaceChar = IsWhiteSpaceCharacter(currentChar);
            }
        }

        private void HandleNewLine(char currentChar)
        {
            if (currentChar == '\n')
            {
                _lineNumber++;
                _columnIndex = -1; //the new line is not a visible char -> it should not be counted by the next advance statement
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
            _columnIndex++;

            return !IsEndOfFile();
        }
    }
}
