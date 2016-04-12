namespace GE.WebUI.Models
{
    public sealed class VMMenu
    {
        public VMMenu()
        {
            Items=new VMMenuItem[0];
        }

        public VMMenuItem[] Items { get; set; }
    }
}