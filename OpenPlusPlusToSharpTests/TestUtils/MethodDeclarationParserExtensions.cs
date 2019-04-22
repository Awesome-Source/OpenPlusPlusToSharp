using OpenPlusPlusToSharp.Parser;

namespace OpenPlusPlusToSharpTests.TestUtils
{
    public static class MethodDeclarationParserExtensions
    {
        public static ParseNode WithParameterList(this ParseNode source)
        {
            var parameterList = new ParseNode("", NodeType.ParameterList);
            source.Descendents.Add(parameterList);

            return parameterList;
        }
    }
}
