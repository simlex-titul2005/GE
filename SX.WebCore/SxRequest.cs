using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SX.WebCore
{
    [Table("D_REQUEST")]
    public sealed class SxRequest : SxDbUpdatedModel<Guid>
    {
        public SxRequest(HttpRequestBase request)
        {
            UrlRef = request.UrlReferrer != null ? request.UrlReferrer.AbsolutePath : null;
            Controller = request.RequestContext.RouteData.Values["controller"].ToString();
            Action = request.RequestContext.RouteData.Values["action"].ToString();
            Browser = request.Browser.Browser;
            UserAgent = request.UserAgent;
            SessionId = request.RequestContext.HttpContext.Session.SessionID;
            ClientIP = request.ServerVariables["REMOTE_ADDR"];
            RequestType = request.RequestType;
            QueryString = request.QueryString != null ? request.QueryString.ToString() : null;
        }

        [Required, MaxLength(128)]
        public string SessionId { get; set; }

        public string UrlRef { get; set; }

        [Required, MaxLength(30)]
        public string Controller { get; set; }

        [Required, MaxLength(30)]
        public string Action { get; set; }

        [Required, MaxLength(150)]
        public string Browser { get; set; }

        [Required, MaxLength(150)]
        public string ClientIP { get; set; }

        [Required, MaxLength(150)]
        public string UserAgent { get; set; }

        [Required, MaxLength(20)]
        public string RequestType { get; set; }

        public string QueryString { get; set; }
    }
}
