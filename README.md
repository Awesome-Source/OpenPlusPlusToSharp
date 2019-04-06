OpenPlusPlusToSharp
===================

The purpose of this program is to convert a C++ project to a C# project.
Therefore the program has to read and parse the C++ sources and write equivalent C# code.

**Disclaimer**: This project is provided "as is". It may be incomplete and/or faulty. The author(s) of this project cannot be held responsible for any damage occurring due to using this software.

How does it work?
-----------------

These are the performed steps:

1. Tokenization: Read tokens from the source file
2. Parse: Parse the tokens into a syntax tree
3. Enhance: Turn the syntax tree of a file into a purified parsed file object.
4. Validation: Check datatypes etc.
5. Transpile: Convert every parsed file into C# equivalents and generate necessary helper classes.
6. Finalize: Create a C# project and solution file.

Read tokens
-----------

The tokenizer reads the sourc file and returns all recognized tokens.
There are three types of tokens:

- Special Characters: { } ( ) ;  < > = ! ? : + - * / % & ,
- String literals
- Text

A string literal is exactly one token. Therefore no tokens are taken from the inside of a string literal.

Syntax Hierachy
---------------

The tokens of every source file are parsed into a ParseNode hierachy.
The hierachy follows a strict description provided below.
The description contains the necessary and optional child nodes of a specific parent node.

- SourceFile (content: file name)
	- 0..n IncludeDirective
	- 0..n ClassDeclaration
	
- IncludeDirective (content: include file name)
	
- ClassDeclaration (content: class name)
	- 0..n MethodDeclaration
	
- MethodDeclaration (content: method name)
	- 0..n KeywordList
	- 0..1 ParameterList
	- 1 MethodBody