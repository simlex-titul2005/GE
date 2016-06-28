using GE.WebUI.Models;
using SX.WebCore.Managers;
using System.Configuration;
using System.Text;
using System.Web.Mvc;

namespace GE.WebUI.Controllers
{
    public sealed class SiteQuetionsController : BaseController
    {
        [HttpGet]
        public ActionResult Edit()
        {
            var viewModel = new VMEditSiteQuetion();
            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ViewResult Edit(VMEditSiteQuetion model)
        {
            if (ModelState.IsValid)
            {
                var smtpUserName = ConfigurationManager.AppSettings["NoReplyMail"];
                var mm = new SxAppMailManager(smtpUserName, ConfigurationManager.AppSettings["NoReplyMailPassword"], "mail.game-exe.com");

                var sb = new StringBuilder();
                sb.AppendLine(model.UserName);
                sb.AppendLine(model.Text);

                mm.SendMail(model.Email, sb.ToString(), new string[] { "admin@game-exe.com", "architect@game-exe.com" }, "Обращение с формы обратной связи");

                TempData["Message"] = "Ваше письмо успешно отправлено";
                return View(model: new VMEditSiteQuetion());
            }
            return View(model);
        }
    }
}