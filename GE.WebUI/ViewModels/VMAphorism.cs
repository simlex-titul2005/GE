using GE.WebUI.ViewModels.Abstracts;
using SX.WebCore.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace GE.WebUI.ViewModels
{
    [MetadataType(typeof(VMAphorismMetadata))]
    public sealed class VMAphorism : VMMaterial
    {
        [Display(Name ="Автор"), UIHint("_AuthorAphorismLookupGrid")]
        public int? AuthorId { get; set; }

        public VMAuthorAphorism Author { get; set; }
        public string AuthorName { get; set; }

        /// <summary>
        /// Флаг, указывающий на принадлежность к автору - 1, категории - 2 или выбранному афоризму - 0
        /// </summary>
        public AphorismFlag Flag { get; set; }

        public enum AphorismFlag : byte
        {
            ForThis = 0,
            ForAuthor = 1,
            ForCategory = 2
        }
    }

    public sealed class VMAphorismMetadata : SxVMMaterialMetadata
    {
        [Required(ErrorMessageResourceType = typeof(SX.WebCore.Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Категория материала"), UIHint("_MaterialCategoryLookupGrid")]
        public new string CategoryId { get; set; }
    }
}