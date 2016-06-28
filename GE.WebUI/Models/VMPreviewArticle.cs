using GE.WebUI.Models.Abstract;

namespace GE.WebUI.Models
{
    public class VMPreviewArticle : VMLastMaterial
    {
        public string UserName { get; set; }
        public string GameTitle { get; set; }
        public VMMateriallnfo Info
        {
            get
            {
                return new VMMateriallnfo
                {
                    CommentsCount=this.CommentsCount,
                    DateOfPublication =this.DateOfPublication,
                    ViewsCount=this.ViewsCount
                };
            }
        }
    }
}