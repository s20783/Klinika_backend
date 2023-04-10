using Application.Common.Exceptions;
using Application.Interfaces;
using Domain;
using Domain.Models;
using System;

namespace Infrastructure.Services
{
    public class LoginService : ILogin
    {
        public bool CheckCredentails(Osoba user, IPassword passwordRepository, string haslo, int iterations)
        {
            if (user == null)
            {
                throw new UserNotAuthorizedException("Incorrect");
            }

            if(user.DataBlokady != null)
            {
                if(user.DataBlokady > DateTime.Now)
                {
                    throw new ConstraintException("Account blocked", user.DataBlokady.ToString());
                }
            }
            
            string passwordHash = user.Haslo;
            byte[] salt = Convert.FromBase64String(user.Salt);
            string currentHashedPassword = passwordRepository.HashPassword(salt, haslo, iterations);

            if (!passwordHash.Equals(currentHashedPassword))
            {
                user.LiczbaProb += 1;
                if (user.LiczbaProb >= GlobalValues.LICZBA_PROB)
                {
                    user.DataBlokady = DateTime.Now.AddHours(GlobalValues.GODZINY_BLOKADY);
                    user.LiczbaProb = 0;
                }
                return false;
            }

            user.LiczbaProb = 0;
            return true;
        }
    }
}