using System;
using System.Collections.Generic;
using System.Linq;
using OpenPlusPlusToSharp.Parser;

namespace OpenPlusPlusToSharpTests.TestUtils
{
    public class ParseNodeHierachyCompare
    {
        public static void Compare(ParseNode expected, ParseNode actual)
        {
            if(expected.NodeType != actual.NodeType)
            {
                throw new Exception($"Expected node type {expected.NodeType} but got {actual.NodeType}");
            }

            if (expected.Content != actual.Content)
            {
                throw new Exception($"Expected content {expected.Content} but got {actual.Content}");
            }

            if (expected.Descendents.Count != actual.Descendents.Count)
            {
                throw new Exception($"Expected {expected.Descendents.Count} ({expected}) descendents but got {actual.Descendents.Count} ({actual}) descendents");
            }

            Compare(expected.Descendents, actual.Descendents);
        }

        public static void Compare(List<ParseNode> expected, List<ParseNode> actual)
        {
            var elementCount = expected.Count > actual.Count ? expected.Count : actual.Count;

            for(var i = 0; i < elementCount; i++)
            {
                var expectedNode = expected.ElementAtOrDefault(i);
                var actualNode = actual.ElementAtOrDefault(i);

                if(expectedNode == null)
                {
                    throw new ArgumentException($"Unexpected node: {actualNode.NodeType}: {actualNode.Content}");
                }

                if(actualNode == null)
                {
                    throw new ArgumentException($"Missing node: {expectedNode.NodeType}: {expectedNode.Content}");
                }

                Compare(expectedNode, actualNode);
            }
        }
    }
}
