using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;
using csfiddle.Controllers.ViewModels;
using csfiddle.csfiddle.IdeOne;
using csfiddle.Database.Entities;
using csfiddle.Database.Repositories;
using Microsoft.CSharp;

namespace csfiddle.Controllers
{
    public class HomeController : Controller
    {
        [HttpPost]
        public ActionResult Run(CodeViewModel vm)
        {
            return new ContentResult{Content = WebUtility.HtmlEncode(CompileAndRun(vm.InputCode))};
        }

        [HttpPost]
        public ActionResult Save(CodeViewModel vm)
        {
            string hash = null;

            if (!string.IsNullOrEmpty(vm.Id))
            {
                var fiddle = new FiddleRepository().Get(vm.Id);
                if (fiddle != null)
                    hash = fiddle.Id;
            }
            if (hash == null)
            {
                const string hashOptions = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                var random = new Random();
                hash = new string(
                    Enumerable.Repeat(hashOptions, 8)
                              .Select(s => s[random.Next(s.Length)])
                              .ToArray());
            }

            new FiddleRepository().Insert(new Fiddle { InputCode = vm.InputCode, Id = hash, Result = CompileAndRun(vm.InputCode) });

            return new ContentResult {Content = hash};
        }

        static string CompileAndRun(string code)
        {
            var compilerParams = new CompilerParameters
            {
                GenerateInMemory = true,
                TreatWarningsAsErrors = false,
                GenerateExecutable = false,
                CompilerOptions = "/optimize"
            };

            string[] references = { "System.dll" };
            compilerParams.ReferencedAssemblies.AddRange(references);

            var provider = new CSharpCodeProvider();
            var compile = provider.CompileAssemblyFromSource(compilerParams, code);

            if (compile.Errors.HasErrors)
            {
                return compile.Errors.Cast<CompilerError>().Aggregate("Compile error: ", (current, ce) => current + string.Format("Line: {0}\nColumn: {1}\nError Code: {2}\nError Text: {3}\n",
                    ce.Line, ce.Column, ce.ErrorNumber, ce.ErrorText));
            }

            var module = compile.CompiledAssembly.GetModules()[0];
            var mainMethod = module.GetTypes().FirstOrDefault(t => t.GetMethods().Any(m => m.Name == "Main"));

            if (mainMethod == null) return "Compile error: Couldn't find a valid Main() method to execute.";
            
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            mainMethod.GetMethod("Main").Invoke(null, new object[]{});
            return stringWriter.ToString();
        }

        public ActionResult Show(string id)
        {
            var fiddle = new FiddleRepository().Get(id);
            if (fiddle == null)
                return RedirectToAction("Index");

            var vm = new CodeViewModel
            {
                Id = fiddle.Id,
                InputCode = string.Join(Environment.NewLine, fiddle.InputCode),
                Result = fiddle.Result
            };
            return View("Index", vm);
        }

        public ActionResult Index()
        {
            var code = new List<string>
            {
                "using System;",
                "",
                "public class Program",
                "{",
                "    public static void Main()",
                "    {",
                @"        Console.WriteLine(""Welcome!"");",
                "    }",
                "}"
            };

            var vm = new CodeViewModel
            {
                InputCode = string.Join(Environment.NewLine, code)
            };
            return View(vm);
        }
    }
}
