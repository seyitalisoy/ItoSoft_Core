using System.ComponentModel.DataAnnotations;

namespace UI.Models.Identity
{
    public class EditUserViewModel
    {
        [Display(Name = "İsim:")]
        public string FirstName { get; set; } = null!;


        [Display(Name = "Soyisim:")]
        public string LastName { get; set; } = null!;


        [Display(Name = "Kullanıcı Adı :")]
        public string UserName { get; set; } = null!;


        [Display(Name = "Email :")]
        public string Email { get; set; } = null!;


        [Display(Name = "Telefon:")]
        public string Phone { get; set; } = null!;

        [Display(Name = "Adres 1:")]
        public string? Adress1 { get; set; }

        [Display(Name = "Adres 2:")]
        public string? Adress2 { get; set; }

        [Display(Name = "Adres Başlığı 1:")]
        public string? AdressTitle1 { get; set; }

        [Display(Name = "Adres Başlığı 2:")]
        public string? AdressTitle2 { get; set; }
    }
}
