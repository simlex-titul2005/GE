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

                    //articles
                    cfg.CreateMap<SxForumPart, VMForumPart>();

                    //comments
                    cfg.CreateMap<SxComment, VMComment>();
                    cfg.CreateMap<VMEditComment, SxComment>();
                    
                    //games
                    cfg.CreateMap<Game, VMGame>();

                    //material category
                    cfg.CreateMap<SxMaterialCategory, VMMaterialCategory>();

                    //seo info
                    cfg.CreateMap<SxSeoInfo, VMSeoInfo>();

                    //seo info
                    cfg.CreateMap<SxAppUser, VMUser>();
                });
            }
        }
    }
}