using SX.WebCore.Abstract;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SX.WebCore
{
    [Table("D_SITE_TEST_RESULT")]
    public class SxSiteTestResult : SxDbModel<Guid>
    {
        public virtual SxSiteTestQuestion Question { get; set; }
        public int QuestionId { get; set; }

        [Index]
        public DateTime DateAnswer { get; set; }

        public bool Result { get; set; }
    }
}
