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
    public partial class NewsController : MaterialsController<int, News>
    {
        public NewsController() : base(Enums.ModelCoreType.News) { }

        [ChildActionOnly]
        public virtual PartialViewResult LastNewsBlock(int amount = 5)
        {
            var viewModel = (base.Repository as RepoNews).LastNewsBlock(amount);
            return viewModel.HasNews ? PartialView(MVC.News.Views._LastNewsBlock, viewModel) : null;
        }
    }
}