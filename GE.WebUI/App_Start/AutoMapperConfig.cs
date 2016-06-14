using AutoMapper;
using GE.WebCoreExtantions;
using GE.WebUI.Models;
using SX.WebCore;
using SX.WebCore.ViewModels;

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
                    //cfg.CreateMap<SxSiteTestQuestion, VMSiteTestQuestion>();
                    //cfg.CreateMap<SxSiteTestBlock, VMSiteTestBlock>();
                    //cfg.CreateMap<SxSiteTest, VMSiteTest>();

                    //seo info
                    cfg.CreateMap<SxAppUser, VMUser>();
                });
            }
        }
    }
}