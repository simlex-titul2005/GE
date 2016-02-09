using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GE.WebAdmin
{
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