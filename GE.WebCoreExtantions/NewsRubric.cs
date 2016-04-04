using SX.WebCore.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GE.WebCoreExtantions
{
    [Table("D_NEWS_RUBRIC")]
    public class NewsRubric : SxDbModel<string>
    {
        [MaxLength(255)]
        public string Description { get; set; }
    }
}
