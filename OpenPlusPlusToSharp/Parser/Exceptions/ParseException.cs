using System;

namespace OpenPlusPlusToSharp.Parser.Exceptions
{
    /// <summary>
    /// This exception should be thrown if a parser encounters a general error.
    /// </summary>
    public class ParseException : Exception
    {
        /// <summary>
        /// Creates a new <see cref="ParseException"/> with the provided message.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public ParseException(string message) : base(message)
        {

        }
    }
}
