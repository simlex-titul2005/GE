using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GE.ViewModels
{
    public class VMSiteMenu
    {
        public VMSiteMenuItem BrandItem { get; set; }
        public List<VMSiteMenuItem> Items { get; set; }
        public VMAuthBlock AuthBlock { get; set; }

    }

    public class VMAuthBlock
    {
        public List<VMSiteMenuItem> MenuItems { get; set; }
    }

    public class VMSiteMenuItem
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public int OrderIndex { get; set; }
    }
}
