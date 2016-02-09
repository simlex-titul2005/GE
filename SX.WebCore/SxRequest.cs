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

        [Required, MaxLength(128), Column("SESSION_ID")]
        public string SessionId { get; set; }

        [Column("URL_REF")]
        public string UrlRef { get; set; }

        [Required, MaxLength(30), Column("CONTROLLER")]
        public string Controller { get; set; }

        [Required, MaxLength(30), Column("ACTION")]
        public string Action { get; set; }

        [Required, MaxLength(150), Column("BROWSER")]
        public string Browser { get; set; }

        [Required, MaxLength(150), Column("CLIENT_IP")]
        public string ClientIP { get; set; }

        [Required, MaxLength(150), Column("USER_AGENT")]
        public string UserAgent { get; set; }

        [Required, MaxLength(20), Column("REQUEST_TYPE")]
        public string RequestType { get; set; }

        [Column("QUERY_STRING")]
        public string QueryString { get; set; }
    }
}
