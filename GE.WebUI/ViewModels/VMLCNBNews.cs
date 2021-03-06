﻿using System.Web.Mvc;
using SX.WebCore.ViewModels;

namespace GE.WebUI.ViewModels
{
    public sealed class VMLCNBNews : SxVMMaterial
    {
        public new string CategoryId { get; set; }
        public new VMLCNBCategory Category { get; set; }
        public override string GetUrl(UrlHelper url)
        {
            return url.Action("Details", "News", new { year = DateCreate.Year, month = DateCreate.Month.ToString("00"), day = DateCreate.Day.ToString("00"), titleUrl = TitleUrl });
        }
    }
}