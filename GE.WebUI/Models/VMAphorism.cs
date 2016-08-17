namespace GE.WebUI.Models
{
    public sealed class VMAphorism : VMMaterial
    {
        public VMAuthorAphorism Author { get; set; }
        public int? AuthorId { get; set; }

        public string SourceUrl { get; set; }

        /// <summary>
        /// Флаг, указывающий на принадлежность к автору - 1, категории - 2 или выбранному афоризму - 0
        /// </summary>
        public AphorismFlag Flag { get; set; }

        public enum AphorismFlag : byte
        {
            ForThis=0,
            ForAuthor=1,
            ForCategory=2
        }
    }
}