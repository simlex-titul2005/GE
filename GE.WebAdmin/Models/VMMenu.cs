using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GE.WebAdmin.Models
{
    public sealed class VMMenu : ISxViewModel<int>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}