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

            var methodDeclarationNode = new ParseNode("testMethod", NodeType.MethodDeclaration);
            methodDeclarationNode.WithType("void");
            methodDeclarationNode.WithParameterList();
            ParseNodeHierachyCompare.Compare(methodDeclarationNode, parseTree.RootNode.Descendents.Single());
        }

        [TestMethod]
        public void ParseThreeMethodDeclarations_ReturnTypeVoid_NoParameters()
        {
            var source = "void testMethod(); int testMethod2();bool testMethod3();";
            var parseTree = ParseSource(source);

            var firstMethodNode = new ParseNode("testMethod", NodeType.MethodDeclaration);
            firstMethodNode.WithType("void");
            firstMethodNode.WithParameterList();

            var secondMethodNode = new ParseNode("testMethod2", NodeType.MethodDeclaration);
            secondMethodNode.WithType("int");
            secondMethodNode.WithParameterList();

            var thirdMethodNode = new ParseNode("testMethod3", NodeType.MethodDeclaration);
            thirdMethodNode.WithType("bool");
            thirdMethodNode.WithParameterList();
            ParseNodeHierachyCompare.Compare(new List<ParseNode> {firstMethodNode, secondMethodNode, thirdMethodNode }, parseTree.RootNode.Descendents);
        }

        [TestMethod]
        public void ParseMethodDeclaration_ReturnTypePointerType_NoParameters()
        {
            var source = "int* testMethod();";
            var parseTree = ParseSource(source);

            var methodDeclarationNode = new ParseNode("testMethod", NodeType.MethodDeclaration);
            methodDeclarationNode.WithType("int").WithPointerType();
            methodDeclarationNode.WithParameterList();
            ParseNodeHierachyCompare.Compare(methodDeclarationNode, parseTree.RootNode.Descendents.Single());
        }

        [TestMethod]
        public void ParseMethodDeclaration_ReturnTypeVoid_OneParameter()
        {
            var source = "void testMethod(int testInt);";
            var parseTree = ParseSource(source);

            var methodDeclarationNode = new ParseNode("testMethod", NodeType.MethodDeclaration);
            methodDeclarationNode.WithType("void");
            methodDeclarationNode.WithParameterList()
                .WithParameter(parameter => parameter.WithType("int"), "testInt");
            ParseNodeHierachyCompare.Compare(methodDeclarationNode, parseTree.RootNode.Descendents.Single());
        }

        [TestMethod]
        public void ParseMethodDeclaration_ReturnTypeVoid_TwoParameters()
        {
            var source = "void testMethod(int testInt, bool testBool);";
            var parseTree = ParseSource(source);

            var methodDeclarationNode = new ParseNode("testMethod", NodeType.MethodDeclaration);
            methodDeclarationNode.WithType("void");
            methodDeclarationNode.WithParameterList()
                .WithParameter(parameter => parameter.WithType("int"), "testInt")
                .WithParameter(parameter => parameter.WithType("bool"), "testBool");
            ParseNodeHierachyCompare.Compare(methodDeclarationNode, parseTree.RootNode.Descendents.Single());
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
