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
        }
    }
}