using SX.WebCore.Abstract;
using System.ComponentModel.DataAnnotations.Schema;

namespace GE.WebUI.Models
{
    [Table("D_APHORISM")]
    public class Aphorism : SxMaterial
    {
        public virtual AuthorAphorism Author { get; set; }
        public int? AuthorId { get; set; }
    }
}