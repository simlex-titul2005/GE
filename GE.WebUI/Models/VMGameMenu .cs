using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GE.WebUI.Models
{
    public sealed class VMGameMenu
    {
        public VMGameMenu(int imgWidth, int iconHeight)
        {
            Games = new VMGame[0];
            ImgWidth = imgWidth;
            IconHeight = iconHeight;
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

        public int ImgWidth { get; set; }
        public int IconHeight { get; set; }

        public VMImgGame this[string gameName]
        {
            get
            {
                var model = new VMImgGame
                {
                    Title="Все записи",
                    GoodPictureUrl=EmptyGame.GoodImagePath,
                    BadPictureUrl=EmptyGame.BadImagePath,
                    IconUrl=EmptyGame.IconPath,
                    ImgWidth=this.ImgWidth,
                    IconHeight=this.IconHeight
                };

                if (string.IsNullOrEmpty(gameName)) return model;

                for (int i = 0; i < Length; i++)
                {
                    var g = Games[i];
                    if (g.Title.ToLower() == gameName.ToLower())
                    {
                        model = new VMImgGame
                        {
                            Title = g.Title,
                            GoodPictureUrl = "/pictures/picture/"+g.GoodPictureId,
                            BadPictureUrl = "/pictures/picture/" + g.BadPictureId,
                            IconUrl = "/pictures/picture/" + g.FrontPictureId
                        };
                        break;
                    }
                }

                model.ImgWidth = this.ImgWidth;
                model.IconHeight = this.IconHeight;
                return model;
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
        public int Id { get; set; }
        public string Title { get; set; }
        public string TitleAbbr { get; set; }
        public Guid FrontPictureId { get; set; }
        public Guid GoodPictureId { get; set; }
        public Guid BadPictureId { get; set; }
        public int ImgWidth { get; set; }
        public int IconHeight { get; set; }
    }

    public sealed class VMImgGame
    {
        public string Title { get; set; }
        public string GoodPictureUrl { get; set; }
        public string BadPictureUrl { get; set; }
        public string IconUrl { get; set; }
        public int ImgWidth { get; set; }
        public int IconHeight { get; set; } 
    }
}