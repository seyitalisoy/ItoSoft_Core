﻿namespace UI.Areas.Admin.Models.Identity
{
    public class UserRoleViewModel
    {
        public string UserId { get; set; } 
        public string UserName { get; set; } 
        public List<string> Roles { get; set; } 
    }
}
