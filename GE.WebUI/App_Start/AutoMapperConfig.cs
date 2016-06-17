using AutoMapper;
using GE.WebCoreExtantions;
using GE.WebUI.Models;
using SX.WebCore;

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
                    //aphorism
                    cfg.CreateMap<Aphorism, VMAphorism>();

                    //author aphorism
                    cfg.CreateMap<AuthorAphorism, VMAuthorAphorism>();

                    //articles
                    cfg.CreateMap<Article, VMDetailArticle>();

                    //employees
                    cfg.CreateMap<SxEmployee, VMEmployee>();

                    //articles
                    cfg.CreateMap<SxForumPart, VMForumPart>();

                    //comments
                    cfg.CreateMap<SxComment, VMComment>();
                    cfg.CreateMap<VMEditComment, SxComment>();
                    
                    //games
                    cfg.CreateMap<Game, VMGame>();
                    cfg.CreateMap<Game, VMDetailGame>();

                    //material category
                    cfg.CreateMap<SxMaterialCategory, VMMaterialCategory>();

                    //seo info
                    cfg.CreateMap<SxSeoInfo, VMSeoInfo>();

                    //site test
                    cfg.CreateMap<SxSiteTestQuestion, VMSiteTestQuestion>();
                    cfg.CreateMap<VMSiteTestQuestion, SxSiteTestQuestion>();
                    cfg.CreateMap<SxSiteTestBlock, VMSiteTestBlock>();
                    cfg.CreateMap<VMSiteTestBlock, SxSiteTestBlock>();
                    cfg.CreateMap<SxSiteTest, VMSiteTest>();
                    cfg.CreateMap<VMSiteTest, SxSiteTest>();
                    cfg.CreateMap<SxSiteTestStep, VMSiteTestStep>();
                    cfg.CreateMap<VMSiteTestStep, SxSiteTestStep>();

                    //seo info
                    cfg.CreateMap<SxAppUser, VMUser>();
                });
            }
        }
    }
}