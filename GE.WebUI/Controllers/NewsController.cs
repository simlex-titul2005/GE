using AutoMapper;
using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using GE.WebUI.Models;
using SX.WebCore;
using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GE.WebUI.Controllers
{
    public partial class NewsController : BaseController
    {
        private SxDbRepository<int, News, DbContext> _repo;
        public NewsController()
        {
            _repo = new RepoNews();
        }

        [ChildActionOnly]
        public virtual ViewResult LastNewsBlock(int newsCount=5)
        {
            var viewModel = new VMLastNewsBlock();
            viewModel.News = _repo.All.Take(newsCount)
                .Select(x => Mapper.Map<News, VMLastNewsBlockNews>(x))
                .ToArray();

            return viewModel.HasNews ? View(viewModel) : null;
        }
    }
}