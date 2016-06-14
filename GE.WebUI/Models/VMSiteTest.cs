using System.Collections.Generic;
using System.Linq;

namespace GE.WebUI.Models
{
    public sealed class VMSiteTest
    {
        public VMSiteTest()
        {
            Blocks = new VMSiteTestBlock[0];
        }

        public string Title { get; set; }
        public string TitleUrl { get; set; }
        public string Description { get; set; }

        public VMSiteTestBlock[] Blocks { get; set; }

        public string[] QuestionTexts
        {
            get
            {
                var list = new List<VMSiteTestQuestion>();
                VMSiteTestBlock block = null;
                VMSiteTestQuestion question = null;
                for (int i = 0; i < Blocks.Length; i++)
                {
                    block = Blocks[i];
                    for (int y = 0; y < block.Questions.Length; y++)
                    {
                        question = block.Questions[y];
                        list.Add(question);
                    }
                }

                return list.Select(x=>x.Text).Distinct().ToArray();
            }
        }

        public string GetRandomQuestion(string[] texts=null)
        {
            var data = QuestionTexts.Where(x => texts==null || !texts.Contains(x));
            return data.FirstOrDefault();
        }
    }
}