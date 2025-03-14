using System.ComponentModel.DataAnnotations;

namespace UI.Models
{
    public class CheckoutViewModel
    {

        [Required(ErrorMessage = "Adres alanı boş bırakılamaz.")]
        [Display(Name = "Adres :")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Lütfen adres başlığı giriniz.")]
        [Display(Name = "Adres Başlığı :")]
        public string AddressTitle { get; set; }

        [EmailAddress(ErrorMessage = "Email formatı yanlıştır")]
        [Required(ErrorMessage = "Lütfen Email giriniz.")]
        [Display(Name = "Email :")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Lütfen ad giriniz.")]
        [Display(Name = "Ad :")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Lütfen soyad giriniz.")]
        [Display(Name = "Soyad :")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Lütfen telefon numarası giriniz.")]
        [Display(Name = "Tel No:")]
        public string Phone { get; set; }
    }

}
