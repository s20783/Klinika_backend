using Application.DTO.Requests;
using Application.DTO.Responses;
using Application.Interfaces;
using Application.Uslugi.Commands;
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
    public class UslugaMockTests
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
        public async Task CreateUslugaShouldBeCorrectTest()
        {
            var before = mockContext.Object.Uslugas.Count();
            var handler = new CreateUslugaCommandHandler(mockContext.Object, hash, new MemoryMockCache<GetUslugaResponse>());

            var command = new CreateUslugaCommand()
            {
                request = new UslugaRequest
                {
                    NazwaUslugi = "aaa",
                    Narkoza = true,
                    Cena = 150,
                    Dolegliwosc = "lekki ból",
                    Opis = "..."
                }
            };

            await handler.Handle(command, CancellationToken.None);
            Assert.AreEqual(mockContext.Object.Uslugas.Count(), before + 1);
        }


        [Test]
        public async Task UpdateUslugaShouldBeCorrectTest()
        {
            var handler = new UpdateUslugaCommandHandler(mockContext.Object, hash, new MemoryMockCache<GetUslugaResponse>());

            var command = new UpdateUslugaCommand()
            {
                ID_usluga = hash.Encode(1),
                request = new UslugaRequest
                {
                    NazwaUslugi = "aaa",
                    Narkoza = true,
                    Opis = "...",
                    Cena = 100
                }
            };

            await handler.Handle(command, CancellationToken.None);
            mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Once());
        }


        [Test]
        public void UpdateUslugaShouldThrowAnExceptionTest()
        {
            var handler = new UpdateUslugaCommandHandler(mockContext.Object, hash, new MemoryMockCache<GetUslugaResponse>());

            var command = new UpdateUslugaCommand()
            {
                ID_usluga = hash.Encode(-1),
                request = new UslugaRequest
                {
                    NazwaUslugi = "aaa",
                    Narkoza = true,
                    Opis = "...",
                    Cena = 100
                }
            };

            Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, CancellationToken.None));
        }


        [Test]
        public async Task DeleteUslugaShouldBeCorrectTest()
        {
            var before = mockContext.Object.Uslugas.Count();
            var handler = new DeleteUslugaCommandHandler(mockContext.Object, hash, new MemoryMockCache<GetUslugaResponse>());

            var command = new DeleteUslugaCommand()
            {
                ID_usluga = hash.Encode(1)
            };

            await handler.Handle(command, CancellationToken.None);
            mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Once());
            Assert.AreEqual(before - 1, mockContext.Object.Uslugas.Count());
        }


        [Test]
        public void DeleteUslugaShouldThrowAnExceptionTest()
        {
            var handler = new DeleteUslugaCommandHandler(mockContext.Object, hash, new MemoryMockCache<GetUslugaResponse>());

            var command = new DeleteUslugaCommand()
            {
                ID_usluga = hash.Encode(-1)
            };

            Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, CancellationToken.None));
        }
    }
}