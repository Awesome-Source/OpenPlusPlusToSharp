using OpenPlusPlusToSharp.Tokenizer;
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
        private List<Token> Tokens;

        /// <summary>
        /// Creates a new <see cref="ParseContext"/> with the provided tokens.
        /// </summary>
        /// <param name="tokens">The tokens that should be parsed.</param>
        public ParseContext(List<Token> tokens)
        {
            Tokens = tokens;
            CurrentIndex = 0;
        }

        public ParseContext CreateSubContext(int startTokenOffset, int endTokenOffset)
        {
            var subContextTokenCount = endTokenOffset - startTokenOffset;
            var subContextTokens = Tokens
                .Skip(CurrentIndex + startTokenOffset)
                .Take(subContextTokenCount)
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

            return Tokens.ElementAtOrDefault(CurrentIndex + offsetFromCurrentIndex);
        }

        /// <summary>
        /// Returns true if all tokens have been processed. Otherwise false is returned.
        /// </summary>
        /// <returns>Returns true if all tokens have been processed. Otherwise false is returned.</returns>
        public bool AllTokensProcessed()
        {
            return CurrentIndex >= Tokens.Count;
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

        public bool VirtuallyAllTokensProcessed(int tokenOffset)
        {
            return CurrentIndex + tokenOffset == Tokens.Count - 1;
        }
    }
}
