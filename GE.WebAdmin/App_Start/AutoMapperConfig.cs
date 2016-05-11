using AutoMapper;
using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using SX.WebCore;
using System.Linq;

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

                    //aphorism
                    cfg.CreateMap<Aphorism, VMAphorism>();
                    cfg.CreateMap<Aphorism, VMEditAphorism>();
                    cfg.CreateMap<VMEditAphorism, Aphorism>();

                    //banned url
                    cfg.CreateMap<SxBannedUrl, VMBannedUrl>();
                    cfg.CreateMap<SxBannedUrl, VMEditBannedUrl>();
                    cfg.CreateMap<VMEditBannedUrl, SxBannedUrl>();

                    //banner
                    cfg.CreateMap<SxBanner, VMBanner>();
                    cfg.CreateMap<SxBanner, VMEditBanner>();
                    cfg.CreateMap<VMEditBanner, SxBanner>();

                    //banner group
                    cfg.CreateMap<SxBannerGroup, VMBannerGroup>();
                    cfg.CreateMap<SxBannerGroup, VMEditBannerGroup>();
                    cfg.CreateMap<VMEditBannerGroup, SxBannerGroup>();

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

                    //news
                    cfg.CreateMap<News, VMNews>();
                    cfg.CreateMap<News, VMEditNews>();
                    cfg.CreateMap<VMEditNews, News>();

                    //picture
                    cfg.CreateMap<SxPicture, VMPicture>();
                    cfg.CreateMap<SxPicture, VMEditPicture>();
                    cfg.CreateMap<VMEditPicture, SxPicture>();

                    //project step
                    cfg.CreateMap<SxProjectStep, VMProjectStep>();
                    cfg.CreateMap<SxProjectStep, VMEditProjectStep>();
                    cfg.CreateMap<VMEditProjectStep, SxProjectStep>();

                    //roles
                    cfg.CreateMap<SxAppRole, VMRole>();
                    cfg.CreateMap<SxAppRole, VMEditRole>();
                    cfg.CreateMap<VMEditRole, SxAppRole>();

                    //redirect
                    cfg.CreateMap<SxRedirect, VMRedirect>();
                    cfg.CreateMap<SxRedirect, VMEditRedirect>();
                    cfg.CreateMap<VMEditRedirect, SxRedirect>();

                    //request
                    cfg.CreateMap<SxRequest, VMRequest>();

                    //seo info
                    cfg.CreateMap<SxSeoInfo, VMSeoInfo>();
                    cfg.CreateMap<VMSeoInfo, SxSeoInfo>();
                    cfg.CreateMap<SxSeoInfo, VMEditSeoInfo>();
                    cfg.CreateMap<VMEditSeoInfo, SxSeoInfo>();

                    //seo keywords
                    cfg.CreateMap<SxSeoKeyword, VMSeoKeyword>();
                    cfg.CreateMap<SxSeoKeyword, VMEditSeoKeyword>();
                    cfg.CreateMap<VMEditSeoKeyword, SxSeoKeyword>();

                    //statistic user login
                    cfg.CreateMap<SxStatisticUserLogin, VMStatisticUserLogin>()
                    .ForMember(d => d.DateCreate, d => d.MapFrom(s => s.Statistic.DateCreate))
                    .ForMember(d => d.NikName, d => d.MapFrom(s => s.User.NikName))
                    .ForMember(d => d.AvatarId, d => d.MapFrom(s => s.User.AvatarId));

                    //users
                    cfg.CreateMap<SxAppUser, VMUser>()
                    .ForMember(d => d.Roles, d => d.MapFrom(s => s.Roles.Select(r=>new VMRole { Id=r.RoleId, Name=r.UserId}).ToArray()));
                    cfg.CreateMap<SxAppUser, VMEditUser>();
                    cfg.CreateMap<VMEditUser, SxAppUser>();

                    //video
                    cfg.CreateMap<SxVideo, VMVideo>();
                    cfg.CreateMap<SxVideo, VMEditVideo>();
                    cfg.CreateMap<VMEditVideo, SxVideo>();
                });
            }
        }
    }
}