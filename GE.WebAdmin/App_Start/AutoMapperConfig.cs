using AutoMapper;
using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using Microsoft.AspNet.Identity.EntityFramework;
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

                    //click type
                    cfg.CreateMap<SxClickType, VMClickType>();
                    cfg.CreateMap<SxClickType, VMEditClickType>();
                    cfg.CreateMap<VMEditClickType, SxClickType>();

                    //forum parts
                    cfg.CreateMap<SxForumPart, VMForumPart>();
                    cfg.CreateMap<SxForumPart, VMEditForumPart>();
                    cfg.CreateMap<VMEditForumPart, SxForumPart>();

                    //game
                    cfg.CreateMap<Game, VMGame>();
                    cfg.CreateMap<VMGame, Game>();
                    cfg.CreateMap<Game, VMEditGame>();
                    cfg.CreateMap<VMEditGame, Game>();

                    //manuals
                    cfg.CreateMap<SxManual, VMManual>();
                    cfg.CreateMap<SxManual, VMEditManual>();
                    cfg.CreateMap<VMEditManual, SxManual>();

                    //material category
                    cfg.CreateMap<SxMaterialCategory, VMMaterialCategory>();
                    cfg.CreateMap<VMMaterialCategory, SxMaterialCategory>();
                    cfg.CreateMap<SxMaterialCategory, VMEditMaterialCategory>();
                    cfg.CreateMap<VMEditMaterialCategory, SxMaterialCategory>();

                    //material tags
                    cfg.CreateMap<SxMaterialTag, VMMaterialTag>();
                    cfg.CreateMap<SxMaterialTag, VMEditMaterialTag>();
                    cfg.CreateMap<VMEditMaterialTag, SxMaterialTag>();

                    //menu
                    cfg.CreateMap<SxMenu, VMMenu>();
                    cfg.CreateMap<SxMenu, VMEditMenu>();
                    cfg.CreateMap<VMEditMenu, SxMenu>();

                    //menu item
                    cfg.CreateMap<SxMenuItem, VMMenuItem>()
                        .ForMember(d => d.Url, d => d.MapFrom(s => s.Route != null ? s.Route.Url : null));
                    cfg.CreateMap<SxMenuItem, VMEditMenuItem>()
                    .ForMember(d => d.Show, d => d.MapFrom(s => Convert.ToBoolean(s.Show)));
                    cfg.CreateMap<VMEditMenuItem, SxMenuItem>()
                    .ForMember(d => d.Show, d => d.MapFrom(s => Convert.ToByte(s.Show)));

                    //news
                    cfg.CreateMap<News, VMNews>();
                    cfg.CreateMap<News, VMEditNews>();
                    cfg.CreateMap<VMEditNews, News>();

                    //picture
                    cfg.CreateMap<SxPicture, VMPicture>();
                    cfg.CreateMap<SxPicture, VMEditPicture>();
                    cfg.CreateMap<VMEditPicture, SxPicture>();

                    //roles
                    cfg.CreateMap<SxAppRole, VMRole>();
                    cfg.CreateMap<SxAppRole, VMEditRole>();
                    cfg.CreateMap<VMEditRole, SxAppRole>();

                    //route
                    cfg.CreateMap<SxRoute, VMRoute>();
                    cfg.CreateMap<SxRoute, VMEditRoute>();
                    cfg.CreateMap<VMEditRoute, SxRoute>();

                    //redirect
                    cfg.CreateMap<SxRedirect, VMRedirect>();
                    cfg.CreateMap<SxRedirect, VMEditRedirect>();
                    cfg.CreateMap<VMEditRedirect, SxRedirect>();

                    //request
                    cfg.CreateMap<SxRequest, VMRequest>();

                    //route value
                    cfg.CreateMap<SxRouteValue, VMRouteValue>();
                    cfg.CreateMap<SxRouteValue, VMEditRouteValue>();
                    cfg.CreateMap<VMEditRouteValue, SxRouteValue>();

                    //seo info
                    cfg.CreateMap<SxSeoInfo, VMSeoInfo>();
                    cfg.CreateMap<VMSeoInfo, SxSeoInfo>();
                    cfg.CreateMap<SxSeoInfo, VMEditSeoInfo>();
                    cfg.CreateMap<VMEditSeoInfo, SxSeoInfo>();

                    //seo keywords
                    cfg.CreateMap<SxSeoKeyword, VMSeoKeyword>();
                    cfg.CreateMap<SxSeoKeyword, VMEditSeoKeyword>();
                    cfg.CreateMap<VMEditSeoKeyword, SxSeoKeyword>();

                    //users
                    cfg.CreateMap<SxAppUser, VMUser>()
                    .ForMember(d => d.Roles, d => d.MapFrom(s => s.Roles.Select(r=>new VMRole { Id=r.RoleId, Name=r.UserId}).ToArray()));
                    cfg.CreateMap<SxAppUser, VMEditUser>();
                    cfg.CreateMap<VMEditUser, SxAppUser>();
                });
            }
        }
    }
}