using SX.WebCore.DbModels.Abstract;
using System.ComponentModel.DataAnnotations.Schema;

namespace GE.WebUI.Models
{
    [Table("D_SITE_TEST_ANSWER")]
    public class SiteTestAnswer : SxDbUpdatedModel<int>
    {
        public virtual SiteTestQuestion Question{ get; set; }
        public int QuestionId { get; set; }

        public virtual SiteTestSubject Subject { get; set; }
        public int SubjectId { get; set; }

        public int IsCorrect { get; set; }
    }
}
