using Application.Common.Exceptions;
using Application.Interfaces;
using Application.WizytaUslugi.Commands;
using Application.Wizyty.Commands;
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
    public class WizytaMockTests
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
        public async Task UmowWizyteShouldBeCorrectTest()
        {
            var before = mockContext.Object.Wizyta.Count();
            var handler = new UmowWizyteKlientCommandHandler(mockContext.Object, hash, new WizytaService(), new MockEmailSender());

            CreateWizytaKlientCommand command = new CreateWizytaKlientCommand()
            {
                ID_klient = hash.Encode(1),
                ID_Harmonogram = hash.Encode(1),
                ID_pacjent = hash.Encode(1),
                Notatka = "..."
            };

            var result = await handler.Handle(command, CancellationToken.None);
            mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Exactly(2));

            Assert.AreEqual(before + 1, mockContext.Object.Wizyta.Count());
            Assert.IsTrue(mockContext.Object.Harmonograms.First(x => x.IdHarmonogram == 1).IdWizyta.HasValue);
        }

        [Test]
        public void UmowWizyteShouldThrowAnExceptionTest()
        {
            var handler = new UmowWizyteKlientCommandHandler(mockContext.Object, hash, new WizytaService(), new MockEmailSender());

            CreateWizytaKlientCommand command = new CreateWizytaKlientCommand()
            {
                ID_klient = hash.Encode(4),
                ID_Harmonogram = hash.Encode(1),
                ID_pacjent = hash.Encode(1),
                Notatka = "..."
            };

            Assert.ThrowsAsync<ConstraintException>(async () => await handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public async Task UpdateWizytaShouldBeCorrectTest()
        {
            var before = mockContext.Object.Wizyta.Count();
            var handler = new UpdateWizytaInfoCommandHandler(mockContext.Object, hash, new WizytaService());

            UpdateWizytaInfoCommand command = new UpdateWizytaInfoCommand()
            {
                ID_weterynarz = hash.Encode(2),
                ID_wizyta = hash.Encode(2),
                request = new Application.DTO.Requests.WizytaInfoUpdateRequest
                {
                    ID_Pacjent = hash.Encode(1),
                    Opis = "..."
                }
            };

            var result = await handler.Handle(command, CancellationToken.None);
            mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Once());

            Assert.AreEqual(mockContext.Object.Wizyta.First(x => x.IdWizyta == 2).Opis, "...");
        }

        [Test]
        public void UpdateWizytaShouldThrowAnExceptionTest()
        {
            var handler = new UpdateWizytaInfoCommandHandler(mockContext.Object, hash, new WizytaService());

            UpdateWizytaInfoCommand command = new UpdateWizytaInfoCommand()
            {
                ID_weterynarz = hash.Encode(3),
                ID_wizyta = hash.Encode(2),
                request = new Application.DTO.Requests.WizytaInfoUpdateRequest
                {
                    ID_Pacjent = hash.Encode(1),
                    Opis = "..."
                }
            };

            Assert.ThrowsAsync<UserNotAuthorizedException>(async () => await handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public async Task PrzelozWizyteShouldBeCorrectTest()
        {
            var handler = new PrzelozWizyteCommandHandler(mockContext.Object, hash, new WizytaService());

            UpdateWizytaDateCommand command = new UpdateWizytaDateCommand()
            {
                ID_klient = hash.Encode(1),
                ID_wizyta = hash.Encode(1),
                ID_pacjent = hash.Encode(1),
                ID_harmonogram = hash.Encode(1),
                Notatka = "przelozWizyte"
            };

            var result = await handler.Handle(command, CancellationToken.None);
            mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Once());
            Assert.AreEqual("przelozWizyte", mockContext.Object.Wizyta.First(x => x.IdWizyta == 1).NotatkaKlient);
        }

        [Test]
        public void PrzelozWizyteShouldThrowAnExceptionTest()
        {
            var handler = new PrzelozWizyteCommandHandler(mockContext.Object, hash, new WizytaService());

            UpdateWizytaDateCommand command = new UpdateWizytaDateCommand()
            {
                ID_klient = hash.Encode(1),
                ID_wizyta = hash.Encode(2),
                ID_pacjent = hash.Encode(1),
                ID_harmonogram = hash.Encode(1),
                Notatka = "przelozWizyte"
            };

            Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public async Task CreateWizytaShouldBeCorrectTest()
        {
            var before = mockContext.Object.Wizyta.Count();
            var handler = new CreateWizytaCommandHandler(mockContext.Object, hash, new WizytaService());

            CreateWizytaCommand command = new CreateWizytaCommand()
            {
                ID_pacjent = hash.Encode(1),
                ID_harmonogram = hash.Encode(1),
                ID_klient = hash.Encode(1)
            };

            var result = await handler.Handle(command, CancellationToken.None);
            mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Exactly(2));
            Assert.AreEqual(before + 1, mockContext.Object.Wizyta.Count());
        }

        [Test]
        public void CreateWizytaShouldThrowAnExceptionTest()
        {
            var handler = new CreateWizytaCommandHandler(mockContext.Object, hash, new WizytaService());

            CreateWizytaCommand command = new CreateWizytaCommand()
            {
                ID_pacjent = hash.Encode(1),
                ID_harmonogram = hash.Encode(-1)
            };

            Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public async Task DeleteWizytaAdminShouldBeCorrectTest()
        {
            var handler = new DeleteWizytaAdminCommandHandler(mockContext.Object, hash, new WizytaService(), new MockEmailSender());

            DeleteWizytaAdminCommand command = new DeleteWizytaAdminCommand()
            {
                ID_wizyta = hash.Encode(1)
            };

            var result = await handler.Handle(command, CancellationToken.None);
            mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Once());
            Assert.AreEqual(WizytaStatus.AnulowanaKlinika.ToString(), mockContext.Object.Wizyta.First(x => x.IdWizyta == 1).Status);
        }

        [Test]
        public void DeleteWizytaAdminShouldThrowAnExceptionTest()
        {
            var handler = new DeleteWizytaAdminCommandHandler(mockContext.Object, hash, new WizytaService(), new MockEmailSender());

            DeleteWizytaAdminCommand command = new DeleteWizytaAdminCommand()
            {
                ID_wizyta = hash.Encode(2)
            };

            Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public async Task DeleteWizytaKlientShouldBeCorrectTest()
        {
            var handler = new DeleteWizytaKlientCommandHandler(mockContext.Object, hash, new WizytaService(), new MockEmailSender());

            DeleteWizytaKlientCommand command = new DeleteWizytaKlientCommand()
            {
                ID_wizyta = hash.Encode(1),
                ID_klient = hash.Encode(1)
            };

            var result = await handler.Handle(command, CancellationToken.None);
            mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Once());
            Assert.AreEqual(WizytaStatus.AnulowanaKlient.ToString(), mockContext.Object.Wizyta.First(x => x.IdWizyta == 1).Status);
        }

        [Test]
        public void DeleteWizytaKlientShouldThrowAnExceptionTest()
        {
            var handler = new DeleteWizytaKlientCommandHandler(mockContext.Object, hash, new WizytaService(), new MockEmailSender());

            DeleteWizytaKlientCommand command = new DeleteWizytaKlientCommand()
            {
                ID_wizyta = hash.Encode(2),
                ID_klient = hash.Encode(1)
            };

            Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public void DeleteWizytaKlientShouldThrowAnExceptionTest2()
        {
            var handler = new DeleteWizytaKlientCommandHandler(mockContext.Object, hash, new WizytaService(), new MockEmailSender());

            DeleteWizytaKlientCommand command = new DeleteWizytaKlientCommand()
            {
                ID_wizyta = hash.Encode(1),
                ID_klient = hash.Encode(2)
            };

            Assert.ThrowsAsync<UserNotAuthorizedException>(async () => await handler.Handle(command, CancellationToken.None));
        }
    }
}