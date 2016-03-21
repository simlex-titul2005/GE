using SX.WebCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GE.WebUI.Models.Abstract
{
    public class VMDetailMaterial
    {
        public int Id { get; set; }
        public Enums.ModelCoreType ModelCoreType { get; set; }
        public Guid? FrontPictureId { get; set; }
        public string Title { get; set; }
        public string Html { get; set; }
        public DateTime DateCreate { get; set; }
        public int CommentsCount { get; set; }
        public int ViewsCount { get; set; }
        public int VoteUpCount { get; set; }
        public int VoteDownCount { get; set; }
        public VMMateriallnfo Info
        {
            get
            {
                return new VMMateriallnfo {
                    DateCreate=this.DateCreate,
                    CommentsCount=this.CommentsCount,
                    ViewsCount=this.ViewsCount,
                    VoteUpCount=this.VoteUpCount,
                    VoteDownCount=this.VoteDownCount
                };
            }
        }
    }
}