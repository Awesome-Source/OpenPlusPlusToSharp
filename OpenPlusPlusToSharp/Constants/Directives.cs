namespace OpenPlusPlusToSharp.Constants
{
    /// <summary>
    /// Directives are all tokens that start with a '#'.
    /// This class contains constants for every directive supported by the parser.
    /// </summary>
    public class Directives
    {
        /// <summary>
        /// The C++ include directive.
        /// </summary>
        public const string IncludeDirective = "#include";

        /// <summary>
        /// The C++ if not defined directive.
        /// </summary>
        public const string IfNotDefinedDirective = "#ifndef";

        /// <summary>
        /// The C++ define directive.
        /// </summary>
        public const string DefineDirective = "#define";

        /// <summary>
        /// The C++ end if directive.
        /// </summary>
        public const string EndIfDirective = "#endif";

        /// <summary>
        /// The C++ pragma directive.
        /// </summary>
        public const string PragmaDirective = "#pragma";
    }
}
