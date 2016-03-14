using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GE.WebAdmin.Models
{
    public sealed class VMEditArticleType : ISxViewModel<int>
    {
        public int Id { get; set; }

        [Display(Name = "Описание"), MaxLength(255), Required, DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Название типа"), MaxLength(150), Required, RegularExpression(@"^[A-Za-z0-9]([-]*[A-Za-z0-9])*$", ErrorMessage = "Поле должно содержать только буквы латинского алфавита и тире")]
        public string Name { get; set; }

        [Display(Name = "Игра"), UIHint("EditGame"), Required]
        public int GameId { get; set; }
        public VMGame Game { get; set; }
    }
}