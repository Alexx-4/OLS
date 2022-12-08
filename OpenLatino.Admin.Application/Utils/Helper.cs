using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace OpenLatino.Admin.Application.Utils
{
    public class Helper
    {
        public static string GetHash(string input)
        {
            var hashAlgorithm = new SHA256CryptoServiceProvider();

            var byteValue = Encoding.UTF8.GetBytes(input);

            var byteHash = hashAlgorithm.ComputeHash(byteValue);

            return Convert.ToBase64String(byteHash);
        }
        public static TimeSpan TIcketLifeTime = TimeSpan.FromDays(1);
    }
}
