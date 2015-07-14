using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.VisualBasic;
using OpenFiddle.Models;
using OpenFiddle.Models.Ide;
using OpenFiddle.Models.Shared;
using OpenFiddle.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OpenFiddle.Controllers
{
    public class IDEController : ApiController
    {

        [HttpPost]
        public SyntaxErrors CheckSyntax(string code)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public AutoCompleteSuggestions AutoComplete(string code)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public string Format(CodeInput input)
        {
            var workspace = new AdhocWorkspace();

            if (input.Language == Language.CSharp)
            {
                var tree = CSharpSyntaxTree.ParseText(input.Code);
                return Formatter.Format(tree.GetRoot(), workspace).ToString();
            }
            else if (input.Language == Language.VbNet)
            {
                var tree = VisualBasicSyntaxTree.ParseText(input.Code);
                return Formatter.Format(tree.GetRoot(), workspace).ToString();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        [HttpPost]
        public string Convert(CodeInput input)
        {
            switch (input.Language)
            {
                case Language.CSharp:
                    if (input.Code == CodeSamples.HelloWorldConsoleVBNet)
                        return CodeSamples.HelloWorldConsoleCSharp;
                    else if (input.Code == CodeSamples.HelloWorldScriptVBNet)
                        return CodeSamples.HelloWorldScriptCSharp;
                    else
                        throw new NotImplementedException();
                case Language.VbNet:
                    if (input.Code == CodeSamples.HelloWorldConsoleCSharp)
                        return CodeSamples.HelloWorldConsoleVBNet;
                    else if (input.Code == CodeSamples.HelloWorldScriptCSharp)
                        return CodeSamples.HelloWorldScriptVBNet;
                    else
                        throw new NotImplementedException();
                default:
                    return string.Empty;
            }
        }

        [HttpGet]
        public Guid GUID()
        {
            return Guid.NewGuid();
        }

    }
}
