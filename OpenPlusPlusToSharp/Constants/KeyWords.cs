namespace OpenPlusPlusToSharp.Constants
{
    /// <summary>
    /// This class contains constants for every C++ keyword supported by the parser.
    /// </summary>
    public static class KeyWords
    {
        /// <summary>
        /// The C++ using keyword.
        /// </summary>
        public const string Using = "using";

        /// <summary>
        /// The C++ namespace keyword.
        /// </summary>
        public const string Namespace = "namespace";

        /// <summary>
        /// The C++ class keyword.
        /// </summary>
        public const string Class = "class";

        /// <summary>
        /// The C++ public keyword.
        /// </summary>
        public const string Public = "public";

        /// <summary>
        /// The C++ private keyword.
        /// </summary>
        public const string Private = "private";

        /// <summary>
        /// The C++ protected keyword.
        /// </summary>
        public const string Protected = "protected";

        /// <summary>
        /// Returns true if the input is an access modifier keyword. Otherwise false is returned.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsAccessModifier(string input)
        {
            return input == Public || input == Protected || input == Private;
        }
    }
}
