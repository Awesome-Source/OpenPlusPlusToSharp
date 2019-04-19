using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenPlusPlusToSharp.Parser;

namespace OpenPlusPlusToSharpTests.TestUtils
{
    public class ParseNodeAssert
    {
        private readonly ParseNode _currentNode;

        public ParseNodeAssert(ParseNode currentNode)
        {
            _currentNode = currentNode;
        }

        public ParseNodeAssert ExpectedDescendentCount(int expectedNumber)
        {
            Assert.AreEqual(expectedNumber, _currentNode.Descendents.Count);

            return this;
        }

        public ParseNodeAssert ExpectedNodeType(NodeType expectedNodeType)
        {
            Assert.AreEqual(expectedNodeType, _currentNode.NodeType);

            return this;
        }

        public ParseNodeAssert ExpectedContent(string expectedContent)
        {
            Assert.AreEqual(expectedContent, _currentNode.Content);

            return this;
        }

        public ParseNodeAssert ExpectedDescendent(int descendentIndex, NodeType expectedNodeType, string expectedContent, int expectedNumberOfDescendents)
        {
            var descendentAssert = new ParseNodeAssert(_currentNode.Descendents[descendentIndex]);
            descendentAssert
                .ExpectedNodeType(expectedNodeType)
                .ExpectedContent(expectedContent)
                .ExpectedDescendentCount(expectedNumberOfDescendents);

            return this;
        }

        public ParseNodeAssert ExpectedDescendent(int descendentIndex, NodeType expectedNodeType, int expectedNumberOfDescendents)
        {
            var descendentAssert = new ParseNodeAssert(_currentNode.Descendents[descendentIndex]);
            descendentAssert
                .ExpectedNodeType(expectedNodeType)
                .ExpectedDescendentCount(expectedNumberOfDescendents);

            return this;
        }

        public ParseNodeAssert CheckDescendent(int descendentIndex, Action<ParseNodeAssert> checkAction)
        {
            var descendentAssert = new ParseNodeAssert(_currentNode.Descendents[descendentIndex]);
            checkAction(descendentAssert);

            return this;
        }
    }
}
