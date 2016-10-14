using SX.WebCore.DbModels.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GE.WebUI.Models
{
    [Table("D_SITE_TEST_QUESTION")]
    public class SiteTestQuestion : SxDbUpdatedModel<int>
    {
        [Required, MaxLength(500)]
        public string Text { get; set; }

        public int TestId { get; set; }
        public virtual SiteTest Test { get; set; }
    }
}
