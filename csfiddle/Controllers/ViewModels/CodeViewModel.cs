using System.ComponentModel.DataAnnotations;

namespace csfiddle.Controllers.ViewModels
{
    public class CodeViewModel
    {
        [DataType(DataType.MultilineText)]
        public string InputCode { get; set; }
    }
}