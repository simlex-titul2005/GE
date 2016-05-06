using System;

namespace GE.WebAdmin.Models
{
    public sealed class VMVideo
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid PictureId { get; set; }
        public string Url { get; set; }
        public string SourceUrl { get; set; }
    }
}