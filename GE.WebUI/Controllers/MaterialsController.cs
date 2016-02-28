using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Abstract;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore;
using SX.WebCore.Abstract;
using SX.WebCore.HtmlHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EXT = GE.WebCoreExtantions;

namespace GE.WebUI.Controllers
{
    public abstract partial class MaterialsController<TKey, TModel> : BaseController where TModel: SxDbModel<TKey>, IHasGame
    {
        private SxDbRepository<TKey, TModel, DbContext> _repo;
        protected MaterialsController() { }
        private Enums.ModelCoreType _modelCoreType;
        protected MaterialsController(Enums.ModelCoreType modelCoreType)
        {
            _modelCoreType = modelCoreType;
            switch (_modelCoreType)
            {
                case Enums.ModelCoreType.Article:
                    _repo = new RepoArticle() as SxDbRepository<TKey, TModel, DbContext>;
                    break;
                case Enums.ModelCoreType.News:
                    _repo = new RepoNews() as SxDbRepository<TKey, TModel, DbContext>;
                    break;
                default:
                    throw new NotSupportedException("Не определен репозиторий");
            }
        }
        protected SxDbRepository<TKey, TModel, DbContext> Repository
        {
            get
            {
                return _repo;
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult List(string game = null, int page = 1)
        {
            var pageSize = 10;
            switch(_modelCoreType)
            {
                case Enums.ModelCoreType.Article:
                    pageSize = 10;
                    break;
            }

            var viewModel = new SxExtantions.SxPagedCollection<TModel>();
            var filter = new EXT.Filter { GameTitle = game, PageSize = pageSize };
            viewModel.Collection = _repo.Query(filter).ToArray();
            viewModel.PagerInfo = new SxExtantions.SxPagerInfo
            {
                Page = page,
                PageSize = pageSize,
                PagerSize = 2,
                TotalItems = _repo.Count
            };

            return View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Details(TKey id)
        {
            Response.StatusCode = 404;
            Response.Clear();
            return null;
        }
    }
}