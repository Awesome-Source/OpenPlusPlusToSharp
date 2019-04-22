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
    public class TypeParserTests
    {
        [TestMethod]
        public void ParseSimpleType()
        {
            var source = "int";
            var parseTree = ParseSource(source);

            var typeNameNode = new ParseNode("int", NodeType.TypeName);
            ParseNodeHierachyCompare.Compare(typeNameNode, parseTree.RootNode.Descendents.Single());
        }

        [TestMethod]
        public void ParsePointerType()
        {
            var source = "int*";
            var parseTree = ParseSource(source);

            var typeNameNode = new ParseNode("int", NodeType.TypeName).WithPointerType();
            ParseNodeHierachyCompare.Compare(typeNameNode, parseTree.RootNode.Descendents.Single());
        }

        [TestMethod]
        public void ParseReferenceType()
        {
            var source = "int&";
            var parseTree = ParseSource(source);

            var typeNameNode = new ParseNode("int", NodeType.TypeName).WithReferenceType();
            ParseNodeHierachyCompare.Compare(typeNameNode, parseTree.RootNode.Descendents.Single());
        }

        [TestMethod]
        public void ParsePointerPointerType()
        {
            var source = "int**";
            var parseTree = ParseSource(source);

            var typeNameNode = new ParseNode("int", NodeType.TypeName)
                .WithPointerType()
                .WithPointerType();
            ParseNodeHierachyCompare.Compare(typeNameNode, parseTree.RootNode.Descendents.Single());
        }

        [TestMethod]
        public void ParsePointerPointerPointerType()
        {
            var source = "int***";
            var parseTree = ParseSource(source);

            var typeNameNode = new ParseNode("int", NodeType.TypeName)
                .WithPointerType()
                .WithPointerType()
                .WithPointerType();
            ParseNodeHierachyCompare.Compare(typeNameNode, parseTree.RootNode.Descendents.Single());
        }

        [TestMethod]
        public void ParseTemplateType()
        {
            var source = "set<int>";
            var parseTree = ParseSource(source);

            var typeNameNode = new ParseNode("set", NodeType.TypeName)
                .WithTemplateType(tt => tt.WithType("int"));
            ParseNodeHierachyCompare.Compare(typeNameNode, parseTree.RootNode.Descendents.Single());
        }

        [TestMethod]
        public void ParseTemplatePointerType()
        {
            var source = "set<int*>";
            var parseTree = ParseSource(source);

            var typeNameNode = new ParseNode("set", NodeType.TypeName)
                .WithTemplateType(tt => tt.WithType("int").WithPointerType());
            ParseNodeHierachyCompare.Compare(typeNameNode, parseTree.RootNode.Descendents.Single());
        }

        [TestMethod]
        public void ParseNestedTemplateType()
        {
            var source = "vector<set<int>>";
            var parseTree = ParseSource(source);

            var typeNameNode = new ParseNode("vector", NodeType.TypeName)
                .WithTemplateType(vectorTemplateType =>
                {
                    vectorTemplateType.WithType("set").WithTemplateType(setTemplateType =>
                    {
                        setTemplateType.WithType("int");
                    });
                });
            ParseNodeHierachyCompare.Compare(typeNameNode, parseTree.RootNode.Descendents.Single());
        }

        [TestMethod]
        public void ParseTemplateWithMultipleTypeParametersType()
        {
            var source = "map<int, char>";
            var parseTree = ParseSource(source);

            var typeNameNode = new ParseNode("map", NodeType.TypeName)
                .WithTemplateType(tt =>
                {
                    tt.WithType("int");
                    tt.WithType("char");
                });
            ParseNodeHierachyCompare.Compare(typeNameNode, parseTree.RootNode.Descendents.Single());
        }

        private static ParseTree ParseSource(string source)
        {
            var tokenizer = new PlusPlusTokenizer(source);
            var topLevelParsers = new List<IParser>() { new TypeParser() };
            var parser = new PlusPlusParser("test.cpp", tokenizer.Tokenize(), topLevelParsers);
            var parseTree = parser.Parse();
            return parseTree;
        }
    }
}
