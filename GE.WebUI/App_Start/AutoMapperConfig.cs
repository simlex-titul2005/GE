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
                    cfg.CreateMap<SxEmployee, SxVMEmployee>();

                    //comments
                    cfg.CreateMap<SxComment, VMComment>();
                    cfg.CreateMap<VMEditComment, SxComment>();
                    
                    //games
                    cfg.CreateMap<Game, VMGame>();
                    cfg.CreateMap<Game, VMDetailGame>();

                    //material category
                    cfg.CreateMap<SxMaterialCategory, VMMaterialCategory>();

                    //seo info
                    cfg.CreateMap<SxSeoTags, SxVMSeoTags>();

                    //site test
                    cfg.CreateMap<SxSiteTestQuestion, SxVMSiteTestQuestion>();
                    cfg.CreateMap<SxVMSiteTestQuestion, SxSiteTestQuestion>();
                    cfg.CreateMap<SxSiteTestBlock, SxVMSiteTestBlock>();
                    cfg.CreateMap<SxVMSiteTestBlock, SxSiteTestBlock>();
                    cfg.CreateMap<SxSiteTest, SxVMSiteTest>();
                    cfg.CreateMap<SxVMSiteTest, SxSiteTest>();
                    cfg.CreateMap<SxSiteTestStep, SxVMSiteTestStep>();
                    cfg.CreateMap<SxVMSiteTestStep, SxSiteTestStep>();

                    //seo info
                    cfg.CreateMap<SxAppUser, VMUser>();
                });
            }
        }
    }
}