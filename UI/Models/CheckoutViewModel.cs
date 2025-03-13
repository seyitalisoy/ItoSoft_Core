using System.ComponentModel.DataAnnotations;

namespace UI.Models
{
    public class CheckoutViewModel
    {

        //public string UserName { get; set; }
        //public string Name { get; set; }
        //public string Phone { get; set; }
        [Required(ErrorMessage = "Adres alanı boş bırakılamaz.")]
        [Display(Name = "Adres :")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Lütfen adres başlığı giriniz.")]
        [Display(Name = "Adres Başlığı :")]
        public string AddressTitle { get; set; }
    }

}
