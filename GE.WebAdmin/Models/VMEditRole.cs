using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GE.WebAdmin.Models
{
    public sealed class VMEditRole : ISxViewModel<string>
    {
        public string Id { get; set; }

        [Display(Name="Наименование"), MaxLength(256), RegularExpression(@"^[A-Za-z0-9]([-]*[A-Za-z0-9])*$", ErrorMessage = "Поле должно содержать только буквы латинского алфавита и тире")]
        public string Name { get; set; }

        [Display(Name = "Описание"), MaxLength(256), DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}