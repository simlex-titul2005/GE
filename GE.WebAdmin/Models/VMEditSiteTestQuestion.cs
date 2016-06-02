using System.ComponentModel.DataAnnotations;

namespace GE.WebAdmin.Models
{
    public sealed class VMEditSiteTestQuestion
    {
        public int Id { get; set; }

        public VMSiteTest Test { get; set; }

        [Required, Display(Name ="Тест"), UIHint("EditSiteTest")]
        public int TestId { get; set; }

        public VMSiteTestBlock Block { get; set; }

        [Required, Display(Name ="Блок"), UIHint("EditSiteTestBlock")]
        public int BlockId { get; set; }

        [Required, MaxLength(400), DataType(DataType.MultilineText), Display(Name ="Вопрос")]
        public string Text { get; set; }

        [Required, Display(Name ="Пометить правильным")]
        public bool IsCorrect { get; set; }
    }
}