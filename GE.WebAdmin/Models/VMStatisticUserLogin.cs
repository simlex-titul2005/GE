using System;

namespace GE.WebAdmin.Models
{
    public sealed class VMStatisticUserLogin
    {
        public DateTime DateCreate { get; set; }
        public string NikName { get; set; }
        public Guid? AvatarId { get; set; }
    }
}