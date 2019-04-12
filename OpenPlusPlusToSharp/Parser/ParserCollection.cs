using OpenPlusPlusToSharp.Parser.Parsers;
using System.Collections.Generic;

namespace OpenPlusPlusToSharp.Parser
{
    /// <summary>
    /// This class is used to get all parsers that are necessary to parse a C++ source file.
    /// </summary>
    public static class ParserCollection
    {
        /// <summary>
        /// Returns all top level parsers.
        /// </summary>
        /// <returns>Returns all top level parsers.</returns>
        public static List<IParser> GetTopLevelParsers()
        {
            var parsers = new List<IParser>();

            parsers.Add(new IncludeParser());
            parsers.Add(new DefineParser());
            parsers.Add(new PragmaParser());
            parsers.Add(new UsingParser());
            parsers.Add(new ForwardDeclarationParser());
            var parameterListParser = new ParameterListParser();
            var constructorDeclarationParser = new ConstructorDeclarationParser(parameterListParser);
            var destructorDeclarationParser = new DestructorDeclarationParser(parameterListParser);
            var methodDeclarationParser = new MethodDeclarationParser(parameterListParser);
            var methodAccessModifierParser = new MethodAccessModifierParser(constructorDeclarationParser, destructorDeclarationParser, methodDeclarationParser);
            parsers.Add(new ClassParser(methodAccessModifierParser));

            return parsers;
        }
    }
}
