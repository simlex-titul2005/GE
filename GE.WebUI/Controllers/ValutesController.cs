using SX.WebCore.ViewModels;
using System;
using System.Linq;
using System.Runtime.Caching;
using System.Web.Mvc;
using System.Web.SessionState;
using System.Xml.Linq;

namespace GE.WebUI.Controllers
{
    public partial class ValutesController : BaseController
    {
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

        [HttpPost]
        public virtual JsonResult GetCurCourse(string cc)
        {
            var strD = DateTime.Now.ToString("dd/MM/yyyy");
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
                }).SingleOrDefault(x=>x.CharCode== cc);

            return Json(data);
        }
    }
}