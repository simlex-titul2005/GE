using AutoMapper;
using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using GE.WebUI.Models;
using SX.WebCore;
using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GE.WebUI.Extantions.Repositories;

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
        public virtual ViewResult LastNewsBlock(int amount = 5)
        {
            var viewModel = (_repo as RepoNews).LastNewsBlock(amount);
            return viewModel.HasNews ? View(viewModel) : null;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult List(int page)
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Details(int id)
        {
            return View();
        }
    }
}