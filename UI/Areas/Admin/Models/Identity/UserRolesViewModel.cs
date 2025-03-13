namespace UI.Areas.Admin.Models.Identity
{
    public class UserRolesViewModel
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public List<SelectableRoleViewModel> Roles { get; set; }
    }

    public class SelectableRoleViewModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsAssigned { get; set; }
    }
}
