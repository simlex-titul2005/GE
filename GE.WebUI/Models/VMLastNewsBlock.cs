using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GE.WebUI.Models
{
    public sealed class VMLastNewsBlock
    {
        private int _amount;
        public VMLastNewsBlock(int amount)
        {
            _amount = amount;
            News = new VMLastNewsBlockNews[0];
        }

        public int Amount 
        { 
            get
            {
                return _amount;
            }
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

        public VMLastNewsBlockGame[] Games
        {
            get
            {
                return News.Select(x => new VMLastNewsBlockGame
                {
                    Id=x.GameId,
                    FrontPictureId = x.FrontPictureId,
                    Title = x.GameTitle,
                    TitleUrl=x.GameTitle
                })
                .ToArray();
            }
        }
        public int GamesLength
        {
            get
            {
                return Games.Length;
            }
        }
        public bool HasGames
        {
            get
            {
                return GamesLength != 0;
            }
        }
    }

    public sealed class VMLastNewsBlockNews
    {
        public string TitleUrl { get; set; }
        public string Title { get; set; }
        public int GameId { get; set; }
        public string GameTitle { get; set; }
        public Guid? FrontPictureId { get; set; }
        public DateTime DateCreate { get; set; }
    }

    public sealed class VMLastNewsBlockGame
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string TitleUrl { get; set; }
        public Guid? FrontPictureId { get; set; }
    }
}