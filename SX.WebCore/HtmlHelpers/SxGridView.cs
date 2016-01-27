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
        public static MvcHtmlString SxGridView<TModel>(this HtmlHelper htmlHelper, TModel[] data, SxGridViewSettings settings = null, object htmlAttributes = null)
            where TModel : ISxViewModel
        {
            var guid = Guid.NewGuid().ToString().ToLower();
            var collectionType = data.GetType();
            var modelType = collectionType.GetElementType();

            if (settings != null && settings.ShowFilterRowMenu)
            {
                htmlHelper.ViewContext.Writer.Write(getForm(settings, guid));
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
            table.InnerHtml += getGridViewHeader(settings);

            //body
            var tbody = new TagBuilder("tbody");
            var showRowMenu = Convert.ToBoolean(settings.ShowFilterRowMenu);
            if (showRowMenu)
                tbody.InnerHtml += getGridViewRowMenu<TModel>(settings);
            tbody.InnerHtml += getGridViewRows(data, settings);
            table.InnerHtml += tbody;

            //footer
            table.InnerHtml += getGridViewFooter(htmlHelper, settings);

            return MvcHtmlString.Create(table.ToString());
        }

        public class SxGridViewSettings
        {
            private Type _modelType=null;
            private static string[] _columns;

            public SxGridViewSettings(Type modelType)
            {
                _modelType = modelType;
            }

            public Type ModelType 
            { 
                get
                {
                    return _modelType;
                }
            }
            public string[] Columns
            {
                get
                {
                    return _columns == null ? getModelColumns(_modelType) : _columns;
                }
                set
                {
                    _columns = value;
                }
            }
            public bool ShowFilterRowMenu { get; set; }
            public bool EnableSorting { get; set; }
            public IDictionary<string, SortDirection> SortDirections { get; set; }
            public bool EnableEditing { get; set; }
            public dynamic Filter { get; set; }
            public int Page { get; set; }
            public int PageSize { get; set; }
            public int RowsCount { get; set; }
            public int? PagerSize { get; set; }
        }

        private static TagBuilder getForm(SxGridViewSettings settings, string guid)
        {
            var form = new TagBuilder("form");
            form.MergeAttributes(new Dictionary<string, object>{
                    {"id", "grid-view-form-"+guid},
                    {"method", "post"},
                    {"data-ajax", "true"},
                    {"data-ajax-update", "#list-articles"}
                });

            //pager
            var pagerHidden = new TagBuilder("input");
            pagerHidden.MergeAttributes(new Dictionary<string, object>{
                        {"name", "page"},
                        {"type", "hidden"}
                    });
            form.InnerHtml += pagerHidden;

            //columns
            for (int i = 0; i < settings.Columns.Length; i++)
            {
                //filter
                var column = settings.Columns[i];
                var hidden = new TagBuilder("input");
                hidden.MergeAttributes(new Dictionary<string, object>{
                        {"name", column},
                        {"type", "hidden"}
                    });
                form.InnerHtml += hidden;

                //order
                hidden = new TagBuilder("input");
                hidden.MergeAttributes(new Dictionary<string, object>{
                        {"name", string.Format("order[{0}]", column)},
                        {"type", "hidden"}
                    });
                form.InnerHtml += hidden;
            }

            return form;
        }

        private static TagBuilder getGridViewHeader(SxGridViewSettings settings)
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
                    var hasOrderDirection = settings.SortDirections != null && settings.SortDirections.ContainsKey(column);
                    var direction = hasOrderDirection && settings.SortDirections[column] != SortDirection.Unknown ? settings.SortDirections[column] : SortDirection.Asc;
                    th.MergeAttributes(new Dictionary<string, object>() {
                        {"onclick", "pressGridViewColumn(this)"},
                        { "class", "ordered-column" },
                        { "data-column-name", column },
                        { "data-sort-direction", direction}
                    });

                    var span = new TagBuilder("span");
                    span.MergeAttributes(new Dictionary<string, object>() {
                        { "class", string.Format("sort-btn fa fa-caret-{0}", direction==SortDirection.Asc?"down":"up") }
                    });
                    th.InnerHtml += span;
                }

                tr.InnerHtml += th;
            }
            thead.InnerHtml += tr;

            return thead;
        }

        private static string getGridViewRows<TModel>(TModel[] data, SxGridViewSettings settings) where TModel : ISxViewModel
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

        private static TagBuilder getGridViewRowMenu<TModel>(SxGridViewSettings settings) where TModel : ISxViewModel
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

                var prop = settings.ModelType.GetProperties().FirstOrDefault(x=>x.Name==column);
                var editor = getColumnEditor(prop, settings);
                td.InnerHtml += editor;

                tr.InnerHtml += td;
            }
            return tr;
        }
        private static TagBuilder getColumnEditor(PropertyInfo propertyInfo, SxGridViewSettings settings)
        {
            var propertyType = propertyInfo.PropertyType;

            var editor = new TagBuilder("input");
            editor.MergeAttribute("name", propertyInfo.Name);
            editor.MergeAttribute("onkeypress", "pressGridViewFilter(event)");

            object value = null;
            if (settings.Filter != null)
                value = settings.Filter.GetType().GetProperty(propertyInfo.Name).GetValue(settings.Filter);

            if (propertyType == typeof(Int32))
            {
                editor.MergeAttributes(new Dictionary<string, object>() {
                    { "type", "number" }
                });
                if (value != null && (int)value != 0)
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
        private static bool isNotEmptyFilter(SxGridViewSettings settings)
        {
            if (settings.Filter == null) return false;

            var props = ((PropertyInfo[])settings.Filter.GetType().GetProperties()).Where(x => settings.Columns.Contains(x.Name) && x.GetValue(settings.Filter) != null).ToArray();
            
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

        private static TagBuilder getGridViewFooter(HtmlHelper htmlHelper, SxGridViewSettings settings)
        {
            var page = htmlHelper.ViewData["Page"];
            var pageSize = htmlHelper.ViewData["PageSize"];
            var rowsCount = htmlHelper.ViewData["RowsCount"];
            var pagerInfo = new SxPagerInfo
            {
                Page = (int)page,
                PageSize = (int)pageSize,
                TotalItems = (int)rowsCount,
                Size = settings.PagerSize.HasValue ? (int)settings.PagerSize : 10
            };

            var tfoot = new TagBuilder("tfoot");
            var tr = new TagBuilder("tr");
            var td = new TagBuilder("td");
            if (settings.ShowFilterRowMenu)
            {
                tr.InnerHtml += td;
            }

            td = new TagBuilder("td");
            td.MergeAttribute("colspan", settings.Columns.Length.ToString());
            if (pagerInfo.TotalPages != 0)
            {
                if (pagerInfo.TotalPages > 1)
                    td.InnerHtml += htmlHelper.SxPager(pagerInfo);
            }
            else
            {
                td.InnerHtml += "Отсутствуют данные для отображения";
            }
            tr.InnerHtml += td;

            tfoot.InnerHtml += tr;
            return tfoot;
        }

        private static TagBuilder getGridViewRow<TModel>(TModel model, SxGridViewSettings settings) where TModel : ISxViewModel
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

        public enum SortDirection : byte
        {
            Unknown=0,
            Asc=1,
            Desc=2
        }
    }
}
