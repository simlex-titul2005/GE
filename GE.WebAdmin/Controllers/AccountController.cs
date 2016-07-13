using SX.WebCore.ViewModels;
using System.Linq;
using System;
using GE.WebCoreExtantions;
using Microsoft.AspNet.Identity;
using SX.WebCore.Hubs;
using Newtonsoft.Json;
using SX.WebCore;

namespace GE.WebAdmin.Controllers
{
    public partial class AccountController : SX.WebCore.MvcControllers.SxAccountController<WebCoreExtantions.DbContext>
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
            if (!MvcApplication.UsersOnSite.ContainsKey(sessionId))
                MvcApplication.UsersOnSite.Add(sessionId, model.Email);
            else
            {
                if (MvcApplication.UsersOnSite.ContainsValue(model.Email))
                {
                    var key = MvcApplication.UsersOnSite.SingleOrDefault(x => x.Value == model.Email).Key;
                    MvcApplication.UsersOnSite.Remove(key);
                }

                MvcApplication.UsersOnSite[sessionId] = model.Email;
            }

            addStatisticUserLogin(date, model.Email);
        }

        private void addStatisticUserLogin(DateTime date, string email)
        {
            var user = UserManager.FindByEmail(email);
            new SX.WebCore.Repositories.SxRepoStatistic<DbContext>().CreateStatisticUserLogin(date, user.Id);

            var context = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<SxChatHub>();
            var json = JsonConvert.SerializeObject(Mapper.Map<SxAppUser, SxVMAppUser>(user));
            context.Clients.All.addUserToChatList(json);
        }
    }
}  