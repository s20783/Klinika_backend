using HashidsNet;
using Infrastructure.Services;
using NUnit.Framework;

namespace Test.Services
{
    [TestFixture]
    public class HashServiceTests
    {
        public Hashids hashids;

        [SetUp]
        public void asgg()
        {
            hashids = new Hashids("zscfhulp36", 7);
        }

        [Test]
        [TestCase("mNJplJM", 2)]
        public void DecodeTest(string id, int expectedResult)
        {
            int result = new HashService(hashids).Decode(id);
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        [TestCase("mNJplJM", "2wJm1JB", 2, 5)]
        public void DecodeTwoValuesTest(string id1, string id2, int expectedResult1, int expectedResult2)
        {
            (int result1, int result2) = new HashService(hashids).Decode(id1, id2);
            Assert.AreEqual(expectedResult1, result1);
            Assert.AreEqual(expectedResult2, result2);
        }

        [Test]
        [TestCase(5, "2wJm1JB")]
        public void EncodeTest(int id, string expectedResult)
        {
            string result = new HashService(hashids).Encode(id);
            Assert.AreEqual(expectedResult, result);
        }
    }
}