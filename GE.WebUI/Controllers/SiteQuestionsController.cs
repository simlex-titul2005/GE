using SX.WebCore.SxManagers;
using SX.WebCore.ViewModels;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

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
            if (ModelState.IsValid)
            {
                var smtpUserName = ConfigurationManager.AppSettings["NoReplyMail"];
                var mm = new SxAppMailManager(smtpUserName, ConfigurationManager.AppSettings["NoReplyMailPassword"], "mail.game-exe.com");

                var sb = new StringBuilder();
                sb.AppendLine(model.Email);
                sb.AppendLine(model.UserName);
                sb.AppendLine(model.Text);

                var result= await mm.SendMail(model.Email, sb.ToString(), new string[] { "admin@game-exe.com", "architect@game-exe.com" }, "Обращение с формы обратной связи");

                TempData["Message"] = "Ваше письмо успешно отправлено";
                return View(model: new SxVMSiteQuestion());
            }
            return View(model);
        }
    }
}