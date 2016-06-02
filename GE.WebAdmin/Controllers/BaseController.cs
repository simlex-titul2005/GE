using AutoMapper;
using System.Web.Mvc;

namespace GE.WebAdmin
{
    [Authorize]
    public abstract partial class BaseController : Controller
    {
        private IMapper _mapper;
        public BaseController()
        {
            _mapper = MvcApplication.MapperConfiguration.CreateMapper();
        }

        protected IMapper Mapper
        {
            get
            {
                return _mapper;
            }
        }
    }
}