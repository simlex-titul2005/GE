using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GE.WebAdmin.Models
{
    public sealed class VMRobotsFile
    {
        [Display(Name = "Содержание файла"), Required, DataType(DataType.MultilineText)]
        public string FileContent { get; set; }
        public string OldFileContent { get; set; }
    }
}