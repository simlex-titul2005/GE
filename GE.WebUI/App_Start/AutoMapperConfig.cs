using AutoMapper;
using GE.WebUI.Models;
using GE.WebUI.ViewModels;
using SX.WebCore;
using SX.WebCore.ViewModels;

namespace GE.WebUI
{
    public class AutoMapperConfig
    {
        public static void Register(IMapperConfigurationExpression cfg)
        {
            //aphorism
            cfg.CreateMap<Aphorism, VMAphorism>();
            cfg.CreateMap<VMAphorism, Aphorism>();

            //article
            cfg.CreateMap<Article, VMArticle>();
            cfg.CreateMap<VMArticle, Article>();

            //author aphorism
            cfg.CreateMap<AuthorAphorism, VMAuthorAphorism>();
            cfg.CreateMap<VMAuthorAphorism, AuthorAphorism>();

            //game
            cfg.CreateMap<Game, VMGame>();
            cfg.CreateMap<VMGame, Game>();

            //material category
            cfg.CreateMap<SxMaterialCategory, VMMaterialCategory>();
            cfg.CreateMap<SxVMMaterialCategory, VMMaterialCategory>();
            cfg.CreateMap<VMMaterialCategory, SxMaterialCategory>()
                .ForMember(d => d.ParentId, d => d.MapFrom(s => s.ParentId == null ? null : s.ParentId.ToString()));

            //news
            cfg.CreateMap<News, VMNews>();
            cfg.CreateMap<VMNews, News>();

            //site test
            cfg.CreateMap<SiteTest, VMSiteTest>();
            cfg.CreateMap<VMSiteTest, SiteTest>();

            //site test answer
            cfg.CreateMap<SiteTestAnswer, VMSiteTestAnswer>();
            cfg.CreateMap<VMSiteTestAnswer, SiteTestAnswer>();

            //site test question
            cfg.CreateMap<SiteTestQuestion, VMSiteTestQuestion>();
            cfg.CreateMap<VMSiteTestQuestion, SiteTestQuestion>();

            //site test subject
            cfg.CreateMap<SiteTestSubject, VMSiteTestSubject>();
            cfg.CreateMap<VMSiteTestSubject, SiteTestSubject>();
        }

        //public static MapperConfiguration MapperConfigurationInstance
        //{
        //    get
        //    {
        //        return new MapperConfiguration(cfg =>
        //        {
        //            ////aphorism
        //            //cfg.CreateMap<Aphorism, VMAphorism>();

        //            ////author aphorism
        //            //cfg.CreateMap<AuthorAphorism, VMAuthorAphorism>();

        //            ////articles
        //            //cfg.CreateMap<Article, VMMaterial>()
        //            //    .ForMember(d => d.Videos, d => d.MapFrom(s => s.VideoLinks.Select(x => x.Video)));

        //            ////employees
        //            //cfg.CreateMap<SxEmployee, SxVMEmployee>();

        //            ////comments
        //            //cfg.CreateMap<SxComment, VMComment>();
        //            //cfg.CreateMap<VMEditComment, SxComment>();

        //            ////games
        //            //cfg.CreateMap<Game, VMGame>();
        //            //cfg.CreateMap<Game, VMDetailGame>();

        //            ////humor
        //            //cfg.CreateMap<SxHumor, VMMaterial>()
        //            //    .ForMember(d => d.Videos, d => d.MapFrom(s => s.VideoLinks.Select(x => x.Video)))
        //            //    .ForMember(d => d.User, d => d.MapFrom(s => s.User!=null
        //            //        ? new SxVMAppUser {
        //            //            Id=s.User.Id,
        //            //            NikName=s.User.NikName
        //            //        }
        //            //        : new SxVMAppUser {
        //            //            Id=null,
        //            //            NikName=s.UserName
        //            //        }));

        //            ////material category
        //            //cfg.CreateMap<SxMaterialCategory, SxVMMaterialCategory>();

        //            ////picture
        //            //cfg.CreateMap<SxPicture, SxVMPicture>();

        //            ////news
        //            //cfg.CreateMap<News, VMMaterial>();

        //            ////seo info
        //            //cfg.CreateMap<SxSeoTags, SxVMSeoTags>();

        //            ////site test
        //            //cfg.CreateMap<SiteTest, VMSiteTest>();
        //            //cfg.CreateMap<VMSiteTest, SiteTest>();
        //            //cfg.CreateMap<SiteTestQuestion, VMSiteTestQuestion>();
        //            //cfg.CreateMap<VMSiteTestQuestion, SiteTestQuestion>();
        //            //cfg.CreateMap<SiteTestSubject, VMSiteTestSubject>();
        //            //cfg.CreateMap<SiteTestAnswer, VMSiteTestAnswer>();

        //            ////seo info
        //            //cfg.CreateMap<SxAppUser, VMUser>();

        //            ////seo keywords
        //            //cfg.CreateMap<SxSeoKeyword, SxVMSeoKeyword>();
        //            //cfg.CreateMap<SxSeoKeyword, SxVMEditSeoKeyword>();
        //            //cfg.CreateMap<SxVMEditSeoKeyword, SxSeoKeyword>();

        //            ////users
        //            //cfg.CreateMap<SxAppUser, SxVMAppUser>()
        //            //.ForMember(d => d.Roles, d => d.MapFrom(s => s.Roles.Select(r => new SxVMAppRole { Id = r.RoleId, Name = r.UserId }).ToArray()));
        //            //cfg.CreateMap<SxAppUser, SxVMEditAppUser>();
        //            //cfg.CreateMap<SxVMAppUser, SxAppUser>();
        //        });
        //    }
        //}
    }
}