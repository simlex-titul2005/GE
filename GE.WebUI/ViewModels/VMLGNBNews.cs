﻿using SX.WebCore.ViewModels;
using System.Web.Mvc;

namespace GE.WebUI.ViewModels
{
    public class VMLGNBNews: SxVMMaterial
    {
        public override string GetUrl(UrlHelper urlHelper)
        {
            return urlHelper.Action("Details", "News", new { year = DateCreate.Year, month = DateCreate.Month.ToString("00"), day = DateCreate.Day.ToString("00"), titleUrl = TitleUrl });
        }

        public int? GameId { get; set; }
    }
}