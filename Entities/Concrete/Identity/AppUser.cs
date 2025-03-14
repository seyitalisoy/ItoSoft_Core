using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Entities.Concrete.Identity
{
    public class AppUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Adress1 { get; set; }
        public string? Adress2 { get; set; }
        public string? AdressTitle1 { get; set; }
        public string? AdressTitle2 { get; set; }
    }
}
