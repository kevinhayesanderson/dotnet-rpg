using System;
using System.Security.Cryptography;

namespace dotnet_rpg.Data
{
    public static class Utility
    {
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        
        private const int SaltSize = 16;
        private const int HashSize = 20;
        public static string Hash(string password) => Hash(password, 10000);
        private static bool IsHashSupported(string hashString) => hashString.Contains("$DNEFSA$V1$");
        public static string Hash(string password, int iterations)
        {
            byte[] salt = new byte[SaltSize];
            new RNGCryptoServiceProvider().GetBytes(salt);
            byte[] bytes = new Rfc2898DeriveBytes(password, salt, iterations).GetBytes(HashSize);
            byte[] inArray = new byte[36];
            Array.Copy((Array)salt, 0, (Array)inArray, 0, 16);
            byte[] numArray = inArray;
            Array.Copy((Array)bytes, 0, (Array)numArray, 16, 20);
            string base64String = Convert.ToBase64String(inArray);
            return $"$DNEFSA$V1${(object)iterations}${(object)base64String}";
        }

        public static bool Verify(string password, string hashedPassword)
        {
            string[] strArray = IsHashSupported(hashedPassword) ? hashedPassword.Replace("$DNEFSA$V1$", "").Split('$') : throw new NotSupportedException("The hashtype is not supported");
            int iterations = int.Parse(strArray[0]);
            byte[] numArray = Convert.FromBase64String(strArray[1]);
            byte[] salt = new byte[16];
            Array.Copy((Array)numArray, 0, (Array)salt, 0, 16);
            byte[] bytes = new Rfc2898DeriveBytes(password, salt, iterations).GetBytes(20);
            for (int index = 0; index < 20; ++index)
            {
                if ((int)numArray[index + 16] != (int)bytes[index])
                    return false;
            }
            return true;
        }
    }
}