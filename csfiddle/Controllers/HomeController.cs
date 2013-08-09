using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.Web.Mvc;
using csfiddle.Controllers.ViewModels;
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
            return new ContentResult { Content = CompileAndRun(vm.InputCode) };
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

            return new ContentResult { Content = hash };
        }

        static string CompileAndRun(string code)
        {
            var compilerParams = new CompilerParameters
            {
                GenerateInMemory = false,
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
                return compile.Errors.Cast<CompilerError>().Aggregate("Compile error: ", (current, ce) => current
                    + string.Format("Line: {0}<br />Column: {1}<br />Error Code: {2}<br />Error Text: {3}<br />",
                    ce.Line, ce.Column, ce.ErrorNumber, ce.ErrorText));
            }

            var sandbox = Sandbox.Create();
            return sandbox.Execute(compile.PathToAssembly);
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
                InputCode = string.Join(Environment.NewLine, code),
                Result = "Welcome!"
            };
            return View(vm);
        }

        public class Sandbox : MarshalByRefObject
        {
            const string BaseDirectory = "Untrusted";
            const string DomainName = "Sandbox";

            public static Sandbox Create()
            {
                var setup = new AppDomainSetup()
                {
                    ApplicationBase = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, BaseDirectory),
                    ApplicationName = DomainName,
                    DisallowBindingRedirects = true,
                    DisallowCodeDownload = true,
                    DisallowPublisherPolicy = true
                };

                var permissions = new PermissionSet(PermissionState.None);
                permissions.AddPermission(new ReflectionPermission(ReflectionPermissionFlag.RestrictedMemberAccess));
                permissions.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));

                var domain = AppDomain.CreateDomain(DomainName, null, setup, permissions, typeof(Sandbox).Assembly.Evidence.GetHostEvidence<StrongName>());

                return (Sandbox)Activator.CreateInstanceFrom(domain, typeof(Sandbox).Assembly.ManifestModule.FullyQualifiedName, typeof(Sandbox).FullName).Unwrap();
            }

            public string Execute(string assemblyPath)
            {
                try
                {
                    new FileIOPermission(FileIOPermissionAccess.Read | FileIOPermissionAccess.PathDiscovery,
                        assemblyPath).Assert();
                    var assembly = Assembly.LoadFile(assemblyPath);
                    CodeAccessPermission.RevertAssert();

                    new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
                    var stringWriter = new StringWriter();
                    Console.SetOut(stringWriter);
                    CodeAccessPermission.RevertAssert();

                    var module = assembly.GetModules()[0];
                    var mainMethod = module.GetTypes().FirstOrDefault(t => t.GetMethods().Any(m => m.Name == "Main"));

                    if (mainMethod == null) return "Compile error: Couldn't find a valid Main() method to execute.";

                    mainMethod.GetMethod("Main").Invoke(null, new object[] {});
                    return "<pre>" + WebUtility.HtmlEncode(stringWriter.ToString()) + "</pre>";
                }
                catch (TargetInvocationException ex)
                {
                    if (ex.InnerException is SecurityException)
                    {
                        return "While in BETA, C#Fiddle runs code under very limited security permissions.<br />Your code failed requesting the following permission:<br /><br />"
                            + ex.InnerException.Message;
                    }
                    return ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }
    }
}
