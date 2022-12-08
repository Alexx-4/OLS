using System;
using System.Text;

namespace OpenLatino.Admin.Application.Utils
{
    public static class Ol_PasswordHasher
    {
        private static readonly string salt = "passwordH@SH-Creat3d-BY-FRANKIE!!!!";
        public static string getHash(string password)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(Encoding.UTF8.GetBytes(salt)))
            {
                string result = string.Concat(Encoding.UTF8.GetChars(hmac.ComputeHash(Encoding.UTF8.GetBytes(password))));
                return result;
            }
        }
        public static string getPassword(string hash)
        {
            throw new NotImplementedException();
        }
    }
}
