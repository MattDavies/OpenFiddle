using OpenFiddle.Models;
using OpenFiddle.Models.IntelliSense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OpenFiddle.Controllers
{
    public class IntelliSenseController : ApiController
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
    }
}
