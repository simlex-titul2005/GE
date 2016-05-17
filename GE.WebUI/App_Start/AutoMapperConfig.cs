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

                    // comments
                    cfg.CreateMap<VMEditComment, SxComment>();

                    // material category
                    cfg.CreateMap<SxMaterialCategory, VMMaterialCategory>();

                    //games
                    cfg.CreateMap<Game, VMGame>();
                    
                    //seo info
                    cfg.CreateMap<SxSeoInfo, VMSeoInfo>();
                });
            }
        }
    }
}