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
using OpenFiddle.Models.Script;
using OpenFiddle.Models.Shared;

namespace OpenFiddle.Controllers
{
    public class ScriptController : ApiController
    {

        [HttpPost]
        public ScriptOutput Run(ScriptInput input)
        {
            var co = new ScriptOutput();
            co.Code = input.Code;
            co.Output = CompileHelper.CompileAndRun(string.Format(input.Language == Language.CSharp?CodeSamples.ScriptWrapperCSharp:CodeSamples.ScriptWrapperVBNet , input.Code), input.Language);
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
                        return CodeSamples.HelloWorldScriptCSharp;
                    case Language.VbNet:
                        return CodeSamples.HelloWorldScriptVBNet;
                    default:
                       return CodeSamples.HelloWorldScriptCSharp;
                }               
            }
        }

        [HttpPost]
        public bool Compile(ScriptInput input)
        {
            throw new NotImplementedException();
        }
    }
}
