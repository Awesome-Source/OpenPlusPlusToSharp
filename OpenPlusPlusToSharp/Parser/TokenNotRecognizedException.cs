using OpenPlusPlusToSharp.Tokenizer;
using System;

namespace OpenPlusPlusToSharp.Parser
{
    /// <summary>
    /// This exception should be thrown if a token is processed that was not expected.
    /// </summary>
    public class TokenNotRecognizedException : Exception
    {
        /// <summary>
        /// The token that couldn't be recognized.
        /// </summary>
        public Token UnrecognizedToken { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="TokenNotRecognizedException"/> with the provided token.
        /// </summary>
        /// <param name="unrecognizedToken">The token that couldn't be recognized.</param>
        public TokenNotRecognizedException(Token unrecognizedToken) : base(GenerateMessage(unrecognizedToken))
        {
            UnrecognizedToken = unrecognizedToken;
        }

        private static string GenerateMessage(Token unrecognizedToken)
        {
            return $"Unexpected token '{unrecognizedToken.Content}' in line {unrecognizedToken.LineNumber} column {unrecognizedToken.Column}";
        }
    }
}
