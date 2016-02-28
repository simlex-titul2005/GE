using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GE.WebUI.Models
{
    public class VMPreviewArticle
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime DateCreate { get; set; }
        public int ViewsCount { get; set; }
        public int CommentsCount { get; set; }
        public string Foreword { get; set; }
        public VMMateriallnfo Info
        {
            get
            {
                return new VMMateriallnfo
                {
                    CommentsCount=this.CommentsCount,
                    DateCreate=this.DateCreate,
                    ViewsCount=this.ViewsCount
                };
            }
        }
    }
}