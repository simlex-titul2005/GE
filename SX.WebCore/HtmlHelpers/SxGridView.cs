using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

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

            private bool _showFilterRowMenu = true;
            public bool ShowFilterRowMenu 
            { 
                get
                {
                    return _showFilterRowMenu;
                }
                set
                {
                    _showFilterRowMenu = value;
                }
            }

            private bool _enableSorting = true;
            public bool EnableSorting 
            { 
                get
                {
                    return _enableSorting;
                }
                set
                {
                    _enableSorting=value;
                }
            }

            public IDictionary<string, SortDirection> SortDirections { get; set; }
            private bool _enableEditing = true;
            public bool EnableEditing 
            { 
                get
                {
                    return _enableEditing;
                }
                set
                {
                    _enableEditing = true;
                }
            }

            public dynamic Filter { get; set; }
            public SxPagerInfo PagerInfo { get; set; }
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
        }

        private static TagBuilder getForm<TModel>(SxGridViewSettings<TModel> settings, string guid)
        {
            var form = new TagBuilder("form");
            form.MergeAttributes(new Dictionary<string, string>{
                    {"id", "grid-view-form-"+guid},
                    {"method", "post"},
                    {"data-ajax", "true"},
                    {"data-ajax-update", "#"+settings.UpdateTargetId}
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

            if (settings.ShowFilterRowMenu || settings.EnableEditing)
            {
                var values=HttpContext.Current.Request.RequestContext.RouteData.Values;
                var controller=values["controller"].ToString().ToLower();
                var th = new TagBuilder("th");
                th.AddCssClass("sx-gv-add-column");
                th.InnerHtml += settings.EnableEditing ? string.Format("<a href=\"{0}/edit\"><i class=\"fa fa-plus-circle\"></i></a>", controller) : "#";
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
                    var direction = hasOrderDirection && settings.SortDirections[column.FieldName] != SortDirection.Unknown ? settings.SortDirections[column.FieldName] : SortDirection.Asc;
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
            var queryString = htmlHelper.ViewContext.HttpContext.Request.QueryString;
            var redactedIdString = queryString.Get("redactedId");
            var redactedId = !string.IsNullOrEmpty(redactedIdString) ? int.Parse(redactedIdString) : (int?)null;

            var sb = new StringBuilder();
            if (data != null && data.Any())
            {
                for (int i = 0; i < data.Length; i++)
                {
                    var model = data[i];
                    sb.Append(getGridViewRow(model, settings, redactedId));
                }
            }

            return sb.ToString();
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

        private static TagBuilder getGridViewRow<TModel>(TModel model, SxGridViewSettings<TModel> settings, int? redactedId=null)
        {
            var tr = new TagBuilder("tr");
            if (redactedId.HasValue)
                tr.AddCssClass("sx-gv-redacted-row");
            if (settings.ShowFilterRowMenu || settings.EnableEditing)
            {
                var td = new TagBuilder("td");
                td.AddCssClass("sx-gv-edit-column");

                if(settings.EnableEditing)
                {
                    var routes = HttpContext.Current.Request.RequestContext.RouteData.Values;
                    var controller = routes["controller"].ToString().ToLower();

                    //edit link
                    var a = new TagBuilder("a");
                    a.MergeAttribute("href", string.Format("{0}/edit?id={1}", controller, model.GetType().GetProperty("Id").GetValue(model)));
                    a.InnerHtml += "<i class=\"fa fa-pencil\"></i>";

                    td.InnerHtml += a;
                }

                tr.InnerHtml += td;
            }

            var props = model.GetType().GetProperties();
            for (int i = 0; i < settings.Columns.Length; i++)
            {
                var column = settings.Columns[i];
                var td = new TagBuilder("td");
                var value = props.First(x => x.Name == column.FieldName).GetValue(model);
                td.InnerHtml += value;
                tr.InnerHtml += td;
            }
            return tr;
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
        //private static TagBuilder getGridViewPagerProgress<TModel>(SxGridViewSettings<TModel> settings)
        //{
        //    var tr = new TagBuilder("tr");
        //    var td = new TagBuilder("td");
        //    var colspan = settings.EnableEditing || settings.ShowFilterRowMenu ? settings.Columns.Length + 1 : settings.Columns.Length;
        //    td.MergeAttribute("colspan", colspan.ToString());
        //    td.AddCssClass("sx-pager-progress");

        //    var div = new TagBuilder("div");
        //    var percent = 100 * settings.PagerInfo.Page / settings.PagerInfo.TotalPages;
        //    div.MergeAttribute("style", string.Format("width:{0}%", percent));
        //    td.InnerHtml += div;

        //    tr.InnerHtml += td;
        //    return tr;
        //}

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
