// <auto-generated />
// This file was generated by a T4 template.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the .tt file (i.e. the T4 template) and save it to regenerate this file.

// Make sure the compiler doesn't complain about missing Xml comments and CLS compliance
// 0108: suppress "Foo hides inherited member Foo. Use the new keyword if hiding was intended." when a controller and its abstract parent are both processed
// 0114: suppress "Foo.BarController.Baz()' hides inherited member 'Qux.BarController.Baz()'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword." when an action (with an argument) overrides an action in a parent controller
#pragma warning disable 1591, 3008, 3009, 0108, 0114
#region T4MVC

using System;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using T4MVC;
namespace GE.WebAdmin.Controllers
{
    public partial class ArticleTypesController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected ArticleTypesController(Dummy d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(Task<ActionResult> taskResult)
        {
            return RedirectToAction(taskResult.Result);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoutePermanent(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(Task<ActionResult> taskResult)
        {
            return RedirectToActionPermanent(taskResult.Result);
        }

        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult ArticleTypesByGameId()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ArticleTypesByGameId);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ArticleTypesController Actions { get { return MVC.ArticleTypes; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "ArticleTypes";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "ArticleTypes";
        [GeneratedCode("T4MVC", "2.0")]
        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string Index = "Index";
            public readonly string Edit = "Edit";
            public readonly string ArticleTypesByGameId = "ArticleTypesByGameId";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string Index = "Index";
            public const string Edit = "Edit";
            public const string ArticleTypesByGameId = "ArticleTypesByGameId";
        }


        static readonly ActionParamsClass_Index s_params_Index = new ActionParamsClass_Index();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Index IndexParams { get { return s_params_Index; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Index
        {
            public readonly string page = "page";
            public readonly string filter = "filter";
            public readonly string order = "order";
        }
        static readonly ActionParamsClass_Edit s_params_Edit = new ActionParamsClass_Edit();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Edit EditParams { get { return s_params_Edit; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Edit
        {
            public readonly string name = "name";
            public readonly string gameId = "gameId";
            public readonly string model = "model";
        }
        static readonly ActionParamsClass_ArticleTypesByGameId s_params_ArticleTypesByGameId = new ActionParamsClass_ArticleTypesByGameId();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_ArticleTypesByGameId ArticleTypesByGameIdParams { get { return s_params_ArticleTypesByGameId; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_ArticleTypesByGameId
        {
            public readonly string gameId = "gameId";
            public readonly string curName = "curName";
        }
        static readonly ViewsClass s_views = new ViewsClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewsClass Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewsClass
        {
            static readonly _ViewNamesClass s_ViewNames = new _ViewNamesClass();
            public _ViewNamesClass ViewNames { get { return s_ViewNames; } }
            public class _ViewNamesClass
            {
                public readonly string _ArticleTypesByGameId = "_ArticleTypesByGameId";
                public readonly string _GridView = "_GridView";
                public readonly string Edit = "Edit";
                public readonly string Index = "Index";
            }
            public readonly string _ArticleTypesByGameId = "~/Views/ArticleTypes/_ArticleTypesByGameId.cshtml";
            public readonly string _GridView = "~/Views/ArticleTypes/_GridView.cshtml";
            public readonly string Edit = "~/Views/ArticleTypes/Edit.cshtml";
            public readonly string Index = "~/Views/ArticleTypes/Index.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_ArticleTypesController : GE.WebAdmin.Controllers.ArticleTypesController
    {
        public T4MVC_ArticleTypesController() : base(Dummy.Instance) { }

        [NonAction]
        partial void IndexOverride(T4MVC_System_Web_Mvc_ViewResult callInfo, int page);

        [NonAction]
        public override System.Web.Mvc.ViewResult Index(int page)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ViewResult(Area, Name, ActionNames.Index);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "page", page);
            IndexOverride(callInfo, page);
            return callInfo;
        }

        [NonAction]
        partial void IndexOverride(T4MVC_System_Web_Mvc_PartialViewResult callInfo, GE.WebAdmin.Models.VMArticleType filter, System.Collections.Generic.IDictionary<string,SX.WebCore.HtmlHelpers.SxExtantions.SortDirection> order, int page);

        [NonAction]
        public override System.Web.Mvc.PartialViewResult Index(GE.WebAdmin.Models.VMArticleType filter, System.Collections.Generic.IDictionary<string,SX.WebCore.HtmlHelpers.SxExtantions.SortDirection> order, int page)
        {
            var callInfo = new T4MVC_System_Web_Mvc_PartialViewResult(Area, Name, ActionNames.Index);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "filter", filter);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "order", order);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "page", page);
            IndexOverride(callInfo, filter, order, page);
            return callInfo;
        }

        [NonAction]
        partial void EditOverride(T4MVC_System_Web_Mvc_ViewResult callInfo, string name, int? gameId);

        [NonAction]
        public override System.Web.Mvc.ViewResult Edit(string name, int? gameId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ViewResult(Area, Name, ActionNames.Edit);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "name", name);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "gameId", gameId);
            EditOverride(callInfo, name, gameId);
            return callInfo;
        }

        [NonAction]
        partial void EditOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, GE.WebAdmin.Models.VMEditArticleType model);

        [NonAction]
        public override System.Web.Mvc.ActionResult Edit(GE.WebAdmin.Models.VMEditArticleType model)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Edit);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "model", model);
            EditOverride(callInfo, model);
            return callInfo;
        }

        [NonAction]
        partial void ArticleTypesByGameIdOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, int? gameId, string curName);

        [NonAction]
        public override System.Web.Mvc.ActionResult ArticleTypesByGameId(int? gameId, string curName)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ArticleTypesByGameId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "gameId", gameId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "curName", curName);
            ArticleTypesByGameIdOverride(callInfo, gameId, curName);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591, 3008, 3009, 0108, 0114
