using System.ComponentModel.DataAnnotations;

namespace UI.Models.Identity
{
    public class ChangePasswordViewModel
    {

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Eski şifre alanı boş geçilemez")]
        [Display(Name = "Eski Şifre : ")]
        [MinLength(6, ErrorMessage = "Eski şifre en az 6 karakter olmalıdır")]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Yeni şifre alanı boş geçilemez")]
        [Display(Name = "Yeni Şifre : ")]
        [MinLength(6, ErrorMessage = "Yeni şifreniz en az 6 karakter olmalıdır")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Şifreler uyuşmuyor")]
        [Required(ErrorMessage = "Şifre tekrar alanı boş geçilemez")]
        [Display(Name = "Şifre tekrar : ")]
        [MinLength(6, ErrorMessage = "Yeni şifreniz en az 6 karakter olmalıdır")]
        public string NewPasswordConfirm { get; set; }
    }
}
