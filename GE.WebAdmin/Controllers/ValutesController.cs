using System;
using System.Web.Mvc;
using System.Xml.Linq;
using System.Linq;
using SX.WebCore.ViewModels;
using System.Runtime.Caching;
using System.Collections.Generic;
using SX.WebCore.HtmlHelpers;

namespace GE.WebAdmin.Controllers
{
    public partial class ValutesController : BaseController
    {
        private static readonly int _pageSize = 40;
        private static MemoryCache _cache;
        private static CacheItemPolicy _defaultPolicy
        {
            get
            {
                return new CacheItemPolicy
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddHours(2)
                };
            }
        }

        public ValutesController()
        {
            _cache = _cache ?? new MemoryCache("GE_CACHE_VALUTES");
        }

        [HttpGet]
        public virtual ActionResult Index(int page = 1, DateTime? date = null)
        {
            var viewModel = getValutes(date);

            var filter = new WebCoreExtantions.Filter(page, _pageSize);
            filter.PagerInfo.TotalItems = viewModel.Length;
            ViewBag.PagerInfo = filter.PagerInfo;

            return View(viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult Index(SxVMValute filterModel, IDictionary<string, SxExtantions.SortDirection> order, int page = 1)
        {
            ViewBag.Filter = filterModel;
            ViewBag.Order = order;

            var viewModel = getValutes(null, filterModel, order);

            var filter = new WebCoreExtantions.Filter(page, _pageSize) { Orders = order, WhereExpressionObject = filterModel };
            filter.PagerInfo.TotalItems = viewModel.Length;
            ViewBag.PagerInfo = filter.PagerInfo;

            return PartialView("_GridView", viewModel);
        }

        private static SxVMValute[] getValutes(DateTime? date = null, SxVMValute filterModel=null, IDictionary<string, SxExtantions.SortDirection> order=null)
        {
            var d = date == null ? DateTime.Now : (DateTime)date;
            var strD = d.ToString("dd/MM/yyyy");
            var url = string.Format("http://www.cbr.ru/scripts/XML_daily.asp?date_req={0}", strD);

            if (_cache["VALUTES"] == null)
                _cache.Add(new CacheItem("VALUTES", XDocument.Load(url)), _defaultPolicy);
            var doc = (XDocument)_cache["VALUTES"];

            var data = doc.Descendants("Valute")
                .Select(x => new SxVMValute
                {
                    Id = x.Attribute("ID").Value,
                    NumCode = Convert.ToInt16(x.Element("NumCode").Value),
                    CharCode = x.Element("CharCode").Value,
                    Nominal = Convert.ToDecimal(x.Element("Nominal").Value),
                    Name = x.Element("Name").Value,
                    Value = Convert.ToDecimal(x.Element("Value").Value)
                }).ToArray();

            if (filterModel != null)
            {
                if (filterModel.Id != null)
                    data = data.Where(x => x.Id.Contains(filterModel.Id)).ToArray();
                if(filterModel.NumCode!=0)
                    data= data.Where(x => x.NumCode>= filterModel.NumCode).ToArray();
                if (filterModel.CharCode != null)
                    data = data.Where(x => x.CharCode.Contains(filterModel.CharCode)).ToArray();
                if (filterModel.Nominal != 0)
                    data = data.Where(x => x.Nominal >= filterModel.Nominal).ToArray();
                if (filterModel.Name != null)
                    data = data.Where(x => x.Name.Contains(filterModel.Name)).ToArray();
                if (filterModel.Value != 0)
                    data = data.Where(x => x.Value >= filterModel.Value).ToArray();
            }

            if (order!=null)
            {
                order = order.Where(x => x.Value != SxExtantions.SortDirection.Unknown).ToDictionary(x=>x.Key, x=>x.Value);

                if (order.ContainsKey("Id"))
                    data = order["Id"] == SxExtantions.SortDirection.Asc ? data.OrderBy(x => x.Id).ToArray() : data.OrderByDescending(x => x.Id).ToArray();
                if (order.ContainsKey("NumCode"))
                    data = order["NumCode"] == SxExtantions.SortDirection.Asc ? data.OrderBy(x => x.NumCode).ToArray() : data.OrderByDescending(x => x.NumCode).ToArray();
                if (order.ContainsKey("CharCode"))
                    data = order["CharCode"] == SxExtantions.SortDirection.Asc ? data.OrderBy(x => x.CharCode).ToArray() : data.OrderByDescending(x => x.CharCode).ToArray();
                if (order.ContainsKey("Nominal"))
                    data = order["Nominal"] == SxExtantions.SortDirection.Asc ? data.OrderBy(x => x.Nominal).ToArray() : data.OrderByDescending(x => x.Nominal).ToArray();
                if (order.ContainsKey("Name"))
                    data = order["Name"] == SxExtantions.SortDirection.Asc ? data.OrderBy(x => x.Name).ToArray() : data.OrderByDescending(x => x.Name).ToArray();
                if (order.ContainsKey("Value"))
                    data = order["Value"] == SxExtantions.SortDirection.Asc ? data.OrderBy(x => x.Value).ToArray() : data.OrderByDescending(x => x.Value).ToArray();
            }

            return data;
        }
    }
}