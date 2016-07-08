using System;

namespace GE.WebUI.Models
{
    public sealed class VMAuthorAphorism
    {
        public int Id { get; set; }
        public string TitleUrl { get; set; }
        public string Name { get; set; }
        public Guid? PictureId { get; set; }
        public string Foreword { get; set; }
        public string Description { get; set; }
    }
}