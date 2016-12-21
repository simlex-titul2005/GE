using AutoMapper;
using GE.WebUI.Models;
using GE.WebUI.ViewModels;

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

            //humor
            cfg.CreateMap<Humor, VMHumor>();
            cfg.CreateMap<VMHumor, Humor>();

            //material category
            cfg.CreateMap<MaterialCategory, VMMaterialCategory>();
            cfg.CreateMap<VMMaterialCategory, MaterialCategory>()
                .ForMember(d => d.ParentId, d => d.MapFrom(s => (string)s.ParentId));

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

            //site test setting
            cfg.CreateMap<SiteTestSetting, VMSiteTestSetting>();
            cfg.CreateMap<VMSiteTestSetting, SiteTestSetting>();

            //site test subject
            cfg.CreateMap<SiteTestSubject, VMSiteTestSubject>();
            cfg.CreateMap<VMSiteTestSubject, SiteTestSubject>();
        }
    }
}