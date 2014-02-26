using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace OpenFiddle.Controllers.ViewModels
{
    public class CodeViewModel
    {
        public string Id { get; set; }

        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string InputCode { get; set; }

        [ReadOnly(true)]
        public string Result { get; set; }
    }
}