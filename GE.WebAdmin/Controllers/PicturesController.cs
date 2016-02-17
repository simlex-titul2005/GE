﻿using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore;
using SX.WebCore.HtmlHelpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GE.WebAdmin.Controllers
{
    public partial class PicturesController : BaseController
    {
        SX.WebCore.Abstract.SxDbRepository<Guid, SxPicture, DbContext> _repo;
        public PicturesController()
        {
            _repo = new RepoPicture();
        }

        private static int _pageSize = 20;

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Index(int page = 1)
        {
            var list = _repo.All
                .Skip((page - 1) * _pageSize)
                .Take(_pageSize).ToArray().Select(x => Mapper.Map<SxPicture, VMPicture>(x)).ToArray();

            ViewData["Page"] = page;
            ViewData["PageSize"] = _pageSize;
            ViewData["RowsCount"] = _repo.All.Count();

            return View(list);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual PartialViewResult Index(VMPicture filter, IDictionary<string, SxExtantions.SortDirection> order, int page = 1)
        {
            int width = filter != null ? filter.Width : 0;
            ViewBag.Filter = filter;
            ViewBag.Order = order;

            //select
            var temp = _repo.All;
            if (width != 0)
                temp = temp.Where(x => x.Width == width);

            //order
            var orders = order.Where(x => x.Value != SxExtantions.SortDirection.Unknown);
            if (orders.Count() != 0)
            {
                foreach (var o in orders)
                {
                    if (o.Key == "Width")
                    {
                        if (o.Value == SxExtantions.SortDirection.Desc)
                            temp = temp.OrderByDescending(x => x.Width);
                        else if (o.Value == SxExtantions.SortDirection.Asc)
                            temp = temp.OrderBy(x => x.Width);
                    }
                }
            }

            var list = temp.Skip((page - 1) * _pageSize)
                .Take(_pageSize).ToArray().Select(x => Mapper.Map<SxPicture, VMPicture>(x)).ToArray();

            ViewData["Page"] = page;
            ViewData["PageSize"] = _pageSize;
            ViewData["RowsCount"] = temp.Count();

            return PartialView("_GridView", list);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Edit(Guid? id)
        {
            var model = id.HasValue ? _repo.GetByKey(id) : new SxPicture ();
            return View(Mapper.Map<SxPicture, VMEditPicture>(model));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(VMEditPicture picture, HttpPostedFileBase file)
        {
            var redactModel=Mapper.Map<VMEditPicture, SxPicture>(picture);
            if(picture.Id==Guid.Empty)
            {
                var image = Image.FromStream(file.InputStream);
                redactModel.OriginalContent = new byte[file.ContentLength];
                file.InputStream.Read(redactModel.OriginalContent, 0, redactModel.OriginalContent.Length);
                redactModel.Width = image.Width;
                redactModel.Height = image.Height;
                _repo.Create(redactModel);
            }
            else
            {
                _repo.Update(redactModel, "Caption", "Description");
            }

            return RedirectToAction(MVC.Pictures.Index());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ViewResult FindTable(int page = 1)
        {
            var list = _repo.All
            .Skip((page - 1) * _pageSize)
            .Take(_pageSize).ToArray().Select(x => Mapper.Map<SxPicture, VMPicture>(x)).ToArray();
            return View(list);
        }
    }
}