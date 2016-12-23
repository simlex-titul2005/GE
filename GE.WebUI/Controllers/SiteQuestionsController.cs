using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using SX.WebCore.SxManagers;
using SX.WebCore.ViewModels;

namespace GE.WebUI.Controllers
{
    public sealed class SiteQuestionsController : BaseController
    {
        [HttpGet]
        public ActionResult Edit()
        {
            var viewModel = new SxVMSiteQuestion();
            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ViewResult> Edit(SxVMSiteQuestion model)
        {
            if (!ModelState.IsValid) return View(model);

            var mm = new SxAppMailManager();

            var sb = new StringBuilder();
            sb.AppendLine(model.Email);
            sb.AppendLine(model.UserName);
            sb.AppendLine(model.Text);

            var result= await mm.SendMail(sb.ToString(), new[] {
                "admin@game-exe.com", 
                "architect@game-exe.com"
            }, "Обращение с формы обратной связи");

            var mes=new SxVMResultMessage("Ваше письмо успешно отправлено", SxVMResultMessage.ResultMessageType.Ok);
            if (!result)
            {
                mes.Message = "Ошибка отправки сообщения. Попробуйте еще раз";
                mes.MessageType = SxVMResultMessage.ResultMessageType.Error;
            }
            ViewBag.Message = mes;
;
            return View(model: new SxVMSiteQuestion());
        }
    }
}