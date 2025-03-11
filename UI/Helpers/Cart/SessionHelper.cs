using Newtonsoft.Json;

namespace UI.Helpers.Cart
{
    public static class SessionHelper
    {
        // Session'dan veri okuma
        public static T Get<T>(ISession session, string key)
        {
            var value = session.GetString(key);
            if (value == null)
                return default(T);

            return JsonConvert.DeserializeObject<T>(value);
        }

        // Session'a veri yazma
        public static void Set<T>(ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        // Session'dan veri silme
        public static void Remove(ISession session, string key)
        {
            session.Remove(key);
        }
    }



}
