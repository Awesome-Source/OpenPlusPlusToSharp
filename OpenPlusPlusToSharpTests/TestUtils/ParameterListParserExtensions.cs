using System;
using OpenPlusPlusToSharp.Parser;

namespace OpenPlusPlusToSharpTests.TestUtils
{
    public static class ParameterListParserExtensions
    {
        public static ParseNode WithParameter(this ParseNode source, Action<ParseNode> typeSpecificationAction, string parameterName)
        {
            var parameterNode = new ParseNode("", NodeType.Paramter);
            source.Descendents.Add(parameterNode);
            typeSpecificationAction(parameterNode);
            var parameterNameNode = new ParseNode(parameterName, NodeType.SymbolName);
            parameterNode.Descendents.Add(parameterNameNode);

            return source;
        }

        public static ParseNode WithParameter(this ParseNode source, Action<ParseNode> typeSpecificationAction, string parameterName, string defaultValue)
        {
            var parameterNode = new ParseNode("", NodeType.Paramter);
            source.Descendents.Add(parameterNode);
            typeSpecificationAction(parameterNode);
            var parameterNameNode = new ParseNode(parameterName, NodeType.SymbolName);
            parameterNode.Descendents.Add(parameterNameNode);
            var defaultValueNode = new ParseNode(defaultValue, NodeType.DefaultValue);
            parameterNode.Descendents.Add(defaultValueNode);

            return source;
        }
    }
}
