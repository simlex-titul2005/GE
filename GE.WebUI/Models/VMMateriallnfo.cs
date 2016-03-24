﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GE.WebUI.Models
{
    public sealed class VMMateriallnfo
    {
        public DateTime DateOfPublication  { get; set; }
        public int? ViewsCount { get; set; }
        public int? CommentsCount { get; set; }
        public int LikeUpCount { get; set; }
        public int LikeDownCount { get; set; }
    }
}