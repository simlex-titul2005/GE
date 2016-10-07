using System;

namespace GE.WebUI.ViewModels
{
    public sealed class VMSearchResult
    {
        public string TitleUrl { get; set; }
        public byte ModelCoreType { get; set; }
        public DateTime DateCreate { get; set; }
        public string Title { get; set; }
        public string Foreword { get; set; }
    }
}