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
        public static MvcHtmlString SxGridView<TModel>(this HtmlHelper htmlHelper, TModel[] data, SxGridViewSettings settings=null, object htmlAttributes=null)
        {
            var table = new TagBuilder("table");

            if (htmlAttributes != null)
            {
                var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                table.MergeAttributes(attributes);
            }

            var modelType = typeof(TModel);
            table.InnerHtml += getGridViewHeader<TModel>(settings);
            table.InnerHtml += getGridViewRows(data, settings);
            return MvcHtmlString.Create(table.ToString());
        }

        public class SxGridViewSettings
        {
            public string[] Columns { get; set; }
            public bool ShowRowMenu { get; set; }
            public bool EnableSorting { get; set; }
            public bool EnableEditing { get; set; }
        }

        private static TagBuilder getGridViewHeader<TModel>(SxGridViewSettings settings)
        {
            var columns = settings != null && settings.Columns != null ? settings.Columns : getModelProperties(typeof(TModel));
            var thead = new TagBuilder("thead");
            var tr = new TagBuilder("tr");

            if (settings.ShowRowMenu || settings.EnableEditing)
            {
                var th = new TagBuilder("th");
                th.InnerHtml += "#";
                tr.InnerHtml += th;
            }

            for (int i = 0; i < columns.Length; i++)
            {
                var column = columns[i];
                var th = new TagBuilder("th");
                th.InnerHtml += column;

                if(settings.EnableSorting)
                {
                    th.MergeAttributes(new Dictionary<string, object>() {
                        { "style", "cursor:pointer;" }
                    });

                    var span = new TagBuilder("span");
                    span.MergeAttributes(new Dictionary<string, object>() {
                        { "style", "float:right; line-height:unset;" },
                        { "class", "fa fa-caret-down" }
                    });
                    th.InnerHtml += span;
                }

                tr.InnerHtml += th;
            }
            thead.InnerHtml += tr;

            return thead;
        }
        
        private static TagBuilder getGridViewRows<TModel>(TModel[] data, SxGridViewSettings settings)
        {
            var tbody = new TagBuilder("tbody");
            
            var showRowMenu = Convert.ToBoolean(settings.ShowRowMenu);
            if (showRowMenu)
                tbody.InnerHtml += getGridViewRowMenu<TModel>(settings);

            if(data!=null && data.Any())
            {
                for (int i = 0; i < data.Length; i++)
                {
                    var model = data[i];
                    tbody.InnerHtml += getGridViewRow(model, settings);
                }
            }

            return tbody;
        }

        private static TagBuilder getGridViewRowMenu<Tmodel>(SxGridViewSettings settings)
        {
            var columns = settings != null && settings.Columns != null ? settings.Columns : getModelProperties(typeof(Tmodel));
            var tr = new TagBuilder("tr");

            if (settings.ShowRowMenu || settings.EnableEditing)
            {
                var td = new TagBuilder("td");
                tr.InnerHtml += td;
            }

            for (int i = 0; i < columns.Length; i++)
            {
                var column = columns[i];
                var td = new TagBuilder("td");

                var prop = typeof(Tmodel).GetProperties().First(x => x.Name == column);
                var editor = getColumnEditor(prop);
                td.InnerHtml += editor;

                tr.InnerHtml += td;
            }
            return tr;
        }
        private static TagBuilder getColumnEditor(PropertyInfo propertyInfo)
        {
            var editor = new TagBuilder("input");
            editor.MergeAttribute("style", "width:100%;");
            var propertyType = propertyInfo.PropertyType;
            if (propertyType == typeof(Int32))
            {
                editor.MergeAttributes(new Dictionary<string, object>() {
                    { "name", propertyInfo.Name },
                    { "type", "number" }
                });
                return editor;
            }
            else if(propertyType == typeof(DateTime))
            {
                editor.MergeAttributes(new Dictionary<string, object>() {
                    { "name", propertyInfo.Name },
                    { "type", "datetime" }
                });
                return editor;
            }
            else if (propertyType == typeof(String))
            {
                editor.MergeAttributes(new Dictionary<string, object>() {
                    { "name", propertyInfo.Name },
                    { "type", "text" }
                });
                return editor;
            }
            else return null;
        }

        private static TagBuilder getGridViewRow<TModel>(TModel model, SxGridViewSettings settings)
        {
            var tr = new TagBuilder("tr");
            var columns = settings != null && settings.Columns != null ? settings.Columns : getModelProperties(typeof(TModel));
            if(settings.ShowRowMenu || settings.EnableEditing)
            {
                var td = new TagBuilder("td");

                if(settings.EnableEditing)
                {
                    var div = new TagBuilder("div");

                    var editBtn = new TagBuilder("input");
                    editBtn.MergeAttribute("type", "button");

                    div.InnerHtml += editBtn;

                    td.InnerHtml += div;
                }
                
                tr.InnerHtml += td;
            }

            var props=model.GetType().GetProperties();
            for (int i = 0; i < columns.Length; i++)
            {
                var column=columns[i];
                var td = new TagBuilder("td");
                var value = props.First(x => x.Name == column).GetValue(model);
                td.InnerHtml += value;
                tr.InnerHtml += td;
            }
            return tr;
        }

        private static string[] getModelProperties(Type modelType)
        {
            return modelType.GetProperties().Select(x => x.Name).OrderBy(x => x).ToArray();
        }
    }
}
