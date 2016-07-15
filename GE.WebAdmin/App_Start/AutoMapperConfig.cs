using AutoMapper;
using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using SX.WebCore;
using SX.WebCore.ViewModels;
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

                    //author aphorism
                    cfg.CreateMap<AuthorAphorism, VMAuthorAphorism>();
                    cfg.CreateMap<VMAuthorAphorism, AuthorAphorism>();
                    cfg.CreateMap<AuthorAphorism, VMEditAuthorAphorism>();
                    cfg.CreateMap<VMEditAuthorAphorism, AuthorAphorism>();

                    //banned url
                    cfg.CreateMap<SxBannedUrl, SxVMBannedUrl>();
                    cfg.CreateMap<SxBannedUrl, SxVMEditBannedUrl>();
                    cfg.CreateMap<SxVMEditBannedUrl, SxBannedUrl>();

                    //banner
                    cfg.CreateMap<SxBanner, SxVMBanner>();
                    cfg.CreateMap<SxVMBanner, SxBanner>();
                    cfg.CreateMap<SxBanner, SxVMEditBanner>();
                    cfg.CreateMap<SxVMEditBanner, SxBanner>();

                    //banner group
                    cfg.CreateMap<SxBannerGroup, SxVMBannerGroup>();
                    cfg.CreateMap<SxBannerGroup, SxVMEditBannerGroup>();
                    cfg.CreateMap<SxVMEditBannerGroup, SxBannerGroup>();

                    //employee
                    cfg.CreateMap<SxEmployee, SxVMEmployee>();
                    cfg.CreateMap<SxEmployee, SxVMEditEmployee>();
                    cfg.CreateMap<SxVMEditEmployee, SxEmployee>();
                    cfg.CreateMap<SxVMEditAppUser, SxAppUser>();

                    //faq
                    cfg.CreateMap<SxManual, SxVMFAQ>();

                    //game
                    cfg.CreateMap<Game, VMGame>();
                    cfg.CreateMap<VMGame, Game>();
                    cfg.CreateMap<Game, VMEditGame>();
                    cfg.CreateMap<VMEditGame, Game>();

                    //humor
                    cfg.CreateMap<SxHumor, VMHumor>();
                    cfg.CreateMap<SxHumor, VMEditHumor>();
                    cfg.CreateMap<VMEditHumor, SxHumor>();

                    //like buttons
                    cfg.CreateMap<SxShareButton, SxVMShareButton>()
                    .ForMember(d => d.NetName, d => d.MapFrom(s => s.Net==null?null: s.Net.Name));
                    cfg.CreateMap<SxShareButton, SxVMEditShareButton>();
                    cfg.CreateMap<SxVMEditShareButton, SxShareButton>();

                    //manuals
                    cfg.CreateMap<SxManual, VMManual>();
                    cfg.CreateMap<SxManual, VMEditManual>();
                    cfg.CreateMap<VMEditManual, SxManual>();

                    //material category
                    cfg.CreateMap<SxMaterialCategory, VMMaterialCategory>();
                    cfg.CreateMap<VMMaterialCategory, SxMaterialCategory>();
                    cfg.CreateMap<SxMaterialCategory, VMEditMaterialCategory>();
                    cfg.CreateMap<VMEditMaterialCategory, SxMaterialCategory>();

                    cfg.CreateMap<MaterialCategory, VMEditMaterialCategory>();
                    cfg.CreateMap<VMEditMaterialCategory, MaterialCategory>();

                    //material tags
                    cfg.CreateMap<SxMaterialTag, SxVMMaterialTag>();
                    cfg.CreateMap<SxMaterialTag, SxVMEditMaterialTag>();
                    cfg.CreateMap<SxVMEditMaterialTag, SxMaterialTag>();

                    //news
                    cfg.CreateMap<News, VMNews>();
                    cfg.CreateMap<News, VMEditNews>();
                    cfg.CreateMap<VMEditNews, News>();

                    //net
                    cfg.CreateMap<SxNet, SxVMNet>();
                    cfg.CreateMap<SxVMNet, SxNet>();

                    //picture
                    cfg.CreateMap<SxPicture, SxVMPicture>();
                    cfg.CreateMap<SxVMPicture, SxPicture>();
                    cfg.CreateMap<SxPicture, SxVMEditPicture>();
                    cfg.CreateMap<SxVMEditPicture, SxPicture>();

                    //project step
                    cfg.CreateMap<SxProjectStep, SxVMProjectStep>();
                    cfg.CreateMap<SxProjectStep, SxVMEditProjectStep>();
                    cfg.CreateMap<SxVMEditProjectStep, SxProjectStep>();

                    //roles
                    cfg.CreateMap<SxAppRole, SxVMAppRole>();
                    cfg.CreateMap<SxAppRole, SxVMEditAppRole>();
                    cfg.CreateMap<SxVMEditAppRole, SxAppRole>();

                    //redirect
                    cfg.CreateMap<Sx301Redirect, SxVM301Redirect>();
                    cfg.CreateMap<Sx301Redirect, SxVMEdit301Redirect>();
                    cfg.CreateMap<SxVMEdit301Redirect, Sx301Redirect>();

                    //request
                    cfg.CreateMap<SxRequest, SxVMRequest>();

                    //site test
                    cfg.CreateMap<SxSiteTest, SxVMSiteTest>();
                    cfg.CreateMap<SxVMSiteTest, SxSiteTest>();
                    cfg.CreateMap<SxSiteTest, SxVMEditSiteTest>();
                    cfg.CreateMap<SxVMEditSiteTest, SxSiteTest>();

                    //site test question
                    cfg.CreateMap<SxSiteTestQuestion, SxVMSiteTestQuestion>();
                    cfg.CreateMap<SxSiteTestQuestion, SxVMEditSiteTestQuestion>();
                    cfg.CreateMap<SxVMEditSiteTestQuestion, SxSiteTestQuestion>();

                    //site test subject
                    cfg.CreateMap<SxSiteTestSubject, SxVMSiteTestSubject>();
                    cfg.CreateMap<SxSiteTestSubject, SxVMEditSiteTestSubject>();
                    cfg.CreateMap<SxVMEditSiteTestSubject, SxSiteTestSubject>();

                    //seo info
                    cfg.CreateMap<SxSeoTags, SxVMSeoTags>();
                    cfg.CreateMap<SxVMSeoTags, SxSeoTags>();
                    cfg.CreateMap<SxSeoTags, SxVMEditSeoTags>();
                    cfg.CreateMap<SxVMEditSeoTags, SxSeoTags>();

                    //seo keywords
                    cfg.CreateMap<SxSeoKeyword, SxVMSeoKeyword>();
                    cfg.CreateMap<SxSeoKeyword, SxVMEditSeoKeyword>();
                    cfg.CreateMap<SxVMEditSeoKeyword, SxSeoKeyword>();

                    //statistic user login
                    cfg.CreateMap<SxStatisticUserLogin, SxVMStatisticUserLogin>()
                    .ForMember(d => d.DateCreate, d => d.MapFrom(s => s.Statistic.DateCreate))
                    .ForMember(d => d.NikName, d => d.MapFrom(s => s.User.NikName))
                    .ForMember(d => d.AvatarId, d => d.MapFrom(s => s.User.AvatarId));

                    //users
                    cfg.CreateMap<SxAppUser, SxVMAppUser>()
                    .ForMember(d => d.Roles, d => d.MapFrom(s => s.Roles.Select(r=>new SxVMAppRole { Id=r.RoleId, Name=r.UserId}).ToArray()));
                    cfg.CreateMap<SxAppUser, SxVMEditAppUser>();
                    cfg.CreateMap<SxVMAppUser, SxAppUser>();


                    cfg.CreateMap<SxVMEditAppUser, SxEmployee>();

                    //video
                    cfg.CreateMap<SxVideo, SxVMVideo>();
                    cfg.CreateMap<SxVideo, SxVMEditVideo>();
                    cfg.CreateMap<SxVMEditVideo, SxVideo>();
                });
            }
        }
    }
}