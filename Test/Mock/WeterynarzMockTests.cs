using Application.DTO.Responses;
using Application.Interfaces;
using Application.Weterynarze.Commands;
using HashidsNet;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
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
    public class WeterynarzMockTests
    {
        private Mock<IKlinikaContext> mockContext;
        public HashService hash;
        public IConfiguration configuration;
        private HarmonogramService harmonogramService;

        [SetUp]
        public void SetUp()
        {
            mockContext = MockKlinikaContext.GetMockDbContext();
            hash = new HashService(new Hashids("zscfhulp36", 7));
            harmonogramService = new HarmonogramService(new MockEmailSender());
            var inMemorySettings = new Dictionary<string, string> {
                {"SecretKey", "q4Ze7tyWVopasdfghjkPnr6uvpapajwEz3m18nqu6cA41qaz2wsx3edc4rfvplijygrdwa2137xd2OChybfthvFcdf"},
                {"PasswordIterations", "150000"}
            };

            configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }


        [Test]
        public async Task UpdateWeterynarzShouldBeCorrectTest()
        {
            var handler = new UpdateWeterynarzCommandHandler(mockContext.Object, hash, new MemoryMockCache<GetWeterynarzListResponse>());

            UpdateWeterynarzCommand command = new UpdateWeterynarzCommand()
            {
                ID_osoba = hash.Encode(2),
                request = new Application.DTO.WeterynarzUpdateRequest
                {
                    Imie = "aaaa",
                    Nazwisko = "bbbb",
                    DataUrodzenia = DateTime.Now.AddYears(-20),
                    Pensja = 6000,
                    DataZatrudnienia = DateTime.Now
                }
            };

            var result = await handler.Handle(command, CancellationToken.None);
            mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Once());
            Assert.AreEqual(1, mockContext.Object.Weterynarzs.Count());
        }


        [Test]
        public void UpdateWeterynarzShouldThrowAnExceptionTest()
        {
            var handler = new UpdateWeterynarzCommandHandler(mockContext.Object, hash, new MemoryMockCache<GetWeterynarzListResponse>());

            UpdateWeterynarzCommand command = new UpdateWeterynarzCommand()
            {
                ID_osoba = hash.Encode(-1),
                request = new Application.DTO.WeterynarzUpdateRequest
                {
                    Imie = "aaaa",
                    Nazwisko = "bbbb",
                    DataUrodzenia = DateTime.Now.AddYears(-20),
                    Pensja = 6000,
                    DataZatrudnienia = DateTime.Now
                }
            };

            Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, CancellationToken.None));
        }


        [Test]
        public async Task DeleteWeterynarzShouldBeCorrectTest()
        {
            var handler = new DeleteWeterynarzCommandHandler(mockContext.Object, hash, new MemoryMockCache<GetWeterynarzListResponse>(), harmonogramService);

            DeleteWeterynarzCommand command = new DeleteWeterynarzCommand()
            {
                ID_osoba = hash.Encode(2)
            };

            var result = await handler.Handle(command, CancellationToken.None);
            mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Once());
            Assert.IsTrue(!mockContext.Object.GodzinyPracies.Where(x => x.IdOsoba == 1).Any());
            Assert.AreEqual(1, mockContext.Object.Weterynarzs.Count());
        }


        [Test]
        public void DeleteWeterynarzShouldThrowAnExceptionTest()
        {
            var handler = new DeleteWeterynarzCommandHandler(mockContext.Object, hash, new MemoryMockCache<GetWeterynarzListResponse>(), harmonogramService);

            DeleteWeterynarzCommand command = new DeleteWeterynarzCommand()
            {
                ID_osoba = hash.Encode(-1)
            };

            Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, CancellationToken.None));
            Assert.AreEqual(1, mockContext.Object.Weterynarzs.Count());
        }
    }
}