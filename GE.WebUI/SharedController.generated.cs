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
namespace T4MVC
{
    public class SharedController
    {

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
                public readonly string _Analytics = "_Analytics";
                public readonly string _Breadcrumbs = "_Breadcrumbs";
                public readonly string _Errors = "_Errors";
                public readonly string _Footer = "_Footer";
                public readonly string _Layout = "_Layout";
                public readonly string _NavBar = "_NavBar";
                public readonly string _SocialIconsMenu = "_SocialIconsMenu";
            }
            public readonly string _Analytics = "~/Views/Shared/_Analytics.cshtml";
            public readonly string _Breadcrumbs = "~/Views/Shared/_Breadcrumbs.cshtml";
            public readonly string _Errors = "~/Views/Shared/_Errors.cshtml";
            public readonly string _Footer = "~/Views/Shared/_Footer.cshtml";
            public readonly string _Layout = "~/Views/Shared/_Layout.cshtml";
            public readonly string _NavBar = "~/Views/Shared/_NavBar.cshtml";
            public readonly string _SocialIconsMenu = "~/Views/Shared/_SocialIconsMenu.cshtml";
            static readonly _DisplayTemplatesClass s_DisplayTemplates = new _DisplayTemplatesClass();
            public _DisplayTemplatesClass DisplayTemplates { get { return s_DisplayTemplates; } }
            [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
            public partial class _DisplayTemplatesClass
            {
                public readonly string VMMateriallnfo = "VMMateriallnfo";
            }
        }
    }

}

#endregion T4MVC
#pragma warning restore 1591, 3008, 3009, 0108, 0114
