using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NaiveCoin
{
    public static class Extensions
    {
        public static string GetSha256(this string str)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] sha256Bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(str));
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < sha256Bytes.Length; i++)
                {
                    sb.Append(sha256Bytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        public static bool IsEqual(this string str, string other)
        {
            StringComparer sc = StringComparer.Ordinal;
            return sc.Compare(str, other) == 0;
        }

        public static bool VerifySha256(this string str, string sha256)
        {
            string check256 = str.GetSha256();
            StringComparer sc = StringComparer.Ordinal;
            return sc.Compare(sha256, check256) == 0;
        }

        public static bool VerifySha256(this string str, string sha256, out string hashed)
        {
            string check256 = str.GetSha256();
            StringComparer sc = StringComparer.Ordinal;
            hashed = check256;
            return sc.Compare(sha256, check256) == 0;
        }
    }
}
