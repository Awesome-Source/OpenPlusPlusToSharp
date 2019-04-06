namespace OpenPlusPlusToSharp.Parser
{
    /// <summary>
    /// The <see cref="NodeType"/> of a node is determined by the context of the token it is parsed from.
    /// It describes which part of the syntax is represented by the node.
    /// </summary>
    public enum NodeType
    {
        /// <summary>
        /// The top level node of a ParseTree is of type SourceFile.
        /// The content of this node is the file name.
        /// All other nodes are direct or indirect descendents of this node.
        /// </summary>
        SourceFile,
        /// <summary>
        /// Nodes with this type contain the file name of the include. They do not have descendents.
        /// </summary>
        IncludeDirective,
        ClassDeclaration,
        MethodDeclaration,
        ParameterList,
        AccessModifier,
        MethodBody,
    }
}
