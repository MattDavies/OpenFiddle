using OpenFiddle.Models;
using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using OpenFiddle.Helpers;
using OpenFiddle.Resources;
using OpenFiddle.Models.Shared;


namespace OpenFiddle.Controllers
{
    public class ConsoleController : ApiController
    {

        [HttpPost]
        public Models.ConsoleOutput Run(ConsoleInput input)
        {
            var co = new Models.ConsoleOutput();
            co.Code = input.Code;
            co.Output = CompileHelper.CompileAndRun(input.Code, input.Language);
            co.Id = input.Id;
            return co;
        }

        [HttpGet]
        public string Code(Language language = Language.CSharp, Guid? id = null)
        {
            if (id.HasValue)
            {
                throw new NotImplementedException();
            }
            else
            {
                switch(language)
                {
                    case Language.CSharp:
                        return CodeSamples.HelloWorldConsoleCSharp;
                    case Language.VbNet:
                        return CodeSamples.HelloWorldConsoleVBNet;
                    default:
                       return CodeSamples.HelloWorldConsoleCSharp;
                }               
            }
        }

        [HttpPost]
        public bool Compile(ConsoleInput input)
        {
            throw new NotImplementedException();
        }
    }
}
