using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenPlusPlusToSharp.Parser;
using OpenPlusPlusToSharp.Parser.Parsers;
using OpenPlusPlusToSharp.Tokenizer;
using OpenPlusPlusToSharpTests.TestUtils;

namespace OpenPlusPlusToSharpTests.Parser.Parsers
{
    [TestClass]
    public class ParameterListParserTests
    {
        [TestMethod]
        public void ParseSingleSimpleParameterTest()
        {
            var source = "int testInt";
            var parseTree = ParseSource(source);

            var parameterList = new ParseNodeAssert(parseTree.RootNode.Descendents.Single());
            parameterList
                .ExpectedDescendentCount(1)
                .ExpectedNodeType(NodeType.ParameterList)
                .ExpectedDescendent(0, NodeType.Paramter, 2)
                .CheckDescendent(0, parameter =>
                {
                    parameter
                        .ExpectedDescendent(0, NodeType.TypeName, "int", 0)
                        .ExpectedDescendent(1, NodeType.SymbolName, "testInt", 0);

                });
        }

        [TestMethod]
        public void ParseSingleSimpleParameterWithDefaultValueTest()
        {
            var source = "int testInt = 42";
            var parseTree = ParseSource(source);

            var parameterList = new ParseNodeAssert(parseTree.RootNode.Descendents.Single());
            parameterList
                .ExpectedDescendentCount(1)
                .ExpectedNodeType(NodeType.ParameterList)
                .ExpectedDescendent(0, NodeType.Paramter, 3)
                .CheckDescendent(0, parameter =>
                {
                    parameter
                        .ExpectedDescendent(0, NodeType.TypeName, "int", 0)
                        .ExpectedDescendent(1, NodeType.SymbolName, "testInt", 0)
                        .ExpectedDescendent(2, NodeType.DefaultValue, "42", 0);

                });
        }

        [TestMethod]
        public void ParseThreeSimpleParametersTest()
        {
            var source = "int testInt, bool testBool, char testChar";
            var parseTree = ParseSource(source);

            var parameterList = new ParseNodeAssert(parseTree.RootNode.Descendents.Single());
            parameterList
                .ExpectedDescendentCount(3)
                .ExpectedNodeType(NodeType.ParameterList)
                .ExpectedDescendent(0, NodeType.Paramter, 2)
                .ExpectedDescendent(1, NodeType.Paramter, 2)
                .ExpectedDescendent(2, NodeType.Paramter, 2)
                .CheckDescendent(0, parameter =>
                {
                    parameter
                        .ExpectedDescendent(0, NodeType.TypeName, "int", 0)
                        .ExpectedDescendent(1, NodeType.SymbolName, "testInt", 0);

                })
                .CheckDescendent(1, parameter =>
                {
                    parameter
                        .ExpectedDescendent(0, NodeType.TypeName, "bool", 0)
                        .ExpectedDescendent(1, NodeType.SymbolName, "testBool", 0);

                })
                .CheckDescendent(2, parameter =>
                {
                    parameter
                        .ExpectedDescendent(0, NodeType.TypeName, "char", 0)
                        .ExpectedDescendent(1, NodeType.SymbolName, "testChar", 0);

                });
        }

        [TestMethod]
        public void ParseThreeSimpleParametersWithDefaultValuesTest()
        {
            var source = "int testInt = 42, bool testBool = true, char testChar";
            var parseTree = ParseSource(source);

            var parameterList = new ParseNodeAssert(parseTree.RootNode.Descendents.Single());
            parameterList
                .ExpectedDescendentCount(3)
                .ExpectedNodeType(NodeType.ParameterList)
                .ExpectedDescendent(0, NodeType.Paramter, 3)
                .ExpectedDescendent(1, NodeType.Paramter, 3)
                .ExpectedDescendent(2, NodeType.Paramter, 2)
                .CheckDescendent(0, parameter =>
                {
                    parameter
                        .ExpectedDescendent(0, NodeType.TypeName, "int", 0)
                        .ExpectedDescendent(1, NodeType.SymbolName, "testInt", 0)
                        .ExpectedDescendent(2, NodeType.DefaultValue, "42", 0);

                })
                .CheckDescendent(1, parameter =>
                {
                    parameter
                        .ExpectedDescendent(0, NodeType.TypeName, "bool", 0)
                        .ExpectedDescendent(1, NodeType.SymbolName, "testBool", 0)
                        .ExpectedDescendent(2, NodeType.DefaultValue, "true", 0);

                })
                .CheckDescendent(2, parameter =>
                {
                    parameter
                        .ExpectedDescendent(0, NodeType.TypeName, "char", 0)
                        .ExpectedDescendent(1, NodeType.SymbolName, "testChar", 0);

                });
        }

        private static ParseTree ParseSource(string source)
        {
            var tokenizer = new PlusPlusTokenizer(source);
            var topLevelParsers = new List<IParser>() { new ParameterListParser(new TypeParser()) };
            var parser = new PlusPlusParser("test.cpp", tokenizer.Tokenize(), topLevelParsers);
            var parseTree = parser.Parse();
            return parseTree;
        }
    }
}
