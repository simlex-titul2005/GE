using System.Collections.Generic;
using System.Web.Http;

namespace GE.WebUI.Controllers.Api
{
    public class LogsController : ApiController
    {
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
