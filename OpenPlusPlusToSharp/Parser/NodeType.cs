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

        /// <summary>
        /// Nodes with this type contain the name of the define. They do not have descendents.
        /// </summary>
        IfNotDefinedDirective,

        /// <summary>
        /// Nodes with this type don't contain content. They do not have descendents.
        /// </summary>
        EndIfDirective,

        /// <summary>
        /// Nodes with this type contain the name of the defined symbol. They do not have descendents.
        /// </summary>
        DefineDirective,

        /// <summary>
        /// Nodes with this symbol contain the content of the pragma. They do not have descendents.
        /// </summary>
        PragmaDirective,

        /// <summary>
        /// Nodes with this symbol contain the name of the namespace. They do not have descendents.
        /// </summary>
        UsingNamespaceStatement,

        /// <summary>
        /// Nodes with this symbol contain the class name of the forward declaration. They do not have descendents.
        /// </summary>
        ForwardDeclaration,

        ClassDeclaration,
        ClassInheritanceDeclaration,
        AccessModifier,
        MethodDeclarationList,
        ConstructorDeclaration,
        ParameterList,
        Paramter,
        TypeName,
        SymbolName,
        DefaultValue,
        DestructorDeclaration,
        MethodDeclaration,
        PointerType,
        ReferenceType,
        TemplateType
    }
}
