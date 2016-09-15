using GE.WebUI.ViewModels.Abstracts;
using System.ComponentModel.DataAnnotations;

namespace GE.WebUI.ViewModels
{
    public sealed class VMNews : VMMaterial
    {
        [MaxLength(100, ErrorMessageResourceType = typeof(SX.WebCore.Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        public string GameVersion { get; set; }
    }
}