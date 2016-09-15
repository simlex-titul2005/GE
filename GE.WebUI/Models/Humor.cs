using SX.WebCore.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GE.WebUI.Models
{
    [Table("D_HUMOR")]
    public class Humor : SxMaterial
    {
        [MaxLength(100)]
        public string UserName { get; set; }
    }
}