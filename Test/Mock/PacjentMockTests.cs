using Application.DTO.Responses;
using Application.Interfaces;
using Application.Pacjenci.Commands;
using HashidsNet;
using Infrastructure.Services;
using Infrastructure.Services.Caching;
using Microsoft.Extensions.Caching.Memory;
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
    public class PacjentMockTests
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
        public async Task CreatePacjentShouldBeCorrectTest()
        {
            var handler = new CreatePacjentCommandHandle(mockContext.Object, hash, new MemoryMockCache<GetPacjentListResponse>());

            CreatePacjentCommand command = new CreatePacjentCommand()
            {
                request = new Application.DTO.Request.PacjentCreateRequest
                {
                    IdOsoba = hash.Encode(1),
                    Nazwa = "Azor",
                    Gatunek = "Pies",
                    Rasa = "Bulldog",
                    Masc = "Szary",
                    Agresywne = false,
                    Ubezplodnienie = false,
                    Waga = 10,
                    DataUrodzenia = DateTime.Now.AddDays(-1),
                    Plec = "M"
                }
            };

            var result = await handler.Handle(command, CancellationToken.None);
            mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Once());
            Assert.AreEqual(2, mockContext.Object.Pacjents.Count());
        }


        [Test]
        public async Task UpdatePacjentShouldBeCorrectTest()
        {
            var handler = new UpdatePacjentCommandHandle(mockContext.Object, hash, new MemoryMockCache<GetPacjentListResponse>());

            UpdatePacjentCommand command = new UpdatePacjentCommand()
            {
                ID_pacjent = hash.Encode(1),
                request = new Application.DTO.Request.PacjentCreateRequest
                {
                    IdOsoba = hash.Encode(1),
                    Nazwa = "Azor",
                    Gatunek = "Pies",
                    Rasa = "Bulldog",
                    Masc = "Szary",
                    Agresywne = false,
                    Ubezplodnienie = false,
                    Waga = 10,
                    DataUrodzenia = DateTime.Now.AddDays(-1),
                    Plec = "M"
                }
            };

            var result = await handler.Handle(command, CancellationToken.None);
            mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Once());
            Assert.AreEqual(1, mockContext.Object.Pacjents.Count());
        }


        [Test]
        public void UpdatePacjentShouldThrowAnExceptionTest()
        {
            var handler = new UpdatePacjentCommandHandle(mockContext.Object, hash, new MemoryMockCache<GetPacjentListResponse>());

            UpdatePacjentCommand command = new UpdatePacjentCommand()
            {
                ID_pacjent = hash.Encode(-1),
                request = new Application.DTO.Request.PacjentCreateRequest
                {
                    IdOsoba = hash.Encode(1),
                    Nazwa = "Azor",
                    Gatunek = "Pies",
                    Rasa = "Bulldog",
                    Masc = "Szary",
                    Agresywne = false,
                    Ubezplodnienie = false,
                    Waga = 10,
                    DataUrodzenia = DateTime.Now.AddDays(-1),
                    Plec = "M"
                }
            };

            Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, CancellationToken.None));
        }


        [Test]
        public async Task DeletePacjentShouldBeCorrectTest()
        {
            var before = mockContext.Object.Pacjents.Count();
            var handler = new DeletePacjentCommandHandle(mockContext.Object, hash, new MemoryMockCache<GetPacjentListResponse>());

            DeletePacjentCommand command = new DeletePacjentCommand()
            {
                ID_Pacjent = hash.Encode(1)
            };

            var result = await handler.Handle(command, CancellationToken.None);
            mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Once());
            Assert.AreEqual(before-1, mockContext.Object.Pacjents.Count());
        }

        [Test]
        public void DeletePacjentShouldThrowAnExceptionTest()
        {
            var handler = new DeletePacjentCommandHandle(mockContext.Object, hash, new MemoryMockCache<GetPacjentListResponse>());

            DeletePacjentCommand command = new DeletePacjentCommand()
            {
                ID_Pacjent = hash.Encode(-1)
            };

            Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, CancellationToken.None));
        }
    }
}