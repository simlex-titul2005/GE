using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GE.WebUI.Models
{
    public sealed class VMMenu
    {
        public VMMenu()
        {
            Items=new VMMenuItem[0];
        }

        public VMMenuItem[] Items { get; set; }
    }

    public sealed class VMMenuItem
    {
        public string Caption { get; set; }
        public string Url { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
    }
}