using OpenPlusPlusToSharp.Parser;
using OpenPlusPlusToSharp.Tokenizer;
using System;
using System.IO;
using System.Linq;

namespace OpenPlusPlusToSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = ParseCommandLineArguments(args);
            Run(config);
        }

        private static void Run(CommandLineConfiguration config)
        {
            if (!string.IsNullOrEmpty(config.TestParseFile))
            {
                TestParseFile(config.TestParseFile);
            }
        }

        private static CommandLineConfiguration ParseCommandLineArguments(string[] args)
        {
            var config = new CommandLineConfiguration();

            for (var i = 0; i < args.Length; i++)
            {
                var currentArgument = args[i];

                switch (currentArgument)
                {
                    case "--test-parse-file":
                        HandleTestParseFileArgument(args, ref i, config);
                        break;
                }
            }

            return config;
        }

        private static void HandleTestParseFileArgument(string[] args, ref int i, CommandLineConfiguration config)
        {
            var filePath = args.ElementAtOrDefault(i + 1);
            if (filePath == null)
            {
                return;
            }

            config.TestParseFile = filePath;
            i++;
        }

        private static void TestParseFile(string filePath)
        {
            var content = File.ReadAllText(filePath);
            var tokenizer = new PlusPlusTokenizer(content);
            var topLevelParsers = ParserCollection.GetTopLevelParsers();
            var parser = new PlusPlusParser(filePath, tokenizer.Tokenize(), topLevelParsers);

            try
            {
                parser.Parse();
                Console.WriteLine("Successfully parsed file.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not parse file.");
                Console.WriteLine(e.Message);
            }
        }
    }
}
