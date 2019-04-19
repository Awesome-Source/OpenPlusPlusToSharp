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
    public class MethodDeclarationParserTests
    {
        [TestMethod]
        public void ParseMethodDeclaration_ReturnTypeVoid_NoParameters()
        {
            var source = "void testMethod();";
            var parseTree = ParseSource(source);

            var methodDeclaration = new ParseNodeAssert(parseTree.RootNode.Descendents.Single());
            methodDeclaration
                .ExpectedNodeType(NodeType.MethodDeclaration)
                .ExpectedContent("testMethod")
                .ExpectedDescendentCount(2)
                .ExpectedDescendent(0, NodeType.TypeName, "void", 0)
                .ExpectedDescendent(1, NodeType.ParameterList, 0);
        }

        [TestMethod]
        public void ParseThreeMethodDeclarations_ReturnTypeVoid_NoParameters()
        {
            var source = "void testMethod(); int testMethod2();bool testMethod3();";
            var parseTree = ParseSource(source);

            var root = new ParseNodeAssert(parseTree.RootNode);
            root
                .ExpectedDescendentCount(3)
                .ExpectedDescendent(0, NodeType.MethodDeclaration, "testMethod", 2)
                .ExpectedDescendent(1, NodeType.MethodDeclaration, "testMethod2", 2)
                .ExpectedDescendent(2, NodeType.MethodDeclaration, "testMethod3", 2)
                .CheckDescendent(0, methodDeclaration => {
                    methodDeclaration
                    .ExpectedDescendent(0, NodeType.TypeName, "void", 0)
                    .ExpectedDescendent(1, NodeType.ParameterList, 0);
                })
                .CheckDescendent(1, methodDeclaration => {
                    methodDeclaration
                    .ExpectedDescendent(0, NodeType.TypeName, "int", 0)
                    .ExpectedDescendent(1, NodeType.ParameterList, 0);
                })
                .CheckDescendent(2, methodDeclaration => {
                    methodDeclaration
                    .ExpectedDescendent(0, NodeType.TypeName, "bool", 0)
                    .ExpectedDescendent(1, NodeType.ParameterList, 0);
                });
        }

        [TestMethod]
        public void ParseMethodDeclaration_ReturnTypePointerType_NoParameters()
        {
            var source = "int* testMethod();";
            var parseTree = ParseSource(source);

            var methodDeclaration = new ParseNodeAssert(parseTree.RootNode.Descendents.Single());
            methodDeclaration
                .ExpectedNodeType(NodeType.MethodDeclaration)
                .ExpectedDescendentCount(2)
                .ExpectedDescendent(0, NodeType.TypeName, 1)
                .ExpectedDescendent(1, NodeType.ParameterList, 0)
                .CheckDescendent(0, returnType => returnType.ExpectedDescendentCount(1).ExpectedDescendent(0, NodeType.PointerType, 0));
        }

        [TestMethod]
        public void ParseMethodDeclaration_ReturnTypeVoid_OneParameter()
        {
            var source = "void testMethod(int testInt);";
            var parseTree = ParseSource(source);

            var methodDeclaration = new ParseNodeAssert(parseTree.RootNode.Descendents.Single());
            methodDeclaration
                .ExpectedNodeType(NodeType.MethodDeclaration)
                .ExpectedDescendentCount(2)
                .ExpectedDescendent(0, NodeType.TypeName, 0)
                .ExpectedDescendent(1, NodeType.ParameterList, 1)
                .CheckDescendent(1, parameterList => parameterList.HasParameterDescendent(0, "int", "testInt"));
        }

        [TestMethod]
        public void ParseMethodDeclaration_ReturnTypeVoid_TwoParameters()
        {
            var source = "void testMethod(int testInt, bool testBool);";
            var parseTree = ParseSource(source);

            var methodDeclaration = new ParseNodeAssert(parseTree.RootNode.Descendents.Single());
            methodDeclaration
                .ExpectedNodeType(NodeType.MethodDeclaration)
                .ExpectedDescendentCount(2)
                .ExpectedDescendent(0, NodeType.TypeName, 0)
                .ExpectedDescendent(1, NodeType.ParameterList, 2)
                .CheckDescendent(1, parameterList => parameterList.HasParameterDescendent(0, "int", "testInt"))
                .CheckDescendent(1, parameterList => parameterList.HasParameterDescendent(1, "bool", "testBool"));
        }

        private static ParseTree ParseSource(string source)
        {
            var tokenizer = new PlusPlusTokenizer(source);
            var typeParser = new TypeParser();
            var topLevelParsers = new List<IParser>() { new MethodDeclarationParser(new ParameterListParser(typeParser), typeParser) };
            var parser = new PlusPlusParser("test.cpp", tokenizer.Tokenize(), topLevelParsers);
            var parseTree = parser.Parse();
            return parseTree;
        }
    }
}
