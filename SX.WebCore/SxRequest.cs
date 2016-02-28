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
    public sealed class SxRequest : SxDbModel<Guid>
    {
        public SxRequest() { }
        public SxRequest(HttpRequestBase request, string sessionId)
        {
            UrlRef = request.UrlReferrer != null ? request.UrlReferrer.AbsolutePath : null;
            Controller = request.RequestContext.RouteData.Values["controller"].ToString();
            Action = request.RequestContext.RouteData.Values["action"].ToString();
            Browser = request.Browser.Browser;
            UserAgent = request.UserAgent;
            SessionId = sessionId;
            ClientIP = request.ServerVariables["REMOTE_ADDR"];
            RequestType = request.RequestType;
            QueryString = request.QueryString != null ? request.QueryString.ToString() : null;
        }

        [Required, MaxLength(128), Index]
        public string SessionId { get; set; }

        public string UrlRef { get; set; }

        [Required, MaxLength(30), Index]
        public string Controller { get; set; }

        [Required, MaxLength(30), Index]
        public string Action { get; set; }

        [Required, MaxLength(150)]
        public string Browser { get; set; }

        [Required, MaxLength(150)]
        public string ClientIP { get; set; }

        [Required, MaxLength(150)]
        public string UserAgent { get; set; }

        [Required, MaxLength(20), Index]
        public string RequestType { get; set; }

        [MaxLength(255), Index]
        public string QueryString { get; set; }
    }
}
