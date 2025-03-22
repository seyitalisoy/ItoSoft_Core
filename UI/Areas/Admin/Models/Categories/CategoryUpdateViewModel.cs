using System.ComponentModel.DataAnnotations;

namespace UI.Areas.Admin.Models.Categories
{
    public class CategoryUpdateViewModel
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Kategori adı boş geçilemez")]
        [Display(Name = "Kategori adı")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Açıklama boş geçilemez")]
        [Display(Name = "Açıklama")]
        public string Description { get; set; }
    }
}
