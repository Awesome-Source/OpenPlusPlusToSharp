using System.Collections.Generic;
using OpenPlusPlusToSharp.Parser.Exceptions;
using OpenPlusPlusToSharp.Tokenizer;

namespace OpenPlusPlusToSharp.Parser
{
    /// <summary>
    /// The <see cref="PlusPlusParser"/> is used to turn a C++ source file into a <see cref="ParseTree"/>.
    /// </summary>
    public class PlusPlusParser
    {        
        private readonly ParseTree _tree;
        private readonly List<IParser> _parsers;
        private readonly ParseContext _parseContext;

        /// <summary>
        /// Creates a <see cref="PlusPlusParser"/> that parses the provided content.
        /// </summary>
        /// <param name="sourceFileName">The filename of the sourc file.</param>
        /// <param name="tokens">The tokens that are read from a source file.</param>
        /// <param name="topLevelParsers">The top level parsers that are used by this parser.</param>
        public PlusPlusParser(string sourceFileName, List<Token> tokens, List<IParser> topLevelParsers)
        {
            _tree = new ParseTree(sourceFileName);
            _parseContext = new ParseContext(tokens);
            _parsers = topLevelParsers;
        }

        /// <summary>
        /// Parses the source file and returns a <see cref="ParseTree"/> of it.
        /// </summary>
        /// <returns>The <see cref="ParseTree"/> representing the file content.</returns>
        public ParseTree Parse()
        {
            while(!_parseContext.AllTokensProcessed())
            {
                ParseTopLevel();
            }

            return _tree;
        }

        private void ParseTopLevel()
        {
            ParserRunner.RunAllParsers(_parsers, _parseContext, _tree.RootNode);
        }
    }
}
