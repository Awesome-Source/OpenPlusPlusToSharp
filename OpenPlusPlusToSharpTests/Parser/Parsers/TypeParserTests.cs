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

            Assert.AreEqual(1, parseTree.RootNode.Descendents.Count);
            Assert.AreEqual(NodeType.TypeName, parseTree.RootNode.Descendents[0].NodeType);
            Assert.AreEqual("int", parseTree.RootNode.Descendents[0].Content);
            Assert.AreEqual(0, parseTree.RootNode.Descendents[0].Descendents.Count);
        }

        [TestMethod]
        public void ParsePointerType()
        {
            var source = "int*";
            var parseTree = ParseSource(source);

            Assert.AreEqual(1, parseTree.RootNode.Descendents.Count);
            Assert.AreEqual(NodeType.TypeName, parseTree.RootNode.Descendents[0].NodeType);
            Assert.AreEqual("int", parseTree.RootNode.Descendents[0].Content);
            Assert.AreEqual(1, parseTree.RootNode.Descendents[0].Descendents.Count);
            Assert.AreEqual(NodeType.PointerType, parseTree.RootNode.Descendents[0].Descendents[0].NodeType);
            Assert.AreEqual("*", parseTree.RootNode.Descendents[0].Descendents[0].Content);
            Assert.AreEqual(0, parseTree.RootNode.Descendents[0].Descendents[0].Descendents.Count);
        }

        [TestMethod]
        public void ParseReferenceType()
        {
            var source = "int&";
            var parseTree = ParseSource(source);

            Assert.AreEqual(1, parseTree.RootNode.Descendents.Count);
            Assert.AreEqual(NodeType.TypeName, parseTree.RootNode.Descendents[0].NodeType);
            Assert.AreEqual("int", parseTree.RootNode.Descendents[0].Content);
            Assert.AreEqual(1, parseTree.RootNode.Descendents[0].Descendents.Count);
            Assert.AreEqual(NodeType.ReferenceType, parseTree.RootNode.Descendents[0].Descendents[0].NodeType);
            Assert.AreEqual("&", parseTree.RootNode.Descendents[0].Descendents[0].Content);
            Assert.AreEqual(0, parseTree.RootNode.Descendents[0].Descendents[0].Descendents.Count);
        }

        [TestMethod]
        public void ParsePointerPointerType()
        {
            var source = "int**";
            var parseTree = ParseSource(source);

            Assert.AreEqual(1, parseTree.RootNode.Descendents.Count);
            Assert.AreEqual(NodeType.TypeName, parseTree.RootNode.Descendents[0].NodeType);
            Assert.AreEqual("int", parseTree.RootNode.Descendents[0].Content);
            Assert.AreEqual(2, parseTree.RootNode.Descendents[0].Descendents.Count);
            Assert.AreEqual(NodeType.PointerType, parseTree.RootNode.Descendents[0].Descendents[0].NodeType);
            Assert.AreEqual(NodeType.PointerType, parseTree.RootNode.Descendents[0].Descendents[1].NodeType);
            Assert.AreEqual("*", parseTree.RootNode.Descendents[0].Descendents[0].Content);
            Assert.AreEqual("*", parseTree.RootNode.Descendents[0].Descendents[1].Content);
            Assert.AreEqual(0, parseTree.RootNode.Descendents[0].Descendents[0].Descendents.Count);
        }

        [TestMethod]
        public void ParsePointerPointerPointerType()
        {
            var source = "int***";
            var parseTree = ParseSource(source);

            Assert.AreEqual(1, parseTree.RootNode.Descendents.Count);
            Assert.AreEqual(NodeType.TypeName, parseTree.RootNode.Descendents[0].NodeType);
            Assert.AreEqual("int", parseTree.RootNode.Descendents[0].Content);
            Assert.AreEqual(3, parseTree.RootNode.Descendents[0].Descendents.Count);
            Assert.AreEqual(NodeType.PointerType, parseTree.RootNode.Descendents[0].Descendents[0].NodeType);
            Assert.AreEqual(NodeType.PointerType, parseTree.RootNode.Descendents[0].Descendents[1].NodeType);
            Assert.AreEqual(NodeType.PointerType, parseTree.RootNode.Descendents[0].Descendents[2].NodeType);
            Assert.AreEqual("*", parseTree.RootNode.Descendents[0].Descendents[0].Content);
            Assert.AreEqual("*", parseTree.RootNode.Descendents[0].Descendents[1].Content);
            Assert.AreEqual("*", parseTree.RootNode.Descendents[0].Descendents[2].Content);
            Assert.AreEqual(0, parseTree.RootNode.Descendents[0].Descendents[0].Descendents.Count);
        }

        [TestMethod]
        public void ParseTemplateType()
        {
            var source = "set<int>";
            var parseTree = ParseSource(source);

            var typeName = new ParseNodeAssert(parseTree.RootNode.Descendents.Single());
            typeName
                .ExpectedContent("set")
                .ExpectedNodeType(NodeType.TypeName)
                .ExpectedDescendentCount(1)
                .ExpectedDescendent(0, NodeType.TemplateType, 1)
                .CheckDescendent(0, templateType =>
                {
                    templateType
                        .ExpectedDescendentCount(1)
                        .ExpectedDescendent(0, NodeType.TypeName, "int", 0);
                });
        }

        [TestMethod]
        public void ParseTemplatePointerType()
        {
            var source = "set<int*>";
            var parseTree = ParseSource(source);

            var typeName = new ParseNodeAssert(parseTree.RootNode.Descendents.Single());
            typeName
                .ExpectedContent("set")
                .ExpectedNodeType(NodeType.TypeName)
                .ExpectedDescendentCount(1)
                .ExpectedDescendent(0, NodeType.TemplateType, 1)
                .CheckDescendent(0, templateType =>
                {
                    templateType
                        .ExpectedDescendentCount(1)
                        .ExpectedDescendent(0, NodeType.TypeName, "int", 1)
                        .CheckDescendent(0, templateTypeName => templateTypeName.ExpectedDescendent(0, NodeType.PointerType, 0));
                });
        }

        [TestMethod]
        public void ParseNestedTemplateType()
        {
            var source = "vector<set<int>>";
            var parseTree = ParseSource(source);

            var typeName = new ParseNodeAssert(parseTree.RootNode.Descendents.Single());
            typeName
                .ExpectedContent("vector")
                .ExpectedNodeType(NodeType.TypeName)
                .ExpectedDescendentCount(1)
                .ExpectedDescendent(0, NodeType.TemplateType, 1)
                .CheckDescendent(0, templateType =>
                {
                    templateType
                        .ExpectedDescendentCount(1)
                        .ExpectedDescendent(0, NodeType.TypeName, "set", 1)
                        .CheckDescendent(0, typeName2 => {
                            typeName2
                                .ExpectedDescendentCount(1)
                                .ExpectedDescendent(0, NodeType.TemplateType, 1)
                                .CheckDescendent(0, templateType2 => templateType2.ExpectedDescendent(0, NodeType.TypeName, "int", 0));
                        });
                });
        }

        [TestMethod]
        public void ParseTemplateWithMultipleTypeParametersType()
        {
            var source = "map<int, char>";
            var parseTree = ParseSource(source);

            var typeName = new ParseNodeAssert(parseTree.RootNode.Descendents.Single());
            typeName
                .ExpectedContent("map")
                .ExpectedNodeType(NodeType.TypeName)
                .ExpectedDescendentCount(1)
                .ExpectedDescendent(0, NodeType.TemplateType, 2)
                .CheckDescendent(0, templateType =>
                {
                    templateType
                        .ExpectedDescendent(0, NodeType.TypeName, "int", 0)
                        .ExpectedDescendent(1, NodeType.TypeName, "char", 0);
                });
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
