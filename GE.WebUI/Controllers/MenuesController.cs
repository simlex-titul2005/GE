using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using GE.WebUI.Models;
using SX.WebCore;
using SX.WebCore.Abstract;
using System.Web.Mvc;
using System.Linq;

namespace GE.WebUI.Controllers
{
    public partial class MenuesController : BaseController
    {
        private SxDbRepository<int, SxMenu, DbContext> _repo;
        public MenuesController()
        {
            _repo = new RepoMenu();
        }

        
        [ChildActionOnly]
        [OutputCache(Duration = 900, VaryByParam = "menuMarker;cssClass")]
        public virtual PartialViewResult Menu(int menuMarker, string cssClass=null)
        {
            ViewBag.MenuCssClass = cssClass;
            var data = _repo.GetByKey(menuMarker);
            if (data == null) return null;

            var viewModel = Mapper.Map<SxMenu, VMMenu>(data);
            viewModel.Items = data.Items.Where(x => x.Show == 1).Select(x => Mapper.Map<SxMenuItem, VMMenuItem>(x)).ToArray();
            return PartialView(MVC.Menues.Views._MenuItems, viewModel);
        }
    }
}