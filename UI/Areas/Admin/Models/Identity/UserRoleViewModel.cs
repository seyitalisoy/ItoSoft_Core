namespace UI.Areas.Admin.Models.Identity
{
    public class UserRoleViewModel
    {
        public string UserId { get; set; } // Kullanıcı ID'si
        public string UserName { get; set; } // Kullanıcı adı
        public List<string> Roles { get; set; } // Kullanıcının rollerinin listesi
    }
    //public class UserRoleViewModel
    //{
    //    public string UserId { get; set; } // Kullanıcı ID'si
    //    public List<string> SelectedRoles { get; set; } // Kullanıcının sahip olduğu roller
    //}

}
