using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SX.WebCore.HtmlHelpers
{
    public static partial class Extantions
    {
        public static MvcHtmlString SxPager(this HtmlHelper htmlHelper, SxPagerInfo pagerinfo)
        {
            var ul = new TagBuilder("ul");
            ul.MergeAttributes(new Dictionary<string, object> {
            });
            ul.AddCssClass("sx-pager");

            var partsCount = Math.Ceiling((decimal)pagerinfo.TotalPages / pagerinfo.Size);

            for (int i = 1; i <= pagerinfo.TotalPages; i++)
            {
                var li = new TagBuilder("li");
                if (i == pagerinfo.Page)
                    li.AddCssClass("active");
                var a = new TagBuilder("a");
                if (i > pagerinfo.Size)
                {
                    var span = new TagBuilder("span");
                    span.AddCssClass("fa fa-caret-right");
                    a.InnerHtml += span;
                    li.InnerHtml += a;
                    ul.InnerHtml += li;
                    break;
                }
                else
                {
                    a.InnerHtml += i;
                    a.MergeAttributes(new Dictionary<string, object>() {
                        { "href", "javascript:void(0)" },
                        { "onclick", "clickPager(this)" },
                    });
                    
                    li.InnerHtml += a;
                    ul.InnerHtml += li;
                }
            }

            return MvcHtmlString.Create(ul.ToString());
        }

        public class SxPagerInfo
        {
            public int Page { get; set; }
            public int PageSize { get; set; }
            public int TotalItems { get; set; }
            public int TotalPages 
            { 
                get
                {
                    var count = (int)Math.Ceiling((decimal)TotalItems / PageSize);
                    return count;
                }
            }
            public int Size { get; set; }
        }
    }
}
