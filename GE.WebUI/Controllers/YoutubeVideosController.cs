﻿using System.Threading.Tasks;
using System.Web.Mvc;
using GE.WebUI.Infrastructure;
using SX.WebCore.MvcControllers.Youtube;
using SX.WebCore;
using SX.WebCore.ViewModels.Youtube;
using System;
using GE.WebUI.Infrastructure.Repositories;
using System.Linq;
using SX.WebCore.ViewModels;

namespace GE.WebUI.Controllers
{
    public sealed class YoutubeVideosController : SxYoutubeVideosController
    {
        private static readonly RepoPopularYoutubeVideo _repoPopularYoutubeVideo = new RepoPopularYoutubeVideo();

        public YoutubeVideosController()
        {
            FillBreadcrumbs = BreadcrumbsManager.WriteBreadcrumbs;
        }

#if !DEBUG
        [OutputCache(Duration =7200)]
#endif
        [HttpGet]
        public override async Task<ActionResult> List(int cat=20, int amount=9)
        {
            var view = await base.List(cat, amount);
            var data = (SxVMVideo[])((view as ViewResult)?.Model);
            await _repoPopularYoutubeVideo.CreateAsync(data);
            return view;
        }

#if !DEBUG
        [OutputCache(Duration =7200, VaryByParam ="amount")]
#endif
        public async Task<ActionResult> PopularForHumor(int amount=20)
        {
            var filter = new SxFilter(1, amount)
            {
                AddintionalInfo = new object[] {
                new SxVMYotubeVideoFilter { CategoryId=23, RegionCode="ru" }
            }
            };

            var viewModel = await Repo.GetPopularAsync(filter); 
            return PartialView("_PopularForHumor", viewModel);
        }

#if !DEBUG
        [OutputCache(Duration =7200)]
#endif
        public async Task<ActionResult> Archive()
        {
            var data = await _repoPopularYoutubeVideo.GetArchiveItemsAsync();
            return PartialView("_Archive", data);
        }

#if !DEBUG
        [OutputCache(Duration =7200, VaryByParam ="year;month;day;amount")]
#endif
        public async Task<ActionResult> ArchiveList(int year, int? month=null, int? day=null, int amount=9)
        {
            if (!Request.IsAjaxRequest()) return new HttpNotFoundResult();

            var filter = new SxFilter(1, amount) { AddintionalInfo = new object[] {
                year,
                month,
                day
            } };

            var data = await _repoPopularYoutubeVideo.GetArchiveList(filter);
            var viewModel = data.Select(x => new SxVMVideo {
                Id=Guid.NewGuid(),
                ChannelId=x.ChannelId,
                ChannelTitle=x.ChannelTitle,
                DateCreate=x.DateCreate,
                Title=x.Title,
                VideoId=x.Id
            }).ToArray();

            return PartialView("_List", viewModel);
        }
    }
}