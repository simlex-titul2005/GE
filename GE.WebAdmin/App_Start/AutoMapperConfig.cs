﻿using AutoMapper;
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
        public static MapperConfiguration MapperConfigurationInstance
        {
            get
            {
                return new MapperConfiguration(cfg =>
                {
                    //article
                    cfg.CreateMap<Article, VMArticle>();
                    cfg.CreateMap<Article, VMEditArticle>();
                    cfg.CreateMap<VMEditArticle, Article>();

                    //article type
                    cfg.CreateMap<ArticleType, VMArticleType>();
                    cfg.CreateMap<ArticleType, VMEditArticleType>();
                    cfg.CreateMap<VMEditArticleType, ArticleType>();

                    //game
                    cfg.CreateMap<Game, VMGame>();
                    cfg.CreateMap<Game, VMEditGame>();
                    cfg.CreateMap<VMEditGame, Game>();

                    //menu
                    cfg.CreateMap<SxMenu, VMMenu>();
                    cfg.CreateMap<SxMenu, VMEditMenu>();
                    cfg.CreateMap<VMEditMenu, SxMenu>();

                    //menu item
                    cfg.CreateMap<SxMenuItem, VMMenuItem>()
                        .ForMember(d => d.Url, d => d.MapFrom(s => s.Route != null ? s.Route.Url : null));
                    cfg.CreateMap<SxMenuItem, VMEditMenuItem>();
                    cfg.CreateMap<VMEditMenuItem, SxMenuItem>();

                    //picture
                    cfg.CreateMap<SxPicture, VMPicture>();
                    cfg.CreateMap<SxPicture, VMEditPicture>();
                    cfg.CreateMap<VMEditPicture, SxPicture>();

                    //route
                    cfg.CreateMap<SxRoute, VMRoute>();
                    cfg.CreateMap<SxRoute, VMEditRoute>();
                    cfg.CreateMap<VMEditRoute, SxRoute>();

                    //route value
                    cfg.CreateMap<SxRouteValue, VMRouteValue>();
                    cfg.CreateMap<SxRouteValue, VMEditRouteValue>();
                    cfg.CreateMap<VMEditRouteValue, SxRouteValue>();
                });
            }
        }
    }
}