using System;

namespace GE.WebAdmin.Models
{
    public sealed class VMAuthorAphorism
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? PictureId { get; set; }
    }
}