using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpenFiddle.Models
{
    public class ScriptOutput
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Output { get; set; }
    }
}