using AutoMapper;
using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GE.WebAdmin
{
    public class AutoMapperConfig
    {
        public static void Configure()
        {
            Mapper.CreateMap<Article, VMArticle>();
            Mapper.CreateMap<Article, VMEditArticle>();
            Mapper.CreateMap<VMEditArticle, Article>();

            Mapper.CreateMap<ArticleType, VMArticleType>();
            Mapper.CreateMap<ArticleType, VMEditArticleType>();
            Mapper.CreateMap<VMEditArticleType, ArticleType>();

            Mapper.CreateMap<Game, VMGame>();
            Mapper.CreateMap<Game, VMEditGame>();
            Mapper.CreateMap<VMEditGame, Game>();
        }
    }
}