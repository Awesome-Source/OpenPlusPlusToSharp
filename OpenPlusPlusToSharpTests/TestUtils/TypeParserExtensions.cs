using System;
using OpenPlusPlusToSharp.Parser;

namespace OpenPlusPlusToSharpTests.TestUtils
{
    public static class TypeParserExtensions
    {
        public static ParseNode WithPointerType(this ParseNode sourceNode)
        {
            var pointerNode = new ParseNode("*", NodeType.PointerType);
            sourceNode.Descendents.Add(pointerNode);

            return sourceNode;
        }

        public static ParseNode WithReferenceType(this ParseNode sourceNode)
        {
            var referenceNode = new ParseNode("&", NodeType.ReferenceType);
            sourceNode.Descendents.Add(referenceNode);

            return sourceNode;
        }

        public static ParseNode WithTemplateType(this ParseNode sourceNode, Action<ParseNode> templateTypeAction)
        {
            var templateTypeNode = new ParseNode("", NodeType.TemplateType);
            sourceNode.Descendents.Add(templateTypeNode);
            templateTypeAction(templateTypeNode);

            return sourceNode;
        }

        public static ParseNode WithType(this ParseNode sourceNode, string typeName)
        {
            var typeNameNode = new ParseNode(typeName, NodeType.TypeName);
            sourceNode.Descendents.Add(typeNameNode);

            return typeNameNode;
        }
    }
}
