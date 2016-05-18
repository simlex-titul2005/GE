using System;

namespace GE.WebUI.Models
{
    public sealed class VMUser
    {
        public string Id { get; set; }
        public string NikName { get; set; }
        public Guid? AvatarId { get; set; }
    }
}