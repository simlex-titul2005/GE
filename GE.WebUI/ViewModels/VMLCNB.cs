namespace GE.WebUI.ViewModels
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