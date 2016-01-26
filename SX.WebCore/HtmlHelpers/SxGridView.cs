using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SX.WebCore.HtmlHelpers
{
    public static partial class Extantions
    {
        public static MvcHtmlString SxGridView<TModel>(this HtmlHelper htmlHelper, TModel[] data, SxGridViewSettings<TModel> settings = null, object htmlAttributes = null)
            where TModel : ISxViewModel
        {
            var page = htmlHelper.ViewData["Page"];
            var pageSize = htmlHelper.ViewData["PageSize"];
            var rowsCount = htmlHelper.ViewData["RowsCount"];
            settings.Page = (int)page;
            settings.PageSize = (int)pageSize;
            settings.RowsCount = (int)rowsCount;

            var guid = Guid.NewGuid().ToString().ToLower();

            if (settings != null && settings.ShowFilterRowMenu)
            {
                htmlHelper.ViewContext.Writer.Write(getFilterForm(settings, guid));
            }

            var table = new TagBuilder("table");
            table.MergeAttribute("id", guid);
            if (htmlAttributes != null)
            {
                var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                table.MergeAttributes(attributes, true);
            }
            table.AddCssClass("sx-grid-view");

            //header
            var modelType = typeof(TModel);
            table.InnerHtml += getGridViewHeader<TModel>(settings);

            //body
            var tbody = new TagBuilder("tbody");
            var showRowMenu = Convert.ToBoolean(settings.ShowFilterRowMenu);
            if (showRowMenu)
                tbody.InnerHtml += getGridViewRowMenu<TModel>(settings);
            tbody.InnerHtml += getGridViewRows(data, settings);
            table.InnerHtml += tbody;

            //footer
            table.InnerHtml += getGridViewFooter(settings);

            htmlHelper.ViewContext.Writer.Write(jsPressGridViewFilter);
            htmlHelper.ViewContext.Writer.Write(jsResetGridViewFilter);
            return MvcHtmlString.Create(table.ToString());
        }

        public class SxGridViewSettings<TModel> where TModel : ISxViewModel
        {
            private static string[] _columns;
            public string[] Columns 
            {
                get
                {
                    return _columns == null ? getModelColumns(typeof(TModel)) : _columns;
                }
                set
                {
                    _columns = value;
                }
            }
            public bool ShowFilterRowMenu { get; set; }
            public bool EnableSorting { get; set; }
            public bool EnableEditing { get; set; }
            public TModel Filter { get; set; }
            public int Page { get; set; }
            public int PageSize { get; set; }
            public int RowsCount { get; set; }
        }

        private static TagBuilder getGridViewHeader<TModel>(SxGridViewSettings<TModel> settings) where TModel : ISxViewModel
        {
            var thead = new TagBuilder("thead");
            var tr = new TagBuilder("tr");

            if (settings.ShowFilterRowMenu || settings.EnableEditing)
            {
                var th = new TagBuilder("th");
                th.InnerHtml += "#";
                tr.InnerHtml += th;
            }

            for (int i = 0; i < settings.Columns.Length; i++)
            {
                var column = settings.Columns[i];
                var th = new TagBuilder("th");
                th.InnerHtml += column;

                if (settings.EnableSorting)
                {
                    th.MergeAttributes(new Dictionary<string, object>() {
                        { "class", "ordered-column" }
                    });

                    var span = new TagBuilder("span");
                    span.MergeAttributes(new Dictionary<string, object>() {
                        { "class", "sort-btn fa fa-caret-down" }
                    });
                    th.InnerHtml += span;
                }

                tr.InnerHtml += th;
            }
            thead.InnerHtml += tr;

            return thead;
        }

        private static string getGridViewRows<TModel>(TModel[] data, SxGridViewSettings<TModel> settings) where TModel : ISxViewModel
        {
            var sb = new StringBuilder();
            if (data != null && data.Any())
            {
                for (int i = 0; i < data.Length; i++)
                {
                    var model = data[i];
                    sb.Append(getGridViewRow(model, settings));
                }
            }

            return sb.ToString();
        }

        private static TagBuilder getFilterForm<TModel>(SxGridViewSettings<TModel> settings, string guid) where TModel : ISxViewModel
        {
            var form = new TagBuilder("form");
            form.MergeAttributes(new Dictionary<string, object>{
                    {"id", "form-filter-"+guid},
                    {"method", "post"},
                    {"data-ajax", "true"},
                    {"data-ajax-update", "#list-articles"}
                });

            var columns = settings != null && settings.Columns != null ? settings.Columns : getModelColumns(typeof(TModel));
            for (int i = 0; i < columns.Length; i++)
            {
                var column = settings.Columns[i];
                var hidden = new TagBuilder("input");
                hidden.MergeAttributes(new Dictionary<string, object>{
                        {"name", column},
                        {"type", "hidden"}
                    });
                form.InnerHtml += hidden;
            }

            return form;
        }
        private static TagBuilder getGridViewRowMenu<TModel>(SxGridViewSettings<TModel> settings) where TModel : ISxViewModel
        {
            var tr = new TagBuilder("tr");
            tr.AddCssClass("filter-row");

            if (settings.ShowFilterRowMenu || settings.EnableEditing)
            {
                var td = new TagBuilder("td");
                td.MergeAttribute("style", "text-align: center;");
                if (isNotEmptyFilter(settings))
                {
                    var span = new TagBuilder("span");
                    span.AddCssClass("fa fa-refresh");
                    span.MergeAttributes(new Dictionary<string, object>{
                        {"onclick", "resetGridViewFilter(this)"},
                        {"style", "cursor:pointer;"}
                    });
                    td.InnerHtml += span;
                }
                tr.InnerHtml += td;
            }

            for (int i = 0; i < settings.Columns.Length; i++)
            {
                var column = settings.Columns[i];
                var td = new TagBuilder("td");

                var prop = typeof(TModel).GetProperties().First(x => x.Name == column);
                var editor = getColumnEditor(prop, settings);
                td.InnerHtml += editor;

                tr.InnerHtml += td;
            }
            return tr;
        }
        private static TagBuilder getColumnEditor<TModel>(PropertyInfo propertyInfo, SxGridViewSettings<TModel> settings) where TModel : ISxViewModel
        {
            var propertyType = propertyInfo.PropertyType;

            var editor = new TagBuilder("input");
            editor.MergeAttribute("name", propertyInfo.Name);
            editor.MergeAttribute("onkeypress", "pressGridViewFilter(event)");

            object value = null;
            if (settings.Filter != null)
                value=settings.Filter.GetType().GetProperty(propertyInfo.Name).GetValue(settings.Filter);

            if (propertyType == typeof(Int32))
            {
                editor.MergeAttributes(new Dictionary<string, object>() {
                    { "type", "number" }
                });
                if (value!=null && (int)value != 0)
                    editor.MergeAttribute("value", value.ToString());
            }
            else if (propertyType == typeof(DateTime))
            {
                editor.MergeAttributes(new Dictionary<string, object>() {
                    { "type", "datetime" }
                });
                if (value != null && (DateTime)value != DateTime.MinValue)
                    editor.MergeAttribute("value", value.ToString());
            }
            else if (propertyType == typeof(String))
            {
                editor.MergeAttributes(new Dictionary<string, object>() {
                    { "type", "text" }
                });
                if (value != null)
                    editor.MergeAttribute("value", value.ToString());
            }

            return editor;
        }
        private static bool isNotEmptyFilter<TModel>(SxGridViewSettings<TModel> settings) where TModel : ISxViewModel
        {
            if (settings.Filter == null) return false;

            var props = settings.Filter.GetType().GetProperties().Where(x => settings.Columns.Contains(x.Name) && x.GetValue(settings.Filter)!=null).ToArray();
            var count = 0;
            for (int i = 0; i < props.Length; i++)
            {
                var prop = props[i];
                var propType = prop.PropertyType;
                var value = prop.GetValue(settings.Filter);
                count += propType == typeof(Int32) && (int)value != 0 ? 1 : 0;
                count += propType == typeof(DateTime) && (DateTime)value != DateTime.MinValue ? 1 : 0;
                count += propType == typeof(String) && value != null ? 1 : 0;
            }
            return count > 0;
        }

        private static TagBuilder getGridViewFooter<TModel>(SxGridViewSettings<TModel> settings) where TModel : ISxViewModel
        {
            var tfoot = new TagBuilder("tfoot");
            var tr = new TagBuilder("tr");
            var td = new TagBuilder("td");
            td.MergeAttribute("colspan", (settings.ShowFilterRowMenu ? settings.Columns.Length + 1 : settings.Columns.Length).ToString());
            td.InnerHtml += "page=" + settings.Page + "; pageSize=" + settings.PageSize + "; rowsCount=" + settings.RowsCount;
            td.InnerHtml += "<br/>simlex.dev.2014@gmail.com";
            tr.InnerHtml += td;
            tfoot.InnerHtml += tr;
            return tfoot;
        }

        private static TagBuilder getGridViewRow<TModel>(TModel model, SxGridViewSettings<TModel> settings) where TModel : ISxViewModel
        {
            var tr = new TagBuilder("tr");
            if (settings.ShowFilterRowMenu || settings.EnableEditing)
            {
                var td = new TagBuilder("td");
                tr.InnerHtml += td;
            }

            var props = model.GetType().GetProperties();
            for (int i = 0; i < settings.Columns.Length; i++)
            {
                var column = settings.Columns[i];
                var td = new TagBuilder("td");
                var value = props.First(x => x.Name == column).GetValue(model);
                td.InnerHtml += value;
                tr.InnerHtml += td;
            }
            return tr;
        }

        private static string[] getModelColumns(Type modelType)
        {
            return modelType.GetProperties().Select(x => x.Name).OrderBy(x => x).ToArray();
        }

        private static string jsPressGridViewFilter
        {
            get
            {
                var sb=new StringBuilder();
                sb.Append("<script type=\"text/javascript\">");
                sb.Append("function pressGridViewFilter(e) {");
                sb.Append("var keyCode = (window.event) ? e.which : e.keyCode;");
                sb.Append("if (keyCode == 13) {");
                sb.Append("var guid = $(e.target).closest('table').attr('id');");
                sb.Append("$('table[id=\"' + guid + '\"] .filter-row input').each(function (i, item) {");
                sb.Append("var column = $(item).attr('name');");
                sb.Append("var value = $(item).val();");
                sb.Append("if(value!='')");
                sb.Append("$('input[name=\"' + column + '\"]').attr('value', value);");
                sb.Append("});");
                sb.Append("$('#form-filter-' + guid).submit();");
                sb.Append("}}</script>");

                return sb.ToString();
            }
        }

        private static string jsResetGridViewFilter
        {
            get
            {
                var sb=new StringBuilder();
                sb.Append("<script type=\"text/javascript\">");
                sb.Append("function resetGridViewFilter(e){");
                sb.Append("var guid = $(e).closest('table').attr('id');");
                sb.Append("$('#form-filter-' + guid).submit();}");
                sb.Append("</script>");

                return sb.ToString();
            }
        }
    }
}
