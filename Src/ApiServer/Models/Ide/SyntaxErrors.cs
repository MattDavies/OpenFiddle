using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpenFiddle.Models.Ide
{
    public class SyntaxErrors : List<SyntaxError>
    {
    }


    public class SyntaxError
    {
        public string Description { get; set; }
        public string Warning { get; set; }
        public string Severity { get; set; }
        public string Location { get; set; }
        public string CharacterAt { get; set; }
        public string OnLine { get; set; }
    }
}