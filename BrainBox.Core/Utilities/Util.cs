using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace BrainBox.Core.Utilities
{
    public class Util
    {
        /// <summary>
        /// Computes a hash
        /// </summary>
        /// <param name="text"></param>
        /// <param name="key"></param>
        /// <returns>string</returns>
        public static string Encrypt(string text, string key)
        {
            byte[] byteArray;
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = new byte[16];

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                            sw.Write(text);
                        byteArray = ms.ToArray();
                    }
                }
            }

            StringBuilder hex = new StringBuilder(byteArray.Length * 2);
            foreach (byte b in byteArray)
            {
                hex.AppendFormat("{0:x2}", b);
            }

            return hex.ToString();
        }

        /// <summary>
        /// Get a cached object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static dynamic GetCachedObject<T>(string service, string key) where T : class
        {
            return ObjectCacheProvider.GetCachedObject<T>($"{service}", $"{key}");
        }

        /// <summary>
        /// Caches an object in memory
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="objectToCache"></param>
        public static void CacheObject<T>(string service, string key, T objectToCache) where T : class
        {
            ObjectCacheProvider.TryCache($"{service}", $"{key}", objectToCache);
        }

        /// <summary>
        /// Get user email from token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string GetTokenUserEmail(string token)
        {
            return ((JwtSecurityToken)(new JwtSecurityTokenHandler()).ReadToken(token)).Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value;
        }

        /// <summary>
        /// Get user Id from token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string GetTokenUserId(string token)
        {
            return ((JwtSecurityToken)(new JwtSecurityTokenHandler()).ReadToken(token)).Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.NameId)?.Value;
        }
    }
}
