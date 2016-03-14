﻿using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GE.WebAdmin.Models
{
    public sealed class VMGame : ISxViewModel<int>
    {
        public int Id { get; set; }
        public DateTime DateCreate { get; set; }
        public string Title { get; set; }
        public string TitleUrl { get; set; }
        public string TitleAbbr { get; set; }
        public string Description { get; set; }
        public bool Show { get; set; }
        public string TitleFull
        {
            get
            {
                return !string.IsNullOrEmpty(TitleAbbr)
                                ? Title + " (" + TitleAbbr + ")"
                                : Title;
            }
        }
    }
}