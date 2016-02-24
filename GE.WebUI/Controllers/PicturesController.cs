using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore;
using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GE.WebUI.Controllers
{
    public partial class PicturesController : Controller
    {
        private SxDbRepository<Guid, SxPicture, DbContext> _repo;
        public PicturesController()
        {
            _repo = new RepoPicture();
        }

        [AcceptVerbs(HttpVerbs.Get)]
//#if !DEBUG
        [OutputCache(Duration = 300, VaryByParam = "id;width;height")]
//#endif
        public virtual FileResult Picture(Guid id, int? width = null, int? height=null)
        {
            var viewModel = _repo.GetByKey(id);
            byte[] byteArray = viewModel.OriginalContent;
            if (width.HasValue && viewModel.Width > width)
            {
                byteArray = SX.WebCore.PictureHandler.ScaleImage(viewModel.OriginalContent, SX.WebCore.PictureHandler.ImageScaleMode.Width, destWidth: width);
            }
            else if(height.HasValue && viewModel.Height>height)
            {
                byteArray = SX.WebCore.PictureHandler.ScaleImage(viewModel.OriginalContent, SX.WebCore.PictureHandler.ImageScaleMode.Height, destHeight: height);
            }

            return new FileStreamResult(new System.IO.MemoryStream(byteArray), viewModel.ImgFormat);
        }
    }
}