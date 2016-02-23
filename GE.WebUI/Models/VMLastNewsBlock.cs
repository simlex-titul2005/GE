using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GE.WebUI.Models
{
    public sealed class VMLastNewsBlock
    {
        public VMLastNewsBlock()
        {
            News = new VMLastNewsBlockNews[0];
        }

        public VMLastNewsBlockNews[] News { get; set; }
        public int NewsLength
        {
            get
            {
                return News.Length;
            }
        }
        public bool HasNews
        {
            get
            {
                return NewsLength != 0;
            }
        }
    }

    public sealed class VMLastNewsBlockNews
    {
        public string DateCreate { get; set; }
        public string Title { get; set; }
        public int GameId { get; set; }
        public Guid? FrontPictureId { get; set; }
    }
}