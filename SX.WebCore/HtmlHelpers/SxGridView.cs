using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace SX.WebCore.HtmlHelpers
{
    public static partial class SxExtantions
    {
        public static MvcHtmlString SxGridView<TModel>(this HtmlHelper htmlHelper, SxGridViewSettings<TModel> settings = null, object htmlAttributes = null)
        {
            var guid = Guid.NewGuid().ToString().ToLower();

            var page = htmlHelper.ViewData["Page"];
            var pageSize = htmlHelper.ViewData["PageSize"];
            var rowsCount = htmlHelper.ViewData["RowsCount"];
            var pagerInfo = new SxPagerInfo
            {
                Page = (int)page,
                PageSize = (int)pageSize,
                TotalItems = (int)rowsCount
            };
            settings.PagerInfo = pagerInfo;

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
            table.AddCssClass("sx-gv");

            //header
            table.InnerHtml += getGridViewHeader(settings);

            //body
            var tbody = new TagBuilder("tbody");
            var showRowMenu = Convert.ToBoolean(settings.ShowFilterRowMenu);
            if (showRowMenu)
                tbody.InnerHtml += getGridViewRowMenu<TModel>(settings);
            tbody.InnerHtml += getGridViewRows(settings.Data, settings, htmlHelper);
            table.InnerHtml += tbody;

            //footer
            table.InnerHtml += getGridViewFooter(htmlHelper, settings);

            return MvcHtmlString.Create(table.ToString());
        }

        public class SxGridViewSettings<TModel>
        {
            public SxGridViewSettings()
            {
                KeyFieldsName = new string[] { "Id" };
                ShowFilterRowMenu = true;
                EnableSorting = true;
                EnableCreate = true;
                EnableEditing = true;
                EnableDelete = false;
            }

            public string UpdateTargetId { get; set; }

            public TModel[] Data { get; set; }
            public Type ModelType
            {
                get
                {
                    var collectionType = Data.GetType();
                    var modelType = collectionType.GetElementType();
                    return modelType;
                }
            }

            public SxGridViewColumn[] Columns { get; set; }

            public string GeneralControllerName { get; set; }

            public string[] KeyFieldsName { get; set; }

            public bool ShowFilterRowMenu { get; set; }

            public bool EnableSorting { get; set; }

            public IDictionary<string, SortDirection> SortDirections { get; set; }

            public bool EnableCreate { get; set; }

            public bool EnableEditing { get; set; }

            
            public string CreateLink { get; set; }

            public bool EnableDelete { get; set; }
            public string DeleteLink { get; set; }

            public dynamic Filter { get; set; }
            public SxPagerInfo PagerInfo { get; set; }
            public string PagerLink { get; set; }
        }
        public class SxGridViewColumn
        {
            public string FieldName { get; set; }

            private string _caption;
            public string Caption 
            { 
                get
                {
                    return string.IsNullOrEmpty(_caption) ? FieldName : _caption;
                }
                set
                {
                    _caption = value;
                }
            }

            public Func<object, string> Template { get; set; }
        }

        private static TagBuilder getForm<TModel>(SxGridViewSettings<TModel> settings, string guid)
        {
            var form = new TagBuilder("form");
            form.MergeAttributes(new Dictionary<string, string>{
                    {"id", "grid-view-form-"+guid},
                    {"method", "post"},
                    {"data-ajax", "true"},
                    {"data-ajax-update", "#"+settings.UpdateTargetId},
                    {"data-ajax-url", settings.PagerLink ?? "index"}
                });

            //pager
            var pagerHidden = new TagBuilder("input");
            pagerHidden.MergeAttributes(new Dictionary<string, string>{
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
                hidden.MergeAttributes(new Dictionary<string, string>{
                        {"name", column.FieldName},
                        {"type", "hidden"}
                    });
                form.InnerHtml += hidden;

                //order
                hidden = new TagBuilder("input");
                hidden.MergeAttributes(new Dictionary<string, string>{
                        {"name", string.Format("order[{0}]", column.FieldName)},
                        {"type", "hidden"}
                    });
                form.InnerHtml += hidden;
            }

            return form;
        }

        private static TagBuilder getGridViewHeader<TModel>(SxGridViewSettings<TModel> settings)
        {
            var thead = new TagBuilder("thead");
            var tr = new TagBuilder("tr");

            if (settings.ShowFilterRowMenu || settings.EnableCreate)
            {
                var values=HttpContext.Current.Request.RequestContext.RouteData.Values;
                var controller= values["controller"].ToString().ToLower();
                var th = new TagBuilder("th");
                th.AddCssClass("sx-gv-add-column");
                var createUrl = settings.CreateLink!=null
                    ? string.Format("<a href=\"{0}\"><i class=\"fa fa-plus-circle\"></i></a>", settings.CreateLink)
                    : string.Format("<a href=\"/{0}/edit\"><i class=\"fa fa-plus-circle\"></i></a>", controller);
                th.InnerHtml += settings.EnableCreate ? createUrl : "#";
                tr.InnerHtml += th;
            }

            for (int i = 0; i < settings.Columns.Length; i++)
            {
                var column = settings.Columns[i];

                var th = new TagBuilder("th");
                th.InnerHtml += column.Caption;

                if (settings.EnableSorting)
                {
                    var hasOrderDirection = settings.SortDirections != null && settings.SortDirections.ContainsKey(column.FieldName);
                    var direction = hasOrderDirection && settings.SortDirections[column.FieldName] != SortDirection.Unknown ? settings.SortDirections[column.FieldName] : SortDirection.Unknown;
                    th.MergeAttributes(new Dictionary<string, object>() {
                        {"onclick", "pressGridViewColumn(this)"},
                        { "class", "ordered-column" },
                        { "data-column-name", column.FieldName },
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

        private static string getGridViewRows<TModel>(TModel[] data, SxGridViewSettings<TModel> settings, HtmlHelper htmlHelper)
        {
            var sb = new StringBuilder();
            if (data != null && data.Any())
            {
                for (int i = 0; i < data.Length; i++)
                {
                    var model = data[i];
                    sb.Append(getGridViewRow(model, settings, htmlHelper));
                }
            }

            return sb.ToString();
        }
        private static TagBuilder getGridViewRow<TModel>(TModel model, SxGridViewSettings<TModel> settings, HtmlHelper htmlHelper)
        {
            var tr = new TagBuilder("tr");
            if (settings.ShowFilterRowMenu || settings.EnableEditing)
            {
                var td = new TagBuilder("td");
                td.AddCssClass("sx-gv-edit-column");

                var routes = HttpContext.Current.Request.RequestContext.RouteData.Values;
                var controller = routes["controller"].ToString().ToLower();
                controller = settings.GeneralControllerName != null ? settings.GeneralControllerName.ToLowerInvariant() : controller;
                var queryString = new StringBuilder();
                queryString.Append("?");
                for (int i = 0; i < settings.KeyFieldsName.Length; i++)
                {
                    var key = settings.KeyFieldsName[i];
                    queryString.Append(key + "=" + model.GetType().GetProperty(key).GetValue(model) + "&");
                }
                var qs = queryString.ToString().Substring(0, queryString.Length - 1);
                //edit link
                if (settings.EnableEditing)
                {
                    var a = new TagBuilder("a");
                    var type = typeof(TModel);

                    a.MergeAttribute("href", string.Format("/{0}/edit{1}", controller, qs));
                    a.InnerHtml += "<i class=\"fa fa-pencil\"></i>";

                    td.InnerHtml += a;
                }
                //delete link
                if(settings.EnableDelete)
                {
                    td.InnerHtml += string.Format("<form method=\"post\" data-ajax-method=\"post\" action=\"{0}\" data-ajax=\"true\" data-ajax-mode=\"replace\" data-ajax-update=\"#{1}\">", settings.DeleteLink ?? "delete", settings.UpdateTargetId);
                    td.InnerHtml += htmlHelper.AntiForgeryToken();
                    for (int i = 0; i < settings.KeyFieldsName.Length; i++)
                    {
                        var key = settings.KeyFieldsName[i];
                        td.InnerHtml += string.Format("<input type=\"hidden\" name=\"{0}\" value=\"{1}\" />", key, model.GetType().GetProperty(key).GetValue(model));
                    }
                    td.InnerHtml += string.Format("<button title=\"Удалить\" type=\"submit\" onclick=\"if(!confirm('Удалить запись?')){{return false;}}\"><i class=\"fa fa-times\"></i></button>");
                    td.InnerHtml += string.Format("</form>");
                }

                tr.InnerHtml += td;
            }

            var props = model.GetType().GetProperties();
            for (int i = 0; i < settings.Columns.Length; i++)
            {
                var column = settings.Columns[i];
                var td = new TagBuilder("td");

                var val=props.First(x => x.Name == column.FieldName).GetValue(model);
                var value = column.Template != null
                    ? htmlHelper.Raw(column.Template(val))
                    : val;

                td.InnerHtml += value;
                tr.InnerHtml += td;
            }
            return tr;
        }

        private static TagBuilder getGridViewRowMenu<TModel>(SxGridViewSettings<TModel> settings)
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

                var prop = settings.ModelType.GetProperties().FirstOrDefault(x=>x.Name==column.FieldName);
                if (prop == null)
                    throw new ArgumentException(string.Format("Модель должна содержать свойство \"{0}\"", column.FieldName));
                var editor = getColumnEditor(prop, settings);
                td.InnerHtml += editor;

                tr.InnerHtml += td;
            }
            return tr;
        }
        private static TagBuilder getColumnEditor<TModel>(PropertyInfo propertyInfo, SxGridViewSettings<TModel> settings)
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
        private static bool isNotEmptyFilter<TModel>(SxGridViewSettings<TModel> settings)
        {
            if (settings.Filter == null) return false;
            var fields = settings.Columns.Select(c => c.FieldName);
            var props = ((PropertyInfo[])settings.Filter.GetType().GetProperties())
                .Where(x => fields.Contains(x.Name) && x.GetValue(settings.Filter) != null).ToArray();
            
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

        private static TagBuilder getGridViewFooter<TModel>(HtmlHelper htmlHelper, SxGridViewSettings<TModel> settings)
        {
            

            var tfoot = new TagBuilder("tfoot");
            var tr = new TagBuilder("tr");
            var td = new TagBuilder("td");
            if (settings.ShowFilterRowMenu)
            {
                tr.InnerHtml += td;
            }

            td = new TagBuilder("td");
            td.MergeAttribute("colspan", settings.Columns.Length.ToString());
            if (settings.PagerInfo.TotalPages != 0)
            {
                if (settings.PagerInfo.TotalPages > 1)
                    td.InnerHtml += htmlHelper.SxPager(settings.PagerInfo);
            }
            else
            {
                td.InnerHtml += "<div class=\"text-danger\">Отсутствуют данные для отображения</div>";
            }
            tr.InnerHtml += td;

            tfoot.InnerHtml += tr;
            //tfoot.InnerHtml += getGridViewPagerProgress(settings);
            return tfoot;
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
