using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenPlusPlusToSharp.Parser;
using OpenPlusPlusToSharp.Parser.Parsers;
using OpenPlusPlusToSharp.Tokenizer;

namespace OpenPlusPlusToSharpTests.Parser.Parsers
{
    [TestClass]
    public class TypeParserTests
    {
        [TestMethod]
        public void ParseSimpleType()
        {
            var source = "int";
            var tokenizer = new PlusPlusTokenizer(source);
            var topLevelParsers = new List<IParser>() { new TypeParser() };
            var parser = new PlusPlusParser("test.cpp", tokenizer.Tokenize(), topLevelParsers);
            var parseTree = parser.Parse();

            Assert.AreEqual(1, parseTree.RootNode.Descendents.Count);
            Assert.AreEqual(NodeType.TypeName, parseTree.RootNode.Descendents[0].NodeType);
            Assert.AreEqual("int", parseTree.RootNode.Descendents[0].Content);
            Assert.AreEqual(0, parseTree.RootNode.Descendents[0].Descendents.Count);
        }

        [TestMethod]
        public void ParsePointerType()
        {
            var source = "int*";
            var tokenizer = new PlusPlusTokenizer(source);
            var topLevelParsers = new List<IParser>() { new TypeParser() };
            var parser = new PlusPlusParser("test.cpp", tokenizer.Tokenize(), topLevelParsers);
            var parseTree = parser.Parse();

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
            var tokenizer = new PlusPlusTokenizer(source);
            var topLevelParsers = new List<IParser>() { new TypeParser() };
            var parser = new PlusPlusParser("test.cpp", tokenizer.Tokenize(), topLevelParsers);
            var parseTree = parser.Parse();

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
            var tokenizer = new PlusPlusTokenizer(source);
            var topLevelParsers = new List<IParser>() { new TypeParser() };
            var parser = new PlusPlusParser("test.cpp", tokenizer.Tokenize(), topLevelParsers);
            var parseTree = parser.Parse();

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
            var tokenizer = new PlusPlusTokenizer(source);
            var topLevelParsers = new List<IParser>() { new TypeParser() };
            var parser = new PlusPlusParser("test.cpp", tokenizer.Tokenize(), topLevelParsers);
            var parseTree = parser.Parse();

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
            var tokenizer = new PlusPlusTokenizer(source);
            var topLevelParsers = new List<IParser>() { new TypeParser() };
            var parser = new PlusPlusParser("test.cpp", tokenizer.Tokenize(), topLevelParsers);
            var parseTree = parser.Parse();

            var topLevelNodes = parseTree.RootNode.Descendents;
            Assert.AreEqual(1, topLevelNodes.Count);
            var typeNameNode = topLevelNodes[0];
            Assert.AreEqual(NodeType.TypeName, typeNameNode.NodeType);
            Assert.AreEqual("set", typeNameNode.Content);
            Assert.AreEqual(1, typeNameNode.Descendents.Count);
            var templateTypeNode = typeNameNode.Descendents[0];
            Assert.AreEqual(NodeType.TemplateType, templateTypeNode.NodeType);
            Assert.AreEqual("", templateTypeNode.Content);
            Assert.AreEqual(1, templateTypeNode.Descendents.Count);
            Assert.AreEqual(NodeType.TypeName, templateTypeNode.Descendents[0].NodeType);
            Assert.AreEqual("int", templateTypeNode.Descendents[0].Content);
        }
    }
}
