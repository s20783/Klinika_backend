using Application.Common.Exceptions;
using Domain;
using Domain.Models;
using Infrastructure.Services;
using NUnit.Framework;
using System;

namespace Test.Services
{
    public class LoginServiceTests
    {
        private int iterations = 150000;
        private string haslo = "Adam1";

        private static object[] GetOsoba1()
        {
            Osoba osoba1 = new Osoba
            {
                Haslo = "TP506vFmQn79Wumsfl012OL3XCvaDsnKGBsjZbRYrZdjnZOrtdaKpyK9VxDN5/+faDZwWUuT2xLbDv0gegrWAg==",
                Salt = "ayH570KmANkGyHqQroN6Nl30mclzaC6Rxfq4SedA+C4=",
                LiczbaProb = GlobalValues.LICZBA_PROB - 1,
                DataBlokady = DateTime.Now.AddHours(-1)
            };

            return new [] { osoba1 };
        }

        private static object[] GetOsoba2()
        {
            Osoba osoba1 = new Osoba
            {
                Haslo = "TP506vFmQn79Wumsfl012OL3XCvaDsnKGBsjZbRYrZdjnZOrtdaKpyfdgK9VxDNZwWUuT2xLbDv0gegr111",
                Salt = "j666AjTc1HgWsLDptN4w+V9oSP+zWFYpkAVCgFsXiM0="
            };

            return new[] { osoba1 };
        }

        private static object[] GetOsoba3()
        {
            Osoba osoba1 = new Osoba
            {
                Haslo = "TP506vFmQn79Wumsfl012OL3XCvaDsnKGBsjZbRYrZdjnZOrtdaKpyK9VxDN5/+faDZwWUuT2xLbDv0gegrWAg==",
                Salt = "ayH570KmANkGyHqQroN6Nl30mclzaC6Rxfq4SedA+C4=",
                DataBlokady = DateTime.Now.AddHours(1)
            };

            return new[] { osoba1 };
        }

        private static object[] GetOsoba4()
        {
            Osoba osoba1 = new Osoba
            {
                Haslo = "",
                Salt = ""
            };

            return new[] { osoba1 };
        }


        [Test]
        [TestCaseSource("GetOsoba1")]
        public void LoginCorrectTest(object o)
        {
            var result = new LoginService().CheckCredentails((Osoba)o, new PasswordService(), haslo, iterations);
            Assert.AreEqual(((Osoba)o).LiczbaProb, 0);
            Assert.IsTrue(result);
        }


        [Test]
        [TestCaseSource("GetOsoba2")]
        public void LoginNotCorrectTest(object o)
        {
            var result = new LoginService().CheckCredentails((Osoba)o, new PasswordService(), haslo, iterations);
            Assert.IsFalse(result);
        }


        [Test]
        [TestCaseSource("GetOsoba3")]
        public void LoginThrowsAnConstraintExceptionTest(object o)
        {
            Assert.Throws<ConstraintException>(() => new LoginService().CheckCredentails((Osoba)o, new PasswordService(), haslo, iterations));
        }

        [Test]
        [TestCaseSource("GetOsoba4")]
        public void LoginDeletedAccountShouldThrowAnExceptionTest(object o)
        {
            var result = new LoginService().CheckCredentails((Osoba)o, new PasswordService(), "", iterations);
            Assert.IsFalse(result);
        }


        [Test]
        [TestCase(null)]
        public void LoginThrowsAnExceptionTest(object o)
        {
            Assert.Throws<UserNotAuthorizedException>(() => new LoginService().CheckCredentails((Osoba)o, new PasswordService(), haslo, iterations));
        }
    }
}