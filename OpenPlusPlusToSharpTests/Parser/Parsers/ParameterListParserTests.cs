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

            var parameterListNode = new ParseNode("", NodeType.ParameterList);
            parameterListNode.WithParameter(parameter => parameter.WithType("int"), "testInt");
            ParseNodeHierachyCompare.Compare(parameterListNode, parseTree.RootNode.Descendents.Single());
        }

        [TestMethod]
        public void ParseSingleSimpleParameterWithDefaultValueTest()
        {
            var source = "int testInt = 42";
            var parseTree = ParseSource(source);

            var parameterListNode = new ParseNode("", NodeType.ParameterList);
            parameterListNode.WithParameter(parameter => parameter.WithType("int"), "testInt", "42");
            ParseNodeHierachyCompare.Compare(parameterListNode, parseTree.RootNode.Descendents.Single());
        }

        [TestMethod]
        public void ParseThreeSimpleParametersTest()
        {
            var source = "int testInt, bool testBool, char testChar";
            var parseTree = ParseSource(source);

            var parameterListNode = new ParseNode("", NodeType.ParameterList);
            parameterListNode.WithParameter(parameter => parameter.WithType("int"), "testInt");
            parameterListNode.WithParameter(parameter => parameter.WithType("bool"), "testBool");
            parameterListNode.WithParameter(parameter => parameter.WithType("char"), "testChar");
            ParseNodeHierachyCompare.Compare(parameterListNode, parseTree.RootNode.Descendents.Single());
        }

        [TestMethod]
        public void ParseThreeSimpleParametersWithDefaultValuesTest()
        {
            var source = "int testInt = 42, bool testBool = true, char testChar";
            var parseTree = ParseSource(source);

            var parameterListNode = new ParseNode("", NodeType.ParameterList);
            parameterListNode.WithParameter(parameter => parameter.WithType("int"), "testInt", "42");
            parameterListNode.WithParameter(parameter => parameter.WithType("bool"), "testBool", "true");
            parameterListNode.WithParameter(parameter => parameter.WithType("char"), "testChar");
            ParseNodeHierachyCompare.Compare(parameterListNode, parseTree.RootNode.Descendents.Single());
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
