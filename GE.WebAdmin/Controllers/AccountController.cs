using SX.WebCore.ViewModels;
using System.Linq;
using System;
using GE.WebCoreExtantions;
using Microsoft.AspNet.Identity;
using SX.WebCore.MvcControllers;

namespace GE.WebAdmin.Controllers
{
    public partial class AccountController : SxAccountController<DbContext>
    {
        protected override Action<SxVMLogin> ActionLogin
        {
            get
            {
                return RegisterLoginUser;
            }
        }

        protected override Action ActionLogOff
        {
            get
            {
                return UnregisterLoginUser;
            }
        }

        public void UnregisterLoginUser()
        {
            var sessionId = Session.SessionID;
            var usersOnSite = MvcApplication.UsersOnSite;
            if (usersOnSite.ContainsKey(sessionId))
                usersOnSite.Remove(sessionId);
        }

        private void RegisterLoginUser(SxVMLogin model)
        {
            var date = DateTime.Now;
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

            addStatisticUserLogin(date, model.Email);
        }

        private void addStatisticUserLogin(DateTime date, string email)
        {
            var user = UserManager.FindByEmail(email);
            new SX.WebCore.Repositories.SxRepoStatistic<DbContext>().CreateStatisticUserLogin(date, user.Id);
        }
    }
}  