using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GE.WebAdmin.Models
{
    public sealed class VMEditRedirect
    {
        public Guid? Id { get; set; }

        [MaxLength(255), Required, Display(Name="Старый адрес")]
        public string OldUrl { get; set; }

        [MaxLength(255), Required, Display(Name="Новый адрес")]
        public string NewUrl { get; set; }
    }
}