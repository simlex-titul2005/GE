namespace GE.WebUI.Models
{
    public sealed class VMLCNB
    {
        public VMLCNB()
        {
            Categories = new VMLCNBCategory[0];
        }

        public VMLCNBCategory[] Categories { get; set; }
    }
}