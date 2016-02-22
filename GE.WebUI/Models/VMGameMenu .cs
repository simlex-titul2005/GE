using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GE.WebUI.Models
{
    public sealed class VMGameMenu
    {
        public VMGameMenu()
        {
            Games = new VMGame[0];
        }

        public VMEmptyGame EmptyGame { get; set; }
        public VMGame[] Games { get; set; }
        public int Length 
        { 
            get
            {
                return Games.Length;
            }
        }
    }

    public sealed class VMEmptyGame
    {
        public string IconPath { get; set; }
        public string GoodImagePath { get; set; }
        public string BadImagePath { get; set; }
    }

    public sealed class VMGame
    {
        public string Title { get; set; }
        public string TitleAbbr { get; set; }
        public Guid FrontPictureId { get; set; }
        public Guid GoodPictureId { get; set; }
        public Guid BadPictureId { get; set; }
    }
}