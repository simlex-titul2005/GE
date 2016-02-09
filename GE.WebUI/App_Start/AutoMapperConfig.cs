﻿using AutoMapper;
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
                    //menu
                    cfg.CreateMap<SxMenu, VMMenu>();

                    //menu item
                    cfg.CreateMap<SxMenuItem, VMMenuItem>()
                        .ForMember(d => d.Url, d => d.MapFrom(s => s.Route != null ? s.Route.Url.ToLower() : null));
                });
            }
        }
    }
}