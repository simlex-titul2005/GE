using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.SessionState;

namespace GE.WebUI.Controllers
{
    [SessionState(SessionStateBehavior.Disabled)]
    public partial class PicturesController : Controller
    {
        private SxDbRepository<Guid, SxPicture, DbContext> _repo;
        public PicturesController()
        {
            _repo = new RepoPicture();
        }

        [HttpGet]
        [OutputCache(Duration = 900, VaryByParam = "id;width;height")]
        public async virtual Task<ActionResult> Picture(Guid id, int? width = null, int? height = null)
        {
            return await Task.Run(() =>
            {
                var imgFormat = string.Empty;
                var isExist = false;
                var picture = getPicture(id, out imgFormat, out isExist, width, height);

                if (!isExist)
                {
                    Response.StatusCode = 404;
                    return null;
                }

                return new FileStreamResult(new System.IO.MemoryStream(picture), imgFormat);
            });
        }

        private byte[] getPicture(Guid id, out string imgFormat, out bool isExists, int? width = null, int? height = null)
        {
            var data = _repo.GetByKey(id);
            isExists = data != null;
            imgFormat = isExists ? data.ImgFormat : null;

            if (!isExists) return new byte[0];


            byte[] byteArray = data.OriginalContent;
            if (width.HasValue && data.Width > width)
            {
                byteArray = PictureProvider.ScaleImage(data.OriginalContent, PictureProvider.ImageScaleMode.Width, destWidth: width);
            }
            else if (height.HasValue && data.Height > height)
            {
                byteArray = PictureProvider.ScaleImage(data.OriginalContent, PictureProvider.ImageScaleMode.Height, destHeight: height);
            }

            return byteArray;
        }
    }
}