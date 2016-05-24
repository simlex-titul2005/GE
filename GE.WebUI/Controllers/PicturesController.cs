using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using System;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.SessionState;

namespace GE.WebUI.Controllers
{
    [SessionState(SessionStateBehavior.Disabled)]
    public partial class PicturesController : Controller
    {
        private static MemoryCache _cache;
        private SxDbRepository<Guid, SxPicture, DbContext> _repo;
        private static CacheItemPolicy _defaultPolicy;
        public PicturesController()
        {
            _repo = new RepoPicture();
            if (_defaultPolicy==null)
                _defaultPolicy= new CacheItemPolicy{ AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(15)};
            if (_cache == null)
                _cache = new MemoryCache("CACHE_PICTURES");
        }

        [HttpGet]
        [OutputCache(Duration = 900, VaryByParam = "id;width;height")]
        public async virtual Task<ActionResult> Picture(Guid id, int? width = null, int? height = null)
        {
            var cacheItem = _cache[getCacheItemKey(id, width, height)];
            if (cacheItem != null)
                return await Task.Run(() =>
                {
                    return new HttpStatusCodeResult(304);
                });

            else
            {
                return await Task.Run(() =>
                {
                    var key = getCacheItemKey(id, width, height);
                    var imgFormat = string.Empty;
                    var isExist = false;
                    var picture = getPicture(id, out imgFormat, out isExist, width, height);

                    if (!isExist)
                    {
                        Response.StatusCode = 404;
                        return null;
                    }

                    _cache.Add(key, picture, _defaultPolicy);
                    return new FileStreamResult(new System.IO.MemoryStream(picture), imgFormat);
                });
            }
        }

        private static string getCacheItemKey(Guid id, int? width = null, int? height = null)
        {
            var sb = new StringBuilder();
            sb.Append("picture_");
            sb.Append(id.ToString().ToLower());
            sb.Append("_");
            if (width.HasValue && !height.HasValue)
                sb.Append("w" + width);
            else if (!width.HasValue && height.HasValue)
                sb.Append("h" + height);
            else if (width.HasValue && height.HasValue)
                sb.Append("w" + width + "_h" + height);

            return sb.ToString();
        }

        private byte[] getPicture(Guid id, out string imgFormat, out bool isExists, int? width = null, int? height = null)
        {
            var data = _repo.GetByKey(id);
            isExists = data != null;
            imgFormat = isExists?data.ImgFormat:null;

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