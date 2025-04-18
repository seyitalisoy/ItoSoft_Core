using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Constants
{
    public static class Messages
    {
        public static string ProductAdded = "Urun basariyla eklendi.";
        public static string CategoryAdded = "Category basariyla eklendi.";
        public static string ProductNameInvalid = "Urun ismi gecersiz";
        public static string MaintenanceTime = "Sistem bakimda";
        public static string ProductListed = "Urunler listelendi";
        public static string ProductUpdated = "Product Updated";
        public static string ProductCountOfCategoryError = "Bir kategoride en fazla 10 ürün olabilir. ";
        public static string ProductNameAlreadyExists = "Bu isimde zaten başka bir ürün var";
        public static string CategoryNameAlreadyExists = "Bu isimde zaten başka bir kategori var";
        public static string CategoryLimitExceded = "Kategori limiti aşıldı.";

        public static string UserNotFound = "Kullanıcı bulunamadı";
        public static string PasswordError = "Şifre hatalı";
        public static string SuccessfulLogin = "Sisteme giriş başarılı";
        public static string UserAlreadyExists = "Bu kullanıcı zaten mevcut";
        public static string UserRegistered = "Kullanıcı başarıyla kaydedildi";
        public static string AccessTokenCreated = "Access token başarıyla oluşturuldu";

        public static string AuthorizationDenied = "Yetkiniz yok";
        
    }
}
