//using Business.Identity.Localizations;
//using DataAccess.Identity;
//using Entities.Concrete.Identity;
//using Microsoft.AspNetCore.Identity;
//using System;

//namespace UI.Extensions
//{
//    public static class StartupExtensions
//    {
//        public static void ConfigureIdentity(this IServiceCollection services)
//        {
//            services.Configure<DataProtectionTokenProviderOptions>(opt =>
//            {
//                opt.TokenLifespan = TimeSpan.FromHours(2);
//            });

//            services.AddIdentity<AppUser, AppRole>(options =>
//            {
//                options.User.RequireUniqueEmail = true;
//                options.User.AllowedUserNameCharacters = "abcdefghijklmnoprstuvwxyz1234567890_";

//                options.Password.RequiredLength = 6;
//                options.Password.RequireNonAlphanumeric = false;
//                options.Password.RequireLowercase = true;
//                options.Password.RequireUppercase = false;
//                options.Password.RequireDigit = false;

//                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
//                options.Lockout.MaxFailedAccessAttempts = 3;

//            }).AddUserValidator<UserValidator>()
//                .AddPasswordValidator<PasswordValidator>()
//                .AddErrorDescriber<LocalizationIdentityErrorDescriber>()
//                .AddDefaultTokenProviders()
//                .AddEntityFrameworkStores<IdentityContext>();

//        }
//    }
//}
