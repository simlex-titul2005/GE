using AutoMapper;
using GE.WebCoreExtantions;
using GE.WebUI.Models;
using SX.WebCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GE.WebUI
{
    public class AutoMapperConfig
    {
        public static MapperConfiguration MapperConfigurationInstance
        {
            get
            {
                return new MapperConfiguration(cfg =>
                {
                    //articles
                    cfg.CreateMap<Article, VMDetailArticle>();

                    //articles
                    cfg.CreateMap<SxForumPart, VMForumPart>();

                    // comments
                    cfg.CreateMap<VMEditComment, SxComment>();

                    //games
                    cfg.CreateMap<Game, VMGame>();

                    //menu
                    cfg.CreateMap<SxMenu, VMMenu>();

                    //menu item
                    cfg.CreateMap<SxMenuItem, VMMenuItem>()
                        .ForMember(d => d.Url, d => d.MapFrom(s => s.Route != null ? s.Route.Url.ToLower() : null))
                        .ForMember(d => d.Controller, d => d.MapFrom(s => s.Route != null ? s.Route.Controller : null))
                        .ForMember(d => d.Action, d => d.MapFrom(s => s.Route != null ? s.Route.Action : null));

                    //seo info
                    cfg.CreateMap<SxSeoInfo, SiteSeoInfo>();
                });
            }
        }
    }
}