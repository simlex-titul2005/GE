using SX.WebCore.DbModels;
using SX.WebCore.DbModels.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GE.WebUI.Models
{
    [Table("D_SITE_TEST_SUBJECT")]
    public class SiteTestSubject : SxDbUpdatedModel<int>
    {
        [Required, MaxLength(400), Index]
        public string Title { get; set; }

        public string Description { get; set; }

        public virtual SiteTest Test { get; set; }
        public int TestId { get; set; }

        public virtual SxPicture Picture { get; set; }
        public Guid? PictureId { get; set; }
    }
}
