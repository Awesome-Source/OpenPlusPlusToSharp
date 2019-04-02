namespace OpenPlusPlusToSharp.Parser
{
    /// <summary>
    /// The <see cref="ParseTree"/> contains a hierachy of <see cref="ParseNode"/>s.
    /// This hierachy describes the file structure.
    /// </summary>
    public class ParseTree
    {
        /// <summary>
        /// The top level node. It always contains the filename.
        /// </summary>
        public ParseNode RootNode { get; }

        /// <summary>
        /// Creates a new <see cref="ParseTree"/>.
        /// The root node is also created containing the provided <paramref name="fileName"/>.
        /// </summary>
        /// <param name="fileName">The filename that is used as content in the root node.</param>
        public ParseTree(string fileName)
        {
            RootNode = new ParseNode(fileName, NodeType.SourceFile);
        }
    }
}
