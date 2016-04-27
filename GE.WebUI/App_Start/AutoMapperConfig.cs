using AutoMapper;
using GE.WebCoreExtantions;
using GE.WebUI.Models;
using SX.WebCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
                    //articles
                    cfg.CreateMap<Article, VMDetailArticle>();

                    //articles
                    cfg.CreateMap<SxForumPart, VMForumPart>();

                    // comments
                    cfg.CreateMap<VMEditComment, SxComment>();

                    //games
                    cfg.CreateMap<Game, VMGame>();
                    
                    //seo info
                    cfg.CreateMap<SxSeoInfo, VMSeoInfo>();
                });
            }
        }
    }
}