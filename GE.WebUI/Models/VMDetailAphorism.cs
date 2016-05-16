namespace GE.WebUI.Models
{
    public sealed class VMDetailAphorism
    {
        public VMDetailAphorism()
        {
            TopForAuthor = new VMAphorism[0];
            TopForCategory = new VMAphorism[0];
        }

        public VMAphorism Aphorism { get; set; }

        public VMAphorism[] TopForAuthor { get; set; }

        public VMAphorism[] TopForCategory { get; set; }
    }
}