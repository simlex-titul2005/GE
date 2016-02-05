using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GE.WebAdmin.Models
{
    public sealed class VMMenuItemList
    {
        public VMMenuItemList()
        {
            Items = new VMMenuItem[0];
        }

        public int MenuId { get; set; }
        public VMMenuItem[] Items { get; set; }
    }
}