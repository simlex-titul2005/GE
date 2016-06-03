using System.Linq;

namespace GE.WebUI.Models
{
    public sealed class VMSiteTest
    {
        public VMSiteTest()
        {
            Blocks = new VMSiteTestBlock[0];
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public VMSiteTestBlock[] Blocks { get; set; }

        public int QuestionsCount
        {
            get
            {
                return Blocks.Sum(x => x.Questions.Length);
            }
        }
    }
}