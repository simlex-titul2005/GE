using System.ComponentModel.DataAnnotations;

namespace GE.WebAdmin.Models
{
    public sealed class VMEditClickType
    {
        public int Id { get; set; }

        [MaxLength(255), Required, Display(Name = "Наименование типа статистики кликов")]
        public string Name { get; set; }

        [MaxLength(255), Display(Name = "Описание типа статистики кликов")]
        public string Description { get; set; }
    }
}