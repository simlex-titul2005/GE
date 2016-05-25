﻿using static SX.WebCore.Enums;

namespace GE.WebAdmin.Models
{
    public sealed class VMAphorism
    {
        public int Id { get; set; }
        public ModelCoreType ModelCoreType { get; set; }
        public string Title { get; set; }
        public bool Show { get; set; }
        public string CategoryId { get; set; }
        public int AuthorId { get; set; }
        public VMAuthorAphorism Author { get; set; }
        public string Html { get; set; }

        public string Name { get; set; }
    }
}