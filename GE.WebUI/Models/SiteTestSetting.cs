using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GE.WebUI.Models
{
    [Table("D_SITE_TEST_SETTING")]
    public class SiteTestSetting
    {
        public int TestId { get; set; }
        public virtual SiteTest Test { get; set; }

        public DateTime DateCreate { get; set; }

        public DateTime DateUpdate { get; set; }

        /// <summary>
        /// Скорость чтения (знаков в секунду)
        /// </summary>
        public int? LettersInSecond { get; set; }

        /// <summary>
        /// Кол-во баллов, онимаемых каждую секунду после завершения времени на ответ
        /// </summary>
        public int? OnEndTimeBalsCount { get; set; }

        /// <summary>
        /// Кол-во баллов, начисляемое за каждую секунду, отведенную на ответ
        /// </summary>
        public int? BalsForOneSecond { get; set; }

        /// <summary>
        /// Время на ответ по-умолчанию
        /// </summary>
        public int? DefQuestionSeconds { get; set; }

        /// <summary>
        /// Кол-вот баллов, начисляемое за правильный ответ"
        /// </summary>
        public int? DefCorrectAnswerBals { get; set; }
    }
}