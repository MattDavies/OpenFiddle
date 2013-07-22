using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using csfiddle.Controllers.ViewModels;

namespace csfiddle.Controllers
{
    public class HomeController : Controller
    {
        [HttpPost]
        public ActionResult Index(CodeViewModel vm)
        {
            return View(vm);
        }

        public ActionResult Index()
        {
            var code = new List<string>()
            {
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
