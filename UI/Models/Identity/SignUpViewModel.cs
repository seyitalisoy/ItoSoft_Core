using System.ComponentModel.DataAnnotations;

namespace UI.Models.Identity
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "Ad boş geçilemez")]
        [Display(Name = "Ad : ")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyad boş geçilemez")]
        [Display(Name = "Soyad : ")]
        public string LastName { get; set; }

        [Required(ErrorMessage ="Kullanıcı adı boş geçilemez")]
        [Display(Name ="Kullanıcı adı : ")]
        public string UserName { get; set; }
        [EmailAddress(ErrorMessage = "Email formatı yanlıştır")]
        [Required(ErrorMessage ="Mail boş geçilemez")]
        [Display(Name ="Email : ")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Telefon numarası boş geçilemez")]
        [Display(Name ="Telefon : ")]
        public string Phone { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage ="Şifre alanı boş geçilemez")]
        [Display(Name ="Şifre : ")]
        [MinLength(6,ErrorMessage ="Şifreniz en az 6 karakter olmalıdır")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password),ErrorMessage ="Şifreler uyuşmuyor")]
        [Required(ErrorMessage = "Şifre tekrar alanı boş geçilemez")]
        [Display(Name = "Şifre tekrar : ")]
        [MinLength(6, ErrorMessage = "Şifreniz en az 6 karakter olmalıdır")]
        public string PasswordConfirm { get; set; }
    }
}
