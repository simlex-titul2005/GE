using SX.WebCore;
using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GE.WebCoreExtantions
{
    [Table("D_AUTHOR_APHORISM")]
    public class AuthorAphorism : SxDbUpdatedModel<int>
    {
        [Required, MaxLength(110), Index]
        public string TitleUrl { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, MaxLength(400)]
        public string Foreword { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Aphorism> Aphorisms { get; set; }

        public virtual SxPicture Picture { get; set; }
        public Guid? PictureId { get; set; }
    }
}
