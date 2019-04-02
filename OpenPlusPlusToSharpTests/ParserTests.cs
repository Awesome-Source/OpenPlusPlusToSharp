using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenPlusPlusToSharp.Parser;
using OpenPlusPlusToSharpTests.ExampleSources;
using System.Linq;

namespace OpenPlusPlusToSharpTests
{
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void ParseProgramWithTwoIncludeDirectives_ReturnsParseTreeWithBothDirectives()
        {
            var source = SimplePrograms.MainWithVariableAssignment;

            var parser = new PlusPlusParser(source, "test.cpp");
            var parseTree = parser.Parse();

            var includeNodes = parseTree.RootNode.Descendents
                .Where(n => n.NodeType == NodeType.IncludeDirective)
                .ToList();

            Assert.AreEqual(2, includeNodes.Count);
            Assert.AreEqual("<iostream>", includeNodes[0].Descendents[0].Content);
            Assert.AreEqual("<string>", includeNodes[1].Descendents[0].Content);
        }
    }
}
