using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GE.WebAdmin.Models
{
    public sealed class VMEditGame : ISxViewModel<int>
    {
        public int Id { get; set; }
        public DateTime DateCreate { get; set; }
        [Required]
        public string Title { get; set; }
        [DataType(DataType.MultilineText), Required]
        public string Description { get; set; }
        public bool Show { get; set; }
    }
}