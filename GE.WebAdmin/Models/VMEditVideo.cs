using System;
using System.ComponentModel.DataAnnotations;

namespace GE.WebAdmin.Models
{
    public sealed class VMEditVideo
    {
        public Guid Id { get; set; }

        [Required, MaxLength(255), Display(Name ="Название видео")]
        public string Title { get; set; }

        [Required, MaxLength(20), Display(Name = "Идентификатор видео")]
        public string VideoId { get; set; }

        [MaxLength(255), DataType(DataType.Url), Display(Name = "Источник видео")]
        public string SourceUrl { get; set; }
    }
}