﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SX.WebCore.HtmlHelpers
{
    public static partial class SxExtantions
    {
        public static MvcHtmlString SxPager(this HtmlHelper htmlHelper, SxPagerInfo pagerinfo)
        {
            var ul = new TagBuilder("ul");
            ul.MergeAttributes(new Dictionary<string, object>
            {
            });
            ul.AddCssClass("sx-pager");

            var max = pagerinfo.CurrentPart * pagerinfo.PagerSize;
            var min = max - pagerinfo.PagerSize + 1;

            //first
            if (pagerinfo.CurrentPart > 1)
            {
                ul.InnerHtml += getPagerItem(pagerinfo, SxPagerItemType.First);
            }

            //prev
            if (pagerinfo.CurrentPart > 1)
            {
                ul.InnerHtml += getPagerItem(pagerinfo, SxPagerItemType.Prev);
            }

            //normal
            for (int i = min; i <= max; i++)
            {
                if (i == pagerinfo.TotalPages + 1) break;

                ul.InnerHtml += getPagerItem(pagerinfo, SxPagerItemType.Normal, i);
            }

            //next
            if (pagerinfo.CurrentPart < pagerinfo.PartsCount)
            {
                ul.InnerHtml += getPagerItem(pagerinfo, SxPagerItemType.Next);
            }

            //last
            if (pagerinfo.CurrentPart < pagerinfo.PartsCount)
            {
                ul.InnerHtml += getPagerItem(pagerinfo, SxPagerItemType.Last);
            }

            return MvcHtmlString.Create(ul.ToString());
        }

        private static TagBuilder getPagerItem(SxPagerInfo pagerinfo, SxPagerItemType itemType, int? number = null)
        {
            int page = 1;
            string cssClass = null;
            switch (itemType)
            {
                case SxPagerItemType.First:
                    page = 1;
                    cssClass = "fa fa-angle-double-left";
                    break;
                case SxPagerItemType.Prev:
                    page = pagerinfo.Page - 1;
                    cssClass = "fa fa-caret-left";
                    break;
                case SxPagerItemType.Normal:
                    page = (int)number;
                    break;
                case SxPagerItemType.Next:
                    page = pagerinfo.Page + 1;
                    cssClass = "fa fa-caret-right";
                    break;
                case SxPagerItemType.Last:
                    page = pagerinfo.TotalPages;
                    cssClass = "fa fa-angle-double-right";
                    break;
            }

            var li = new TagBuilder("li");
            if (pagerinfo.Page == page && itemType == SxPagerItemType.Normal)
                li.AddCssClass("active");
            var a = new TagBuilder("a");
            a.MergeAttributes(new Dictionary<string, object>() {
                        { "href", "javascript:void(0)" },
                        { "data-page", page },
                        { "onclick", "clickPager(this)" },
                    });
            if (itemType != SxPagerItemType.Normal)
            {
                var span = new TagBuilder("span");
                span.AddCssClass(cssClass);
                a.InnerHtml += span;
            }
            else
            {
                a.InnerHtml += page;
            }
            li.InnerHtml += a;

            return li;
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

            private int _pagerSize = 10;
            public int PagerSize
            {
                get
                {
                    return _pagerSize;
                }
                set
                {
                    _pagerSize = (int)value;
                }
            }

            public int PartsCount
            {
                get
                {
                    return (int)Math.Ceiling((decimal)TotalPages / PagerSize);
                }
            }
            public int CurrentPart
            {
                get
                {
                    return (int)Math.Ceiling((decimal)Page / PagerSize);
                }
            }
        }

        private enum SxPagerItemType : byte
        {
            Unknown = 0,
            First = 1,
            Prev = 2,
            Normal = 3,
            Next = 4,
            Last = 5
        }

        public sealed class SxPagedCollection<TModel>
        {
            public SxPagedCollection()
            {
                Collection = new TModel[0];
            }

            public TModel[] Collection { get; set; }
            public SX.WebCore.HtmlHelpers.SxExtantions.SxPagerInfo PagerInfo { get; set; }
            public int Length
            {
                get
                {
                    return this.Collection.Length;
                }
            }
        }
    }
}