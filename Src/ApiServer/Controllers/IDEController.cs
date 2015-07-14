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
        public string Format(string code)
        {
            //https://github.com/dotnet/codeformatter
            throw new NotImplementedException();
        }
        
        [HttpPost]
        public string Convert(ConvertInput input)
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
