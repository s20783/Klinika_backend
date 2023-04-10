using Application.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class PasswordService : IPassword
    {
        public string HashPassword(byte[] salt, string plainPassword, int iterations)
        {
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: plainPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: iterations,
                numBytesRequested: 512 / 8));
            return hashedPassword;
        }

        public byte[] GenerateSalt()
        {
            byte[] salt = new byte[256 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        public string GetRandomPassword(int l)
        {
            StringBuilder password = new StringBuilder();
            string validUpper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string validDigits = "1234567890";
            string validLower = "abcdefghijklmnopqrstuvwxyz";
            for (int i = 0; i < l/3; i++)
            {
                password.Append(validUpper.ElementAt(new Random().Next(0, validUpper.Length)));
            }

            for (int i = 0; i < l/3; i++)
            {
                password.Append(validDigits.ElementAt(new Random().Next(0, validDigits.Length)));
            }

            for (int i = 0; i < l/3; i++)
            {
                password.Append(validLower.ElementAt(new Random().Next(0, validLower.Length)));
            }

            if(l%3 > 0)
            {
                string allRequiredChars = validLower + validUpper + validDigits;
                for (int i = 0; i < l % 3; i++)
                {
                    password.Append(allRequiredChars.ElementAt(new Random().Next(0, allRequiredChars.Length)));
                }
            }

            return password.ToString();
        }
    }
}