using System.Collections.Generic;

namespace GE.WebUI.Models
{
    public sealed class VMSiteTestSession
    {
        public VMSiteTestSession()
        {
            Blocks = new List<VMSiteTestSessionBlock>();
        }

        public int TestId { get; set; }
        public List<VMSiteTestSessionBlock> Blocks { get; set; }
    }

    public sealed class VMSiteTestSessionBlock
    {
        public VMSiteTestSessionBlock()
        {
            Questions = new List<VMSiteTestSessionQuestion>();
        }

        public int BlockId { get; set; }
        public List<VMSiteTestSessionQuestion> Questions { get; set; }
    }

    public sealed class VMSiteTestSessionQuestion
    {
        public int QuestionId { get; set; }
        public bool Result { get; set; }
    }
}