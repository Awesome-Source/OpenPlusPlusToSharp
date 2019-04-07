using System.Collections.Generic;

namespace OpenPlusPlusToSharp.Parser
{
    /// <summary>
    /// A <see cref="ParseNode"/> is a node of a <see cref="ParseTree"/>.
    /// It represents one specific part of a source file.
    /// </summary>
    public class ParseNode
    {
        /// <summary>
        /// If the node has descendents then they add more context to the node itself.
        /// </summary>
        public List<ParseNode> Descendents { get; } = new List<ParseNode>();

        /// <summary>
        /// The node type describes which part of the source file is represented by the node.
        /// </summary>
        public NodeType NodeType { get; }

        /// <summary>
        /// The content holds the information that is described by this node.
        /// </summary>
        public string Content { get; }

        /// <summary>
        /// Creates a <see cref="ParseNode"/> with the provided content and node type.
        /// </summary>
        /// <param name="content">The content of the node.</param>
        /// <param name="nodeType">The type of the node.</param>
        public ParseNode(string content, NodeType nodeType)
        {
            Content = content;
            NodeType = nodeType;
        }

        /// <summary>
        /// Returns a string representation of the node.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Type: {NodeType}, Content: {Content}";
        }
    }
}
