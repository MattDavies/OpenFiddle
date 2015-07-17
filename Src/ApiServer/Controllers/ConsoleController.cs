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
using OpenFiddle.Repos;
using OpenFiddle.Repos.Data;
using System.Web;
using System.ServiceModel.Channels;

namespace OpenFiddle.Controllers
{
    public class ConsoleController : ApiController
    {
        private readonly ILogRepository _logRepository;
        private readonly IFiddleRepository _fiddleRepository;

        public ConsoleController(ILogRepository logRepository, IFiddleRepository fiddleRepository)
        {
            _logRepository = logRepository;
            _fiddleRepository = fiddleRepository;
        }

        [HttpPost]
        public ConsoleOutput Run(ConsoleInput input)
        {
            _logRepository.Insert(new Log { InputCode = input.Code, IpAddress = GetClientIp(Request) });
            //TODO: use Roslyn to compile and run
            var co = new Models.ConsoleOutput();
            co.Code = input.Code;
            co.Output = CompileHelper.CompileAndRun(input.Code, input.Language);
            co.Id = input.Id;
            return co;
        }

        [HttpPost]
        public ConsoleOutput Save(ConsoleInput input)
        {
            _logRepository.Insert(new Log { InputCode = input.Code, IpAddress = GetClientIp(Request) });

            string id = null;

            if (!string.IsNullOrEmpty(input.Id))
            {
                var fiddle = _fiddleRepository.Get(input.Id);
                if (fiddle != null)
                    id = fiddle.Id;
            }

            var co = Run(input);

            _fiddleRepository.Insert(new Fiddle { InputCode = input.Code, Id = id, Result = co.Output, Language = input.Language });

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
                switch (language)
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

        private string GetClientIp(HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            }

            if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                RemoteEndpointMessageProperty prop;
                prop = (RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name];
                return prop.Address;
            }

            return null;
        }
    }
}
