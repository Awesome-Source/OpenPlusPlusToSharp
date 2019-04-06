namespace OpenPlusPlusToSharp
{
    /// <summary>
    /// Every parameter that can be set by the command line interface is represented in this class.
    /// </summary>
    public class CommandLineConfiguration
    {
        /// <summary>
        /// If the test parse file is specified then the file gets parsed.
        /// The result of the parse process gets printed.
        /// </summary>
        public string TestParseFile { get; set; }
    }
}
