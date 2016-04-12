using System;
using System.Web.Mvc;
using System.Linq;

namespace SX.WebCore.HtmlHelpers
{
    public static partial class SxExtantions
    {
        public class SxTreeViewTreeViewSettings<TModel>
        {
            public Func<TModel, string> FuncContent { get; set; }
            public Func<TModel, int> FuncCurLevel { get; set; }
            public int MaxLevel { get; set; }
            public Func<TModel, string> FuncEditUrl { get; set; }
            public Func<string> FuncCreateUrl { get; set; }
            public Func<TModel, TModel[]> FuncChildren { get; set; }
            public Func<string> FuncSearchUrl { get; set; }
            public string UpdateTargetId { get; set; }
        }

        public static MvcHtmlString SxTreeView<TModel>(this HtmlHelper htmlHelper, TModel[] data, SxTreeViewTreeViewSettings<TModel> settings, object htmlAttributes = null)
        {
            var table = new TagBuilder("table");
            if (htmlAttributes != null)
            {
                var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                table.MergeAttributes(attributes, true);
            }

            writeTreeViewHeader(table, settings);

            var tbody = new TagBuilder("tbody");

            if (!data.Any())
            {
                var tr = new TagBuilder("tr");
                var td = new TagBuilder("td");
                td.MergeAttribute("colspan", "3");
                var div = new TagBuilder("div");
                div.MergeAttribute("class", "text-danger");
                div.InnerHtml += "Отсутствуют результаты запроса";
                td.InnerHtml += div;
                tr.InnerHtml += td;
                tbody.InnerHtml += tr;
            }
            else {

                for (int i = 0; i < data.Length; i++)
                {
                    var parent = data[i];
                    writeTreeViewNode(tbody, parent, settings);
                }
            }

            table.InnerHtml += tbody;

            return MvcHtmlString.Create(table.ToString());
        }

        private static void writeTreeViewNode<TModel>(TagBuilder tb, TModel node, SxTreeViewTreeViewSettings<TModel> settings)
        {
            var tr = new TagBuilder("tr");
            writeTreeViewRow(tr, node, settings);
            tb.InnerHtml += tr;

            var childs = settings.FuncChildren(node);
            if (childs.Any())
            {
                for (int i = 0; i < childs.Length; i++)
                {
                    var child = childs[i];
                    writeTreeViewNode(tb, child, settings);
                }
            }
        }

        private static void writeTreeViewRow<TModel>(TagBuilder tr, TModel node, SxTreeViewTreeViewSettings<TModel> settings)
        {
            writeTreeViewEditCol(tr, node, settings);

            var cl = settings.FuncCurLevel(node);
            if (cl < settings.MaxLevel)
            {
                for (int i = 1; i <= cl; i++)
                {
                    var td = new TagBuilder("td");
                    if (cl == i)
                    {
                        td.MergeAttribute("colspan", (settings.MaxLevel - cl + 1).ToString());
                        td.InnerHtml += settings.FuncContent(node);
                    }
                    else
                    {
                        td.MergeAttribute("class", "empty");
                    }

                    tr.InnerHtml += td;
                }
            }
            else
            {
                for (int i = 1; i <= settings.MaxLevel; i++)
                {
                    var td = new TagBuilder("td");
                    if (cl == i)
                        td.InnerHtml += settings.FuncContent(node);
                    else
                    {
                        td.MergeAttribute("class", "empty");
                    }
                    tr.InnerHtml += td;
                }
            }
        }

        private static void writeTreeViewEditCol<TModel>(TagBuilder tr, TModel node, SxTreeViewTreeViewSettings<TModel> settings)
        {
            var tdEdit = new TagBuilder("td");
            tdEdit.MergeAttribute("class", "edit-col");

            var a = new TagBuilder("a");
            a.MergeAttribute("href", settings.FuncEditUrl != null ? settings.FuncEditUrl(node) : "javascript:void(0)");
            var i = new TagBuilder("i");
            i.MergeAttribute("class", "fa fa-pencil");
            a.InnerHtml += i;

            tdEdit.InnerHtml += a;
            tr.InnerHtml += tdEdit;
        }

        private static void writeTreeViewHeader<TModel>(TagBuilder tb, SxTreeViewTreeViewSettings<TModel> settings)
        {
            var thead = new TagBuilder("thead");
            var tr = new TagBuilder("tr");
            writeTreeViewCreateCol(tr, settings.FuncCreateUrl);

            var th = new TagBuilder("th");
            th.MergeAttribute("colspan", settings.MaxLevel.ToString());

            var form = new TagBuilder("form");
            form.MergeAttribute("action", settings.FuncSearchUrl());
            form.MergeAttribute("data-ajax", "true");
            form.MergeAttribute("data-ajax-method", "post");
            form.MergeAttribute("data-ajax-update", "#"+settings.UpdateTargetId);
            var input = new TagBuilder("input");
            input.MergeAttribute("type", "text");
            input.MergeAttribute("placeholder", "Введите строку для поиска");
            input.MergeAttribute("name", "Title");
            form.InnerHtml += input;
            th.InnerHtml += form;

            tr.InnerHtml += th;

            thead.InnerHtml += tr;
            tb.InnerHtml += thead;
        }

        private static void writeTreeViewCreateCol(TagBuilder tr, Func<string> createUrl)
        {
            var th = new TagBuilder("th");
            th.MergeAttribute("class", "create-col");
            var a = new TagBuilder("a");
            a.MergeAttribute("href", createUrl != null ? createUrl() : "#");
            var i = new TagBuilder("i");
            i.MergeAttribute("class", "fa fa-plus-circle");
            a.InnerHtml += i;
            th.InnerHtml += a;
            tr.InnerHtml += th;
        }
    }
}
