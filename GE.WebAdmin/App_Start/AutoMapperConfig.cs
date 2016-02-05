using AutoMapper;
using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using SX.WebCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GE.WebAdmin
{
    public class AutoMapperConfig
    {
        public static void Configure()
        {
            //article
            Mapper.CreateMap<Article, VMArticle>();
            Mapper.CreateMap<Article, VMEditArticle>();
            Mapper.CreateMap<VMEditArticle, Article>();

            //article type
            Mapper.CreateMap<ArticleType, VMArticleType>();
            Mapper.CreateMap<ArticleType, VMEditArticleType>();
            Mapper.CreateMap<VMEditArticleType, ArticleType>();

            //game
            Mapper.CreateMap<Game, VMGame>();
            Mapper.CreateMap<Game, VMEditGame>();
            Mapper.CreateMap<VMEditGame, Game>();

            //menu
            Mapper.CreateMap<SxMenu, VMMenu>();
            Mapper.CreateMap<SxMenu, VMEditMenu>();
            Mapper.CreateMap<VMEditMenu, SxMenu>();

            //menu item
            Mapper.CreateMap<SxMenuItem, VMMenuItem>();
            Mapper.CreateMap<SxMenuItem, VMEditMenuItem>();
            Mapper.CreateMap<VMEditMenuItem, SxMenuItem>();

            //picture
            Mapper.CreateMap<SxPicture, VMPicture>();
            Mapper.CreateMap<SxPicture, VMEditPicture>();
            Mapper.CreateMap<VMEditPicture, SxPicture>();
        }
    }
}