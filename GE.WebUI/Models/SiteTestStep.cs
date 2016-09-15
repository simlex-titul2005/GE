using System.ComponentModel.DataAnnotations.Schema;

namespace GE.WebUI.Models
{
    [NotMapped]
    public sealed class SiteTestStep
    {
        public SiteTestQuestion Question { get; set; }
        public int Order { get; set; }
    }
}
