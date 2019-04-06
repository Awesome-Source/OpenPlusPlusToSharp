using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenPlusPlusToSharp.Parser;
using OpenPlusPlusToSharp.Parser.Parsers;
using OpenPlusPlusToSharp.Tokenizer;
using OpenPlusPlusToSharpTests.ExampleSources;
using System.Collections.Generic;
using System.Linq;

namespace OpenPlusPlusToSharpTests
{
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void ParseThreeIncludeDirectives_ReturnsParseTreeWithThreeDirectives()
        {
            var source = @"
                #include <iostream>
                #include <string>
                #include ""test""
            ";
            var tokenizer = new PlusPlusTokenizer(source);
            var topLevelParsers = new List<IParser>() { new IncludeParser() };
            var parser = new PlusPlusParser("test.cpp", tokenizer.Tokenize(), topLevelParsers);
            var parseTree = parser.Parse();

            var includeNodes = parseTree.RootNode.Descendents
                .Where(n => n.NodeType == NodeType.IncludeDirective)
                .ToList();

            Assert.AreEqual(3, includeNodes.Count);
            Assert.AreEqual("iostream", includeNodes[0].Content);
            Assert.AreEqual("string", includeNodes[1].Content);
            Assert.AreEqual("test", includeNodes[2].Content);
        }
    }
}
