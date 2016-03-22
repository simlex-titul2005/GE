using SX.WebCore.Abstract;

namespace GE.WebAdmin.Models
{
    public class VMRole : ISxViewModel<string>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}