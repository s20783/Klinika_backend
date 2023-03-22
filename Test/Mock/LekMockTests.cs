using Application.DTO.Requests;
using Application.Interfaces;
using Application.Leki.Commands;
using HashidsNet;
using Infrastructure.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Test.Mock
{
    public class LekMockTests
    {
        private Mock<IKlinikaContext> mockContext;
        public HashService hash;

        [SetUp]
        public void SetUp()
        {
            mockContext = MockKlinikaContext.GetMockDbContext();
            hash = new HashService(new Hashids("zscfhulp36", 7));
        }


        [Test]
        public async Task CreateLekShouldBeCorrectTest()
        {
            var before = mockContext.Object.Leks.Count();
            var handler = new CreateLekCommandHandler(mockContext.Object, hash);

            var command = new CreateLekCommand()
            {
                request = new LekRequest
                {
                    Nazwa = "aaa",
                    JednostkaMiary = "ml",
                    Producent = "..."
                }
            };

            await handler.Handle(command, CancellationToken.None);
            Assert.AreEqual(mockContext.Object.Leks.Count(), before + 1);
        }


        [Test]
        public async Task UpdateLekShouldBeCorrectTest()
        {
            var before = mockContext.Object.Leks.Count();
            var handler = new UpdateLekCommandHandler(mockContext.Object, hash);

            var command = new UpdateLekCommand()
            {
                ID_lek = hash.Encode(1),
                request = new LekRequest
                {
                    Nazwa = "newNazwa",
                    JednostkaMiary = "ml"
                }
            };

            await handler.Handle(command, CancellationToken.None);
            Assert.AreEqual(mockContext.Object.Leks.Where(x => x.IdLek == 1).First().Nazwa, "newNazwa");
        }


        [Test]
        public async Task DeleteLekShouldBeCorrectTest()
        {
            var before = mockContext.Object.Leks.Count();
            var handler = new DeleteLekCommandHandler(mockContext.Object, hash);

            var command = new DeleteLekCommand()
            {
                ID_lek = hash.Encode(1)
            };

            await handler.Handle(command, CancellationToken.None);
            Assert.AreEqual(mockContext.Object.Leks.Count(), before - 1);
        }
    }
}