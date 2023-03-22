using Application.DTO.Requests;
using Application.Interfaces;
using Application.Szczepienia.Commands;
using HashidsNet;
using Infrastructure.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Test.Mock
{
    public class SzczepienieMockTests
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
        public async Task CreateSzczepienieShouldBeCorrectTest()
        {
            var before = mockContext.Object.Szczepienies.Count();
            var handler = new CreateSzczepienieCommandHandler(mockContext.Object, hash);

            var command = new CreateSzczepienieCommand()
            {
                request = new SzczepienieRequest
                {
                    IdLek = hash.Encode(2),
                    IdPacjent = hash.Encode(1),
                    Data = DateTime.Now,
                    Dawka = 10
                }
            };

            await handler.Handle(command, CancellationToken.None);
            Assert.AreEqual(mockContext.Object.Szczepienies.Count(), before + 1);
        }


        [Test]
        public async Task UpdateSzczepienieShouldBeCorrectTest()
        {
            var handler = new UpdateSzczepienieCommandHandler(mockContext.Object, hash);

            var command = new UpdateSzczepienieCommand()
            {
                ID_szczepienie = hash.Encode(1),
                request = new SzczepienieRequest
                {
                    IdLek = hash.Encode(2),
                    IdPacjent = hash.Encode(1),
                    Data = DateTime.Now,
                    Dawka = 10
                }
            };

            await handler.Handle(command, CancellationToken.None);
            mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Once());
        }


        [Test]
        public void UpdateSzczepienieShouldThrowAnExceptionTest()
        {
            var handler = new UpdateSzczepienieCommandHandler(mockContext.Object, hash);

            var command = new UpdateSzczepienieCommand()
            {
                ID_szczepienie = hash.Encode(-1),
                request = new SzczepienieRequest
                {
                    IdLek = hash.Encode(1),
                    IdPacjent = hash.Encode(1),
                    Data = DateTime.Now,
                    Dawka = 10
                }
            };

            Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, CancellationToken.None));
        }


        [Test]
        public async Task DeleteSzczepionkaShouldBeCorrectTest()
        {
            var before = mockContext.Object.Szczepienies.Count();
            var handler = new DeleteSzczepienieCommandHandler(mockContext.Object, hash);

            var command = new DeleteSzczepienieCommand()
            {
                ID_szczepienie = hash.Encode(1)
            };

            await handler.Handle(command, CancellationToken.None);
            mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Once());
            Assert.AreEqual(before - 1, mockContext.Object.Szczepienies.Count());
        }


        [Test]
        public void DeleteSzczepionkaShouldThrowAnExceptionTest()
        {
            var handler = new DeleteSzczepienieCommandHandler(mockContext.Object, hash);

            var command = new DeleteSzczepienieCommand()
            {
                ID_szczepienie = hash.Encode(-1)
            };

            Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, CancellationToken.None));
        }
    }
}