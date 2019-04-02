namespace OpenPlusPlusToSharpTests.ExampleSources
{
    public class SimplePrograms
    {
        public const string MainWithVariableAssignment = @"
            #include <iostream>
            #include <string>
             
            int main()
            {
            	int width;
            	width = 5;             
            	width = 7;

                std::string str = ""String test with space"";
             
            	return 0;
            }";
    }
}
