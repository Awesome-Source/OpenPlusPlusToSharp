﻿using OpenPlusPlusToSharp.Tokenizer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenPlusPlusToSharp.Parser
{
    /// <summary>
    /// The parse context contains the tokens that are currently parsed.
    /// It also contains the index where the next parser should continue with the parse process.
    /// Furthermore the <see cref="ParseContext"/> contains a few utility methods to read the current and future tokens.
    /// </summary>
    public class ParseContext
    {
        /// <summary>
        /// The index of the current token that is processed.
        /// </summary>
        public int CurrentIndex { get; set; }
        private readonly List<Token> _tokens;

        /// <summary>
        /// Creates a new <see cref="ParseContext"/> with the provided tokens.
        /// </summary>
        /// <param name="tokens">The tokens that should be parsed.</param>
        public ParseContext(List<Token> tokens)
        {
            _tokens = tokens;
            CurrentIndex = 0;
        }

        /// <summary>
        /// Creates a new parse context from the current one containing only the tokens between the specified offsets.
        /// </summary>
        /// <param name="startTokenOffset"></param>
        /// <param name="endTokenOffset"></param>
        /// <returns></returns>
        public ParseContext CreateSubContext(int startTokenOffset, int endTokenOffset)
        {
            var subContextTokenCount = endTokenOffset - startTokenOffset;
            var subContextTokens = _tokens
                .Skip(CurrentIndex + startTokenOffset)
                .Take(subContextTokenCount)
                .ToList();

            return new ParseContext(subContextTokens);
        }

        /// <summary>
        /// Creates a new parse context from the current one containing only the tokens starting from the <paramref name="startTokenOffset"/> to the end.
        /// </summary>
        /// <param name="startTokenOffset"></param>
        /// <returns></returns>
        public ParseContext CreateSubContext(int startTokenOffset)
        {
            var subContextTokens = _tokens
                .Skip(CurrentIndex + startTokenOffset)
                .ToList();

            return new ParseContext(subContextTokens);
        }

        /// <summary>
        /// Returns a token that is at a higher index than the current index. 
        /// If there is no token at this index null is returned.
        /// </summary>
        /// <param name="offsetFromCurrentIndex">The offset from the current index. Must not be less than zero.</param>
        /// <returns>The token at the specified index or null if there is none.</returns>
        public Token GetFutureToken(int offsetFromCurrentIndex)
        {
            if(offsetFromCurrentIndex < 0)
            {
                throw new ArgumentException("Invalid offset. It is not allowed to access a previous token or the current token from this method.");
            }

            return _tokens.ElementAtOrDefault(CurrentIndex + offsetFromCurrentIndex);
        }

        /// <summary>
        /// Returns true if all tokens have been processed. Otherwise false is returned.
        /// </summary>
        /// <returns>Returns true if all tokens have been processed. Otherwise false is returned.</returns>
        public bool AllTokensProcessed()
        {
            return CurrentIndex >= _tokens.Count;
        }

        /// <summary>
        /// Checks if a future token is from the expected type and has the expected content.
        /// </summary>
        /// <param name="offsetFromCurrentIndex">The offset from the current index.</param>
        /// <param name="expectedTokenType">The expected token type.</param>
        /// <param name="expectedContent">The expected content</param>
        /// <returns>True if all conditions are satisfied. Otherwise false is returned.</returns>
        public bool CheckFutureToken(int offsetFromCurrentIndex, TokenType expectedTokenType, string expectedContent)
        {
            var token = GetFutureToken(offsetFromCurrentIndex);

            if(token == null)
            {
                return false;
            }

            if(token.TokenType != expectedTokenType || token.Content != expectedContent)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if a future token is from the expected type.
        /// </summary>
        /// <param name="offsetFromCurrentIndex">The offset from the current index.</param>
        /// <param name="expectedTokenType">The expected token type.</param>
        /// <returns>True if all conditions are satisfied. Otherwise false is returned.</returns>
        public bool CheckFutureToken(int offsetFromCurrentIndex, TokenType expectedTokenType)
        {
            var token = GetFutureToken(offsetFromCurrentIndex);

            if (token == null)
            {
                return false;
            }

            if (token.TokenType != expectedTokenType)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks whether all tokens would be processed with the provided offset or not.
        /// </summary>
        /// <param name="tokenOffset"></param>
        /// <returns></returns>
        public bool VirtuallyAllTokensProcessed(int tokenOffset)
        {
            return CurrentIndex + tokenOffset >= _tokens.Count - 1;
        }
    }
}
