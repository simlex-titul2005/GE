using System;

namespace GE.WebUI.ViewModels
{
    public sealed class VMSteamApp
    {
        public Guid? Id { get; set; }

        public int? AppId { get; set; }

        public string Name { get; set; }
    }
}