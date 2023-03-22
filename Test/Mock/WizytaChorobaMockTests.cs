using Application.Interfaces;
using Application.WeterynarzSpecjalizacje.Commands;
using Application.WizytaChoroby.Commands;
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
    public class WizytaChorobaMockTests
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
        public async Task AddWizytaChorobaShouldBeCorrectTest()
        {
            var before = mockContext.Object.WizytaChorobas.Count();
            var handler = new AddWizytaChorobaCommandHandler(mockContext.Object, hash);

            AddWizytaChorobaCommand command = new AddWizytaChorobaCommand()
            {
                ID_choroba = hash.Encode(1),
                ID_wizyta = hash.Encode(1)
            };

            var result = await handler.Handle(command, CancellationToken.None);
            mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Once());
            Assert.AreEqual(before + 1, mockContext.Object.WizytaChorobas.Count());
        }

        [Test]
        public async Task RemoveWizytaChorobaShouldBeCorrectTest()
        {
            var before = mockContext.Object.WizytaChorobas.Count();
            var handler = new RemoveWizytaChorobaCommandHandler(mockContext.Object, hash);

            RemoveWizytaChorobaCommand command = new RemoveWizytaChorobaCommand()
            {
                ID_choroba = hash.Encode(1),
                ID_wizyta = hash.Encode(1)
            };

            var result = await handler.Handle(command, CancellationToken.None);
            mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Once());
            Assert.AreEqual(before - 1, mockContext.Object.WizytaChorobas.Count());
        }

        [Test]
        public void RemoveWizytaChorobaShouldThrowAnExceptionTest()
        {
            var handler = new RemoveWizytaChorobaCommandHandler(mockContext.Object, hash);

            RemoveWizytaChorobaCommand command = new RemoveWizytaChorobaCommand()
            {
                ID_choroba = hash.Encode(1),
                ID_wizyta = hash.Encode(-1)
            };

            Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, CancellationToken.None));
        }
    }
}
