using System;
using System.Collections.Specialized;
using System.Net;
using System.Web.Mvc;

namespace GE.WebUI.Controllers
{
    public partial class ValuteController : Controller
    {
        private static string _url = "http://www.cbr.ru/DailyInfoWebServ/DailyInfo.asmx";

        [HttpGet]
        public virtual ActionResult GetCursOnDate()
        {
            var result = string.Empty;
            using (var client = new WebClient())
            {

                byte[] response =
                client.UploadValues(_url, new NameValueCollection()
                {
                    { "on_date", DateTime.Now.ToString("yyyy-MM-dd")}
                });

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            return new EmptyResult();
        }
    }
}