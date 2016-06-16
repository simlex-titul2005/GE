using GE.WebCoreExtantions;
using SX.WebCore.Repositories;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GE.WebUI.Controllers
{
    public partial class BannersController : Controller
    {
        [HttpPost]
        public async virtual Task AddClick(Guid bannerId)
        {
            await Task.Run(() =>
            {
                new SxRepoBanner<DbContext>().AddClick(bannerId);
            });
        }

        [HttpPost]
        public async virtual Task AddShow(Guid bannerId)
        {
            await Task.Run(() =>
            {
                new SxRepoBanner<DbContext>().AddShows(new Guid[] { bannerId });
            });
        }
    }
}