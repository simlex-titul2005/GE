using SX.WebCore.Abstract;
using SX.WebCore.ViewModels;
using System;

namespace GE.WebUI.ViewModels
{
    public sealed class VMMaterialCategory : SxVMMaterialCategory, IHierarchy<VMMaterialCategory>
    {
        public VMGame Game { get; set; }

        public new VMMaterialCategory[] Childs { get; set; }

        public new Func<VMMaterialCategory, string> FuncGetTitle { get; set; } = x => { return x.Title; };
    }
}