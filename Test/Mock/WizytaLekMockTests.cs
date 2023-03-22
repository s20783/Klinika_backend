using Application.Interfaces;
using Application.WizytaLeki.Commands;
using Application.WizytaUslugi.Commands;
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
    public class WizytaLekMockTests
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
        public async Task AddWizytaLekShouldBeCorrectTest()
        {
            var before = mockContext.Object.WizytaLeks.Count();
            var handler = new AddWizytaLekCommandHandler(mockContext.Object, hash);

            AddWizytaLekCommand command = new AddWizytaLekCommand()
            {
                ID_Lek = hash.Encode(2),
                ID_wizyta = hash.Encode(1),
                Ilosc = 2
            };

            var result = await handler.Handle(command, CancellationToken.None);
            mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Once());
            Assert.AreEqual(before + 1, mockContext.Object.WizytaLeks.Count());
        }

        [Test]
        public async Task RemoveWizytaLekShouldBeCorrectTest()
        {
            var before = mockContext.Object.WizytaLeks.Count();
            var handler = new RemoveWizytaLekCommandHandler(mockContext.Object, hash);

            RemoveWizytaLekCommand command = new RemoveWizytaLekCommand()
            {
                ID_lek = hash.Encode(1),
                ID_wizyta = hash.Encode(1)
            };

            var result = await handler.Handle(command, CancellationToken.None);
            mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Once());
            Assert.AreEqual(before - 1, mockContext.Object.WizytaLeks.Count());
        }

        [Test]
        public void RemoveWizytaLekShouldThrowAnExceptionTest()
        {
            var handler = new RemoveWizytaLekCommandHandler(mockContext.Object, hash);

            RemoveWizytaLekCommand command = new RemoveWizytaLekCommand()
            {
                ID_lek = hash.Encode(-1),
                ID_wizyta = hash.Encode(1)
            };

            Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, CancellationToken.None));
        }
    }
}