using OpenFiddle.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpenFiddle.Models.Ide
{
    public class CodeInput
    {
        public string Code { get; set; }
        public Language Language { get; set; }
    }
}