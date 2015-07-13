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
using OpenFiddle.Models.Console;

namespace OpenFiddle.Controllers
{
     public class ConsoleController : ApiController
    {
      

        [HttpPost]
        public Models.ConsoleOutput Run(ConsoleInput input)
        {
            var co = new Models.ConsoleOutput();
            co.Code = input.Code;
            co.Output =  CompileHelper.CompileAndRun(input.Code);
            co.Id = input.Id;
            return co;
        }

        [HttpGet]
        public string Code(Guid? id = null)
        {
            if (id.HasValue)
            {
                throw new NotImplementedException();
            }
            else
            {
                return CodeSamples.HelloWorldConsole;
            }
        }
         
        [HttpPost]
        public bool Compile(ConsoleInput input)
        {
            throw new NotImplementedException();
        }
    }
}
