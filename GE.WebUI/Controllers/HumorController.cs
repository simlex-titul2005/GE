using GE.WebCoreExtantions.Repositories;
using SX.WebCore;

namespace GE.WebUI.Controllers
{
    public sealed class HumorController : MaterialsController<SxHumor>
    {
        public HumorController() : base(Enums.ModelCoreType.Humor) {
            if (Repo == null)
                Repo = new RepoHumor();
        }
    }
}