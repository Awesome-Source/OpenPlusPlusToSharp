using System.Collections.Generic;
using System.Linq;
using OpenPlusPlusToSharp.Constants;
using OpenPlusPlusToSharp.Tokenizer;

namespace OpenPlusPlusToSharp.Parser
{
    /// <summary>
    /// The <see cref="PlusPlusParser"/> is used to turn a C++ source file into a <see cref="ParseTree"/>.
    /// </summary>
    public class PlusPlusParser
    {
        private string _sourceFileContent;
        private ParseTree _tree;
        private List<Token> _tokens;
        private int _currentIndex = -1;

        /// <summary>
        /// Creates a <see cref="PlusPlusParser"/> that parses the provided content.
        /// </summary>
        /// <param name="sourceFileContent">The file content that should be parsed.</param>
        /// <param name="sourceFileName">The filename of the sourc file.</param>
        public PlusPlusParser(string sourceFileContent, string sourceFileName)
        {
            _sourceFileContent = sourceFileContent;
            _tree = new ParseTree(sourceFileName);
            var tokenizer = new PlusPlusTokenizer(_sourceFileContent);
            _tokens = tokenizer.Tokenize();
        }

        /// <summary>
        /// Parses the source file and returns a <see cref="ParseTree"/> of it.
        /// </summary>
        /// <returns>The <see cref="ParseTree"/> representing the file content.</returns>
        public ParseTree Parse()
        {
            while(_currentIndex < _tokens.Count)
            {
                var topLevelToken = GetNextToken();

                if (topLevelToken == null)
                {
                    break;
                }

                ParseTopLevel(topLevelToken);
            }

            return _tree;
        }

        private void ParseTopLevel(Token topLevelToken)
        {
            if(topLevelToken.Content == Directives.IncludeDirective)
            {
                ParseInclude(topLevelToken);
                return;
            }
        }

        private void ParseInclude(Token topLevelToken)
        {
            var includeNode = new ParseNode(topLevelToken.Content, NodeType.IncludeDirective);
            var includeContentToken = GetNextToken();
            var includeContentNode = new ParseNode(includeContentToken.Content, NodeType.IncludeContent);
            includeNode.Descendents.Add(includeContentNode);
            _tree.RootNode.Descendents.Add(includeNode);
        }

        private Token GetNextToken()
        {
            return _tokens.ElementAtOrDefault(++_currentIndex);
        }
    }
}
