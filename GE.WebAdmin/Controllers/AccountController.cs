using SX.WebCore.ViewModels;
using System.Linq;
using System;
using GE.WebCoreExtantions;
using System.Threading.Tasks;

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

        private async void RegisterLoginUser(SxVMLogin model)
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

            await addStatisticUserLogin(date, model.Email);
        }

        private async Task addStatisticUserLogin(DateTime date, string email)
        {
            var user = await UserManager.FindByEmailAsync(email);
            new SX.WebCore.Repositories.RepoStatistic<DbContext>().CreateStatisticUserLogin(date, user.Id);
        }
    }
}  