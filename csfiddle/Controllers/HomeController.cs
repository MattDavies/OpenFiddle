using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.AccessControl;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;
using csfiddle.Controllers.ViewModels;
using csfiddle.csfiddle.IdeOne;
using Microsoft.CSharp;

namespace csfiddle.Controllers
{
    public class HomeController : Controller
    {
        [HttpPost]
        public ActionResult Index(CodeViewModel vm)
        {
            return new ContentResult{Content = WebUtility.HtmlEncode(CompileAndRun(vm.InputCode))};
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
