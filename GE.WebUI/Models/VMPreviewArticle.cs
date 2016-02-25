using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GE.WebUI.Models
{
    public sealed class VMPreviewArticle
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime DateCreate { get; set; }
        public int ViewsCount { get; set; }
        public int CommentsCount { get; set; }
        public string Foreword { get; set; }
        public VMPreviewInfo PreviewInfo
        {
            get
            {
                return new VMPreviewInfo {
                    CommentsCount=this.CommentsCount,
                    DateCreate=this.DateCreate,
                    ViewsCount=this.ViewsCount
                };
            }
        }
    }
}