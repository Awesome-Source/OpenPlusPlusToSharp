OpenPlusPlusToSharp
===================

The purpose of this program is to convert a C++ project to a C# project.
Therefore the program has to read and parse the C++ sources and write equivalent C# code.

**Disclaimer**: This project is provided "as is". It may be incomplete and/or faulty. The author(s) of this project cannot be held responsible for any damage occurring due to using this software.

How does it work?
-----------------

First of all a C++ source file has to be read and prepocessed by the tokenizer.
This returns a list of tokens that are parsed in the next step.
After these two steps are done for the entire C++ project the program has a syntax tree for every file.

Afterwards the obtained information can be used to convert every C++ syntax tree to C# code.
During this process standard library usages have to be replaced by the C# variants.
Furthermore the C++ specific constructs like pointers need to be transpiled into valid C# code.
In the end a C# project has to be generated containing all the generated sources.

Read tokens
-----------

The tokenizer reads the sourc file and returns all recognized tokens.
The following tokens are recognized:

- { = Begin of a block
- } = End of a block
- ( = Opening bracket
- ) = Closing bracket
- ; = End of statement
- , = Separator
- Every other token is of type text

A token is also complete if it ends with a semicolon.
A string literal is exactly one token. Therefore no tokens are taken from the inside of a string literal.

Syntax Hierachy
---------------

The tokens of every source file are parsed into a ParseNode hierachy.
The hierachy follows a strict description provided below.
The description contains the necessary and optional child nodes of a specific parent node.

- SourceFile (content: file name)
	- 0..n IncludeDirective
	- 0..n ClassDeclaration
	
- IncludeDirective
	- 1 IncludeContent
	
- ClassDeclaration (content: class name)
	- 0..n MethodDeclaration
	
- MethodDeclaration (content: method name)
	- 0..n KeywordList
	- 0..1 ParameterList
	- 1 MethodBody