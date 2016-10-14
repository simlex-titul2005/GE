using System.ComponentModel.DataAnnotations;

namespace GE.WebUI.ViewModels
{
    public sealed class VMSiteTestSetting
    {
        public int TestId { get; set; }

        [Display(Name ="Время чтения знаков в секунду")]
        public int? LettersInSecond { get; set; }

        [Display(Name = "Кол-во баллов, онимаемых каждую секунду после завершения времени на ответ")]
        public int? OnEndTimeBalsCount { get; set; }

        [Display(Name = "Кол-во баллов, начисляемое за каждую секунду, отведенную на ответ")]
        public int? BalsForOneSecond { get; set; }

        [Display(Name = "Время на ответ по-умолчанию")]
        public int? DefQuestionSeconds { get; set; }

        [Display(Name ="Кол-вот баллов, начисляемое за правильный ответ")]
        public int? DefCorrectAnswerBals { get; set; }
    }
}