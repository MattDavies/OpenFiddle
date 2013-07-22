using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace csfiddle.Controllers.ViewModels
{
    public class CodeViewModel
    {
        [DataType(DataType.MultilineText)]
        public string InputCode { get; set; }

        [ReadOnly(true)]
        public string Result { get; set; }
    }
}