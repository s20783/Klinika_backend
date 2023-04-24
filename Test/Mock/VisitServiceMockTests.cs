using Application.Interfaces;
using Application.WizytaUslugi.Commands;
using Domain.Enums;
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
    public class VisitServiceMockTests
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
        public async Task AcceptWizytaUslugaShouldBeCorrectTest()
        {
            var handler = new AcceptWizytaUslugaCommandHandler(mockContext.Object, hash);

            AcceptVisitServicesCommand command = new AcceptVisitServicesCommand()
            {
                ID_wizyta = hash.Encode(1)
            };

            var result = await handler.Handle(command, CancellationToken.None);
            mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Once());

            var wizyta = mockContext.Object.Wizyta.First(x => x.IdWizyta == 1);

            Assert.AreEqual(wizyta.Status, VisitStatus.Zrealizowana.ToString());
            Assert.IsTrue(wizyta.CzyZaakceptowanaCena);
            Assert.AreEqual(200, wizyta.Cena);
            Assert.AreEqual(180, wizyta.CenaZnizka);
        }

        [Test]
        public void AcceptWizytaUslugaShouldThrowAnExceptionTest()
        {
            var handler = new AcceptWizytaUslugaCommandHandler(mockContext.Object, hash);

            AcceptVisitServicesCommand command = new AcceptVisitServicesCommand()
            {
                ID_wizyta = hash.Encode(2)
            };

            Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public async Task AddWizytaUslugaShouldBeCorrectTest()
        {
            var before = mockContext.Object.WizytaUslugas.Count();
            var handler = new AddWizytaUslugaCommandHandler(mockContext.Object, hash, new VisitService());

            AddVisitServiceCommand command = new AddVisitServiceCommand()
            {
                ID_usluga = hash.Encode(2),
                ID_wizyta = hash.Encode(1)
            };

            var result = await handler.Handle(command, CancellationToken.None);
            mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Exactly(2));
            Assert.AreEqual(before + 1, mockContext.Object.WizytaUslugas.Count());
        }

        [Test]
        public async Task RemoveWizytaUslugaShouldBeCorrectTest()
        {
            var before = mockContext.Object.WizytaUslugas.Count();
            var handler = new RemoveWizytaUslugaCommandHandler(mockContext.Object, hash, new VisitService());

            RemoveVisitServiceCommand command = new RemoveVisitServiceCommand()
            {
                ID_usluga = hash.Encode(1),
                ID_wizyta = hash.Encode(1)
            };

            var result = await handler.Handle(command, CancellationToken.None);
            mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Exactly(2));
            Assert.AreEqual(before - 1, mockContext.Object.WizytaUslugas.Count());
        }

        [Test]
        public void RemoveWizytaUslugaShouldThrowAnExceptionTest()
        {
            var handler = new RemoveWizytaUslugaCommandHandler(mockContext.Object, hash, new VisitService());

            RemoveVisitServiceCommand command = new RemoveVisitServiceCommand()
            {
                ID_usluga = hash.Encode(-1),
                ID_wizyta = hash.Encode(1)
            };

            Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, CancellationToken.None));
        }
    }
}