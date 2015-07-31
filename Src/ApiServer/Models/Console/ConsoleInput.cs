using OpenFiddle.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OpenFiddle.Models
{
    public class ConsoleInput
    {
        [Required(ErrorMessage = "Id is Required.")]
        public string Id { get; set; }
        [Required(ErrorMessage = "Code is Required.")]
        public string Code { get; set; }
        public Language Language { get; set; }
    }
}