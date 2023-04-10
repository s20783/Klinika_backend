using Application.DTO.Request;
using Application.DTO.Responses;
using Application.Interfaces;
using Application.Specjalizacje.Commands;
using Application.Specjalizacje.Queries;
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
    public class SpecjalizacjaMockTests
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
        public async Task CreateSpecjalizacjaShouldBeCorrectTest()
        {
            var before = mockContext.Object.Specjalizacjas.Count();
            var handler = new CreateSpecjalizacjaCommandHandler(mockContext.Object, hash, new MemoryMockCache<GetSpecjalizacjaResponse>());

            var command = new CreateSpecjalizacjaCommand()
            {
                request = new SpecjalizacjaRequest
                {
                    Opis = "aaa",
                    Nazwa = "aaa"
                }
            };

            await handler.Handle(command, CancellationToken.None);
            Assert.AreEqual(mockContext.Object.Specjalizacjas.Count(), before+1);
        }


        [Test]
        public async Task UpdateSpecjalizacjaShouldBeCorrectTest()
        {
            var handler = new UpdateSpecjalizacjaCommandHandler(mockContext.Object, hash, new MemoryMockCache<GetSpecjalizacjaResponse>());

            var command = new UpdateSpecjalizacjaCommand()
            {
                ID_specjalizacja = hash.Encode(1),
                request = new SpecjalizacjaRequest
                {
                    Opis = "update",
                    Nazwa = "aaa"
                }
            };

            await handler.Handle(command, CancellationToken.None);
            mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Once());
        }


        [Test]
        public void UpdateSpecjalizacjaShouldThrowAnExceptionTest()
        {
            var handler = new UpdateSpecjalizacjaCommandHandler(mockContext.Object, hash, new MemoryMockCache<GetSpecjalizacjaResponse>());

            var command = new UpdateSpecjalizacjaCommand()
            {
                ID_specjalizacja = hash.Encode(-1),
                request = new SpecjalizacjaRequest
                {
                    Opis = "update",
                    Nazwa = "aaa"
                }
            };

            Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, CancellationToken.None));
        }


        [Test]
        public async Task DeleteSpecjalizacjaShouldBeCorrectTest()
        {
            var before = mockContext.Object.Specjalizacjas.Count();
            var handler = new DeleteSpecjalizacjaCommandHandler(mockContext.Object, hash, new MemoryMockCache<GetSpecjalizacjaResponse>());

            var command = new DeleteSpecjalizacjaCommand()
            {
                ID_specjalizacja = hash.Encode(1)
            };

            await handler.Handle(command, CancellationToken.None);
            mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Once());
            Assert.AreEqual(before - 1, mockContext.Object.Specjalizacjas.Count());
        }


        [Test]
        public void DeleteSpecjalizacjaShouldThrowAnExceptionTest()
        {
            var handler = new DeleteSpecjalizacjaCommandHandler(mockContext.Object, hash, new MemoryMockCache<GetSpecjalizacjaResponse>());

            var command = new DeleteSpecjalizacjaCommand()
            {
                ID_specjalizacja = hash.Encode(-1)
            };

            Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, CancellationToken.None));
        }
    }
}