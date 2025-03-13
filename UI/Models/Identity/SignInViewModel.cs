using System.ComponentModel.DataAnnotations;

namespace UI.Models.Identity
{
    public class SignInViewModel
    {
        [Display(Name = "Email:")]
        public string Email { get; set; }



        [Display(Name = "Şifre:")]
        public string Password { get; set; }

        [Display(Name = "Beni Hatırla")]
        public bool RememberMe { get; set; }
    }
}
