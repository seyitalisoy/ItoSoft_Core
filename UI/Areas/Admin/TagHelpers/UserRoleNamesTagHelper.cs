//using Entities.Concrete.Identity;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Razor.TagHelpers;

//namespace UI.Areas.Admin.TagHelpers
//{
//    public class UserRoleNamesTagHelper : TagHelper
//    {
//        public string UserId { get; set; }

//        private readonly UserManager<AppUser> _userManager;

//        public UserRoleNamesTagHelper(UserManager<AppUser> userManager)
//        {
//            _userManager = userManager;
//        }

//        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
//        {
//            output.TagName = null;  // HTML etiketini kaldır

//            var user = await _userManager.FindByIdAsync(UserId);
//            if (user == null)
//            {
//                output.Content.SetContent("Kullanıcı bulunamadı");
//                return;
//            }

//            var userRoles = await _userManager.GetRolesAsync(user);

//            // Eğer roller mevcutsa, her birini bir etiket olarak ekleyelim
//            if (userRoles.Any())
//            {
//                //var stringBuilder = new StringBuilder();

//                //userRoles.ToList().ForEach(x =>
//                //{
//                //    // Burada sadece rollerin adlarını içeren bir içerik döndürüyoruz
//                //    stringBuilder.Append(@$"<span class='ui red horizontal label mb-2'>{x.ToUpperInvariant()}</span>");
//                //});

//                //output.Content.SetHtmlContent(stringBuilder.ToString());
//                // Basit bir işaret olarak içerik döndürmek
//                output.Content.SetContent("Rol bulunuyor.");
//            }
//            else
//            {
//                output.Content.SetContent("Bu kullanıcıya atanmış rol bulunmuyor.");
//            }
//        }
//    }
//}
