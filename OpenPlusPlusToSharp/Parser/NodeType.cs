namespace OpenPlusPlusToSharp.Parser
{
    /// <summary>
    /// The <see cref="NodeType"/> of a node is determined by the context of the token it is parsed from.
    /// It describes which part of the syntax is represented by the node.
    /// </summary>
    public enum NodeType
    {
        IncludeDirective,
        IncludeContent,
        ClassDeclaration,
        MethodDeclaration,
        ParameterList,
        AccessModifier,
        MethodBody,
        SourceFile
    }
}
