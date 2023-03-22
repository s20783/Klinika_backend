using Infrastructure.Services;
using NUnit.Framework;
using System.Text.RegularExpressions;

namespace Test.Services
{
    public class PasswordServiceTests
    {
        [Test]
        public void GenerateSaltTest()
        {
            var s = new PasswordService().GenerateSalt();
            Assert.IsTrue(s.Length != 0);
        }

        [Test]
        [TestCase(8)]
        [TestCase(9)]
        [TestCase(10)]
        public void RandomPasswordTest(int l)
        {
            var s = new PasswordService().GetRandomPassword(l);
            Regex rg = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{" + l + ",}$");
            Assert.IsTrue(rg.Match(s).Success);
        }
    }
}