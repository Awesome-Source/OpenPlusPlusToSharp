using OpenPlusPlusToSharp.Parser;

namespace OpenPlusPlusToSharpTests.TestUtils
{
    public static class ParseNodeAssertExtensions
    {
        public static ParseNodeAssert HasParameterDescendent(this ParseNodeAssert nodeAssert, int descendentIndex, string typeName, string symbolName, string defaultValue = null)
        {
            var numberOfParameters = defaultValue == null ? 2 : 3;

            return nodeAssert.ExpectedDescendent(descendentIndex, NodeType.Paramter, numberOfParameters)
                .CheckDescendent(descendentIndex, parameter =>
                {
                    parameter
                        .ExpectedDescendent(0, NodeType.TypeName, typeName, 0)
                        .ExpectedDescendent(1, NodeType.SymbolName, symbolName, 0);

                    if (defaultValue != null)
                    {
                        parameter.ExpectedDescendent(2, NodeType.DefaultValue, defaultValue, 0);
                    }
                });
        }
    }
}
