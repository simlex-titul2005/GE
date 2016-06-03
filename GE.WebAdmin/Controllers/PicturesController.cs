using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using SX.WebCore;
using SX.WebCore.HtmlHelpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.Mvc;
using GE.WebAdmin.Extantions.Repositories;
using System.Linq;
using SX.WebCore.Repositories;
using AutoMapper;

namespace GE.WebAdmin.Controllers
{
    public partial class PicturesController : SX.WebCore.Controllers.SxPicturesController<DbContext>
    {
        private IMapper _mapper;
        public PicturesController()
        {
            _mapper = MvcApplication.MapperConfiguration.CreateMapper();
        }

        protected IMapper Mapper
        {
            get
            {
                return _mapper;
            }
        }

        private static int _pageSize = 20;

        [Authorize(Roles = "photo-redactor")]
        [HttpGet]
        public virtual ViewResult Index(int page = 1)
        {
            var filter = new WebCoreExtantions.Filter(page, _pageSize);
            var totalItems = (Repo as RepoPicture<DbContext>).FilterCount(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = (Repo as RepoPicture<DbContext>).QueryForAdmin(filter);
            return View(viewModel);
        }

        [Authorize(Roles = "photo-redactor")]
        [HttpPost]
        public virtual PartialViewResult Index(VMPicture filterModel, IDictionary<string, SxExtantions.SortDirection> order, int page = 1)
        {
            ViewBag.Filter = filterModel;
            ViewBag.Order = order;

            var filter = new WebCoreExtantions.Filter(page, _pageSize) { Orders = order, WhereExpressionObject = filterModel };
            var totalItems = (Repo as RepoPicture<DbContext>).FilterCount(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = (Repo as RepoPicture<DbContext>).QueryForAdmin(filter);

            return PartialView("_GridView", viewModel);
        }

        [Authorize(Roles = "photo-redactor")]
        [HttpGet]
        public virtual ViewResult Edit(Guid? id)
        {
            var model = id.HasValue ? Repo.GetByKey(id) : new SxPicture();
            return View(Mapper.Map<SxPicture, VMEditPicture>(model));
        }

        private static int maxSize = 307200;
        [Authorize(Roles = "photo-redactor")]
        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Edit(VMEditPicture picture, HttpPostedFileBase file)
        {
            var allowFormats = new string[] {
                "image/jpeg",
                "image/png",
                "image/gif"
            };

            if(file!=null && file.ContentLength > maxSize)
                ModelState.AddModelError("Caption", string.Format("Размер файла не должен превышать {0} kB", maxSize/1024));
            if(file!=null && !allowFormats.Contains(file.ContentType))
                ModelState.AddModelError("Caption", string.Format("Недопустимый формат файла {0}", file.ContentType));

            if (ModelState.IsValid)
            {
                var isNew = picture.Id == Guid.Empty;
                var redactModel = Mapper.Map<VMEditPicture, SxPicture>(picture);
                if (isNew)
                {
                    redactModel = getImage(redactModel, file);
                    Repo.Create(redactModel);
                }
                else
                {
                    if (file != null)
                    {
                        redactModel = getImage(redactModel, file);
                        Repo.Update(redactModel, true, "Caption", "Description", "OriginalContent", "Width", "Height", "Size", "ImgFormat");
                    }
                    else
                    {
                        Repo.Update(redactModel, true, "Caption", "Description");
                    }
                }
                return RedirectToAction(MVC.Pictures.Index());
            }

            return View(picture);
        }

        private static SxPicture getImage(SxPicture redactModel, HttpPostedFileBase file)
        {
            byte[] imageData = null;
            Image image = null;
            using (var binaryReader = new BinaryReader(file.InputStream))
            {
                imageData = binaryReader.ReadBytes(file.ContentLength);
                image = Image.FromStream(file.InputStream);
            }
            redactModel.OriginalContent = imageData;
            redactModel.Width = image.Width;
            redactModel.Height = image.Height;
            redactModel.Size = imageData.Length;

            return redactModel;
        }

        [Authorize(Roles = "photo-redactor")]
        [HttpPost]
        public virtual PartialViewResult FindGridView(VMPicture filterModel, int page = 1, int pageSize=10)
        {
            ViewBag.Filter = filterModel;
            var filter = new WebCoreExtantions.Filter(page, pageSize);
            filter.WhereExpressionObject = filterModel;
            var totalItems = (Repo as RepoPicture<DbContext>).FilterCount(filter);
            filter.PagerInfo.TotalItems = totalItems;
            filter.PagerInfo.PagerSize = 5;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = (Repo as RepoPicture<DbContext>).QueryForAdmin(filter);

            return PartialView(MVC.Pictures.Views._FindGridView, viewModel);
        }

        [Authorize(Roles = "photo-redactor")]
        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Delete(Guid id)
        {
            Repo.Delete(id);
            return RedirectToAction(MVC.Pictures.Index());
        }
    }
}