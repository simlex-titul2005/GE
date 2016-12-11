using SX.WebCore.DbModels.Abstract;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GE.WebUI.Models
{
    [Table("D_SITE_TEST")]
    public class SiteTest : SxDbUpdatedModel<int>
    {
        [Required, MaxLength(200)]
        public string Title { get; set; }

        [Required, MaxLength(255), Index]
        public string TitleUrl { get; set; }

        [Required, MaxLength(1000)]
        public string Description { get; set; }

        public bool Show { get; set; }

        public string Rules { get; set; }

        public virtual ICollection<SiteTestQuestion> Questions { get; set; }
        public virtual ICollection<SiteTestSubject> Answers { get; set; }

        public SiteTestType Type { get; set; }

        public enum SiteTestType : byte
        {
            /// <summary>
            /// Угадыватель
            /// </summary>
            Guess = 0,

            /// <summary>
            /// Обычный тест (один вопрос - несколько вариантов ответов)
            /// </summary>
            Normal = 1,

            /// <summary>
            /// Обычный тест (один вопрос - несколько вариантов ответов, только в качестве объектов используется картинка)
            /// </summary>
            NormalImage = 2
        }

        public int ViewsCount { get; set; }

        public bool ShowSubjectDesc { get; set; }

        public virtual SiteTestSetting Settings { get; set; }

        public bool ViewOnMainPage { get; set; }
    }
}