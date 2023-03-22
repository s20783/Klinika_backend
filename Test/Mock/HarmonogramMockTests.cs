using Application.Harmonogramy.Commands;
using Application.Interfaces;
using Domain.Enums;
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
    public class HarmonogramMockTests
    {
        private Mock<IKlinikaContext> mockContext;
        public HashService hash;
        private HarmonogramService harmonogramService;

        [SetUp]
        public void SetUp()
        {
            mockContext = MockKlinikaContext.GetMockDbContext();
            hash = new HashService(new Hashids("zscfhulp36", 7));
            harmonogramService = new HarmonogramService(new MockEmailSender());
        }


        [Test]
        public async Task CreateHarmonogramShouldBeCorrectTest()
        {
            var before = mockContext.Object.Harmonograms.Count();
            var handler = new CreateHarmonogramDefaultCommandHandler(mockContext.Object, hash, harmonogramService);

            var command = new CreateHarmonogramDefaultCommand()
            {
                Data = new DateTime(2022, 10, 26)
            };

            await handler.Handle(command, CancellationToken.None);
            Assert.AreEqual(mockContext.Object.Harmonograms.Count(), before + 6);
        }

        [Test]
        public void CreateHarmonogramShouldThrowAnExceptionTest()
        {
            var before = mockContext.Object.Harmonograms.Count();
            var handler = new CreateHarmonogramDefaultCommandHandler(mockContext.Object, hash, harmonogramService);

            var command = new CreateHarmonogramDefaultCommand()
            {
                Data = new DateTime(2022, 10, 25)
            };

            Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public void CreateHarmonogramShouldNotThrowAnExceptionTest2()
        {
            var before = mockContext.Object.Harmonograms.Count();
            var handler = new CreateHarmonogramDefaultCommandHandler(mockContext.Object, hash, harmonogramService);

            var command = new CreateHarmonogramDefaultCommand()
            {
                Data = new DateTime(2022, 10, 27)
            };

            Assert.DoesNotThrowAsync(async () => await handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public async Task DeleteHarmonogramShouldBeCorrectTest()
        {
            var before = mockContext.Object.Harmonograms.Count();
            var before2 = mockContext.Object.Wizyta.Where(x => x.Status == WizytaStatus.AnulowanaKlinika.ToString()).Count();
            var handler = new DeleteHarmonogramCommandHandler(mockContext.Object, hash, harmonogramService);

            var command = new DeleteHarmonogramCommand()
            {
                Data = new DateTime(2022, 10, 27)
            };

            await handler.Handle(command, CancellationToken.None);
            Assert.AreNotEqual(mockContext.Object.Harmonograms.Count(), before);
            Assert.AreEqual(mockContext.Object.Harmonograms.Count(), before - 3);
            Assert.AreEqual(mockContext.Object.Wizyta.Where(x => x.Status == WizytaStatus.AnulowanaKlinika.ToString()).Count(), before2 + 1);
        }
    }
}