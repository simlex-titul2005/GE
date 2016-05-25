using System.ComponentModel.DataAnnotations;

namespace GE.WebAdmin.Models
{
    public sealed class VMEditEmployee
    {
        [MaxLength(128)]
        public string Id { get; set; }
    }
}