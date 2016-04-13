using SX.WebCore.ViewModels;
using System.Linq;
using System;

namespace GE.WebAdmin.Controllers
{
    public partial class AccountController : SX.WebCore.Controllers.AccountController
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

        public void RegisterLoginUser(SxVMLogin model)
        {
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
        }
    }
}