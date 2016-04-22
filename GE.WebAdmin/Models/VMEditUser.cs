﻿using System;
using System.ComponentModel.DataAnnotations;

namespace GE.WebAdmin.Models
{
    public sealed class VMEditUser
    {
        public VMEditUser()
        {
            Roles = new VMRole[0];
        }

        public string Id { get; set; }
        public Guid? AvatarId { get; set; }
        public string Email { get; set; }

        [Display(Name ="Никнейм"), MaxLength(50)]
        public string NikName { get; set; }

        public VMRole[] Roles { get; set; }
        public bool IsOnline { get; set; }
    }
}