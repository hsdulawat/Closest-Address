using System;
using System.Web;

namespace ClosestAddress.Helper
{
    public class CacheHelper
    {
        public static void Add<T>(T o, string key, double chacheDuration)
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Cache.Insert(
                    key,
                    o,
                    null,
                    DateTime.Now.AddMinutes(chacheDuration),
                    System.Web.Caching.Cache.NoSlidingExpiration);
            }
        }
        public static void Clear(string key)
        {
            if (HttpContext.Current != null)
            {
                if (Exists(key))
                {
                    HttpContext.Current.Cache.Remove(key);
                }
            }
        }
        public static bool Exists(string key)
        {
            if (HttpContext.Current == null)
                return false;

            return HttpContext.Current.Cache[key] != null;
        }
        public static bool Get<T>(string key, out T value)
        {
            if (HttpContext.Current == null)
            {
                value = default(T);
                return false;
            }

            try
            {
                if (!Exists(key))
                {
                    value = default(T);
                    return false;
                }

                value = (T)HttpContext.Current.Cache[key];
            }
            catch
            {
                value = default(T);
                return false;
            }
            return true;
        }
    }
}
