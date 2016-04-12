using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using SX.WebCore.Managers;
using GE.WebAdmin.Models;
using System.Linq;

namespace GE.WebAdmin.Controllers
{
    public partial class AccountController : BaseController
    {
        private SxAppSignInManager _signInManager;
        private SxAppUserManager _userManager;
        public AccountController() { }

        private SxAppSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<SxAppSignInManager>();
            }
            set
            {
                _signInManager = value;
            }
        }

        private SxAppUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<SxAppUserManager>();
            }
            set
            {
                _userManager = value;
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        [AllowAnonymous]
        public virtual ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction(MVC.Home.Index());
            else
                return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public virtual async Task<ActionResult> Login(VMLoginModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            SignInManager.AuthenticationManager.SignOut();
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    var sessionId = Session.SessionID;
                    var usersOnSite = MvcApplication.UsersOnSite;
                    if (!usersOnSite.ContainsKey(sessionId))
                        usersOnSite.Add(sessionId, model.Email);
                    else
                    {
                        if (usersOnSite.ContainsValue(model.Email))
                        {
                            var key = usersOnSite.SingleOrDefault(x => x.Value == model.Email).Key;
                            usersOnSite.Remove(key);
                        }

                        usersOnSite[sessionId] = model.Email;
                    }

                    return RedirectToAction(MVC.Home.Index());
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public virtual ActionResult LogOff()
        {
            //var userManager = new UserManager<SxAppUser>(new UserStore<SxAppUser>(new DbContext()));
            //var email = User.Identity.Name;

            //var onlineUsers = System.Web.HttpContext.Current.Cache.OnlineUsers();
            //var viewedPages = System.Web.HttpRuntime.Cache.ViewedPages();
            //var existOnline = onlineUsers.Where(x => x.Value.UserEmail == email);
            //existOnline.ToList().ForEach((user) =>
            //{
            //    onlineUsers.Remove(user.Key);
            //});
            //viewedPages.Where(x => x.Key == email).ToList()
            //    .ForEach(x =>
            //    {
            //        viewedPages.Remove(x.Key);
            //    });
            //AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult UserList()
        {
            return View();
        }
    }
}