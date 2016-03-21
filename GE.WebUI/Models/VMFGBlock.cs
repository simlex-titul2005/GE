using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GE.WebUI.Models
{
    public sealed class VMFGBlock
    {
        public VMFGBlock()
        {
            Games = new VMFGBGame[0];
        }

        public VMFGBGame[] Games { get; set; }
        public int GameLength { get { return Games.Length; } }
        public bool HasGames { get { return GameLength != 0; } }
        public VMPreviewArticle[] Articles { get; set; }
        public string SelectedGameTitle { get; set; }
    }

    public sealed class VMFGBGame
    {
        public VMFGBGame()
        {
            ArticleTypes = new VMFGBArticleType[0];
        }
        public int Id { get; set; }
        public Guid FrontPictureId { get; set; }
        public string Title { get; set; }
        public string TitleUrl { get; set; }
        public string Description { get; set; }

        public VMFGBArticleType[] ArticleTypes { get; set; }
        public int ArticleTypeLength { get { return ArticleTypes.Length; } }
        public bool HasArticleTypes { get { return ArticleTypes.Length != 0; } }
    }

    public sealed class VMFGBArticleType
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}