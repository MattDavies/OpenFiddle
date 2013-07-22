using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;
using csfiddle.Controllers.ViewModels;
using csfiddle.csfiddle.IdeOne;

namespace csfiddle.Controllers
{
    public class HomeController : Controller
    {
        [HttpPost]
        public async Task<ActionResult> Index(CodeViewModel vm)
        {
            var client = new Ideone_Service_v1Service();
            
            var thing = client.createSubmission("Thallar", "randompass3", vm.InputCode, 27, string.Empty, true, false);
            var result = thing.OfType<XmlElement>().Select(o => (o).ChildNodes).ToDictionary(x => x.Item(0).InnerText, x => x.Item(1).InnerText);
            while (string.IsNullOrEmpty(vm.Result))
            {
                var req = client.getSubmissionDetails("Thallar", "randompass3", result["link"], true, true, true, true, true);
                vm.Result = req.OfType<XmlElement>().Select(o => (o).ChildNodes).ToDictionary(x => x.Item(0).InnerText, x => x.Item(1).InnerText)["output"];
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }

            return View(vm);
        }

        public ActionResult Index()
        {
            var code = new List<string>()
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
