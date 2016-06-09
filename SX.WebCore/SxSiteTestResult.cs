using SX.WebCore.Abstract;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SX.WebCore
{
    [Table("D_SITE_TEST_RESULT")]
    public class SxSiteTestResult : SxDbModel<Guid>
    {
        public virtual SxSiteTest Test { get; set; }
        public int TestId { get; set; }

        public virtual SxSiteTestBlock Block { get; set; }
        public int BlockId { get; set; }

        public virtual SxSiteTestQuestion Question { get; set; }
        public int QuestionId { get; set; }

        public bool Result { get; set; }
    }
}
