using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GE.WebAdmin.Models
{
    public sealed class VMClick : ISxViewModel<Guid>
    {
        public Guid Id { get; set; }
        public string UrlRef { get; set; }
        public string RawUrl { get; set; }
        public string LinkTarget { get; set; }
        public string ClickTypeName { get; set; }
    }
}