using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore;
using SX.WebCore.HtmlHelpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.Mvc;
using GE.WebAdmin.Extantions.Repositories;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;

namespace GE.WebAdmin.Controllers
{
    public partial class PicturesController : BaseController
    {
        private SxDbRepository<Guid, SxPicture, DbContext> _repo;
        public PicturesController()
        {
            _repo = new RepoPicture();
        }

        private static int _pageSize = 20;

        [Authorize(Roles = "photo-redactor")]
        [HttpGet]
        public virtual ViewResult Index(int page = 1)
        {
            var filter = new WebCoreExtantions.Filter(page, _pageSize);
            var totalItems = (_repo as RepoPicture).FilterCount(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = (_repo as RepoPicture).QueryForAdmin(filter);
            return View(viewModel);
        }

        [Authorize(Roles = "photo-redactor")]
        [HttpPost]
        public virtual PartialViewResult Index(VMPicture filterModel, IDictionary<string, SxExtantions.SortDirection> order, int page = 1)
        {
            ViewBag.Filter = filterModel;
            ViewBag.Order = order;

            var filter = new WebCoreExtantions.Filter(page, _pageSize) { Orders = order, WhereExpressionObject = filterModel };
            var totalItems = (_repo as RepoPicture).FilterCount(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = (_repo as RepoPicture).QueryForAdmin(filter);

            return PartialView("_GridView", viewModel);
        }

        [Authorize(Roles = "photo-redactor")]
        [HttpGet]
        public virtual ViewResult Edit(Guid? id)
        {
            var model = id.HasValue ? _repo.GetByKey(id) : new SxPicture();
            return View(Mapper.Map<SxPicture, VMEditPicture>(model));
        }

        private static int maxSize = 307200;
        [Authorize(Roles = "photo-redactor")]
        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Edit(VMEditPicture picture, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                var isNew = picture.Id == Guid.Empty;
                var redactModel = Mapper.Map<VMEditPicture, SxPicture>(picture);
                if (isNew)
                {
                    redactModel = getImage(redactModel, file);
                    if(redactModel.Size> maxSize)
                    {
                        ModelState.AddModelError("Title", string.Format("Размер файла не должен превышать {0} kB", maxSize));
                        return View(picture);
                    }
                    _repo.Create(redactModel);
                }
                else
                {
                    if (file != null)
                    {
                        redactModel = getImage(redactModel, file);
                        if (redactModel.Size > maxSize)
                        {
                            ModelState.AddModelError("Title", string.Format("Размер файла не должен превышать {0} kB", maxSize));
                            return View(picture);
                        }
                        _repo.Update(redactModel, true, "Caption", "Description", "OriginalContent", "Width", "Height", "Size");
                    }
                    else
                    {
                        _repo.Update(redactModel, true, "Caption", "Description");
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
            var totalItems = (_repo as RepoPicture).FilterCount(filter);
            filter.PagerInfo.TotalItems = totalItems;
            filter.PagerInfo.PagerSize = 5;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = (_repo as RepoPicture).QueryForAdmin(filter);

            return PartialView(MVC.Pictures.Views._FindGridView, viewModel);
        }

        [HttpGet]
        #if !DEBUG
        [OutputCache(Duration = 300, VaryByParam = "id;width;height")]
        #endif
        public virtual FileResult Picture(Guid id, int? width = null, int? height = null)
        {
            if (_repo == null)
                _repo = new RepoPicture();

            var viewModel = _repo.GetByKey(id);
            byte[] byteArray = viewModel.OriginalContent;
            if (width.HasValue && viewModel.Width > width)
            {
                byteArray = PictureProvider.ScaleImage(viewModel.OriginalContent, PictureProvider.ImageScaleMode.Width, destWidth: width);
            }
            else if (height.HasValue && viewModel.Height > height)
            {
                byteArray = PictureProvider.ScaleImage(viewModel.OriginalContent, PictureProvider.ImageScaleMode.Height, destHeight: height);
            }

            return new FileStreamResult(new System.IO.MemoryStream(byteArray), viewModel.ImgFormat);
        }

        [Authorize(Roles = "photo-redactor")]
        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Delete(Guid id)
        {
            _repo.Delete(id);
            return RedirectToAction(MVC.Pictures.Index());
        }
    }
}