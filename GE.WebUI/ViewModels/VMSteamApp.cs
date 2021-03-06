﻿using System.ComponentModel.DataAnnotations;

namespace GE.WebUI.ViewModels
{
    public sealed class VMSteamApp
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(SX.WebCore.Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(400, ErrorMessageResourceType = typeof(SX.WebCore.Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        public string Name { get; set; }
    }
}