using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using csfiddle.Controllers.ViewModels;
using csfiddle.Database.Entities;
using csfiddle.Database.Repositories;
using csfiddle.Helpers;

namespace csfiddle.Controllers
{
    public class HomeController : Controller
    {
        [HttpPost]
        public ActionResult Run(CodeViewModel vm)
        {
            return new ContentResult { Content = CompileHelper.CompileAndRun(vm.InputCode) };
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

            new FiddleRepository().Insert(new Fiddle { InputCode = vm.InputCode, Id = hash, Result = CompileHelper.CompileAndRun(vm.InputCode) });

            return new ContentResult { Content = hash };
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
    }
}
