namespace GE.WebUI.Models
{
    public sealed class VMLCNBNews : Abstract.VMLastMaterial
    {
        public string CategoryId { get; set; }
        public VMLCNBCategory Category { get; set; }
    }
}