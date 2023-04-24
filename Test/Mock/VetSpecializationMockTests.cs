using Application.Interfaces;
using Application.WeterynarzSpecjalizacje.Commands;
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
    public class VetSpecializationMockTests
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
        public async Task AddWeterynarzSpecjalizacjaShouldBeCorrectTest()
        {
            var before = mockContext.Object.WeterynarzSpecjalizacjas.Count();
            var handler = new AddSpecjalizacjaWeterynarzCommandHandler(mockContext.Object, hash);

            AddVetSpecializationCommand command = new AddVetSpecializationCommand()
            {
                ID_weterynarz = hash.Encode(2),
                ID_specjalizacja = hash.Encode(2)
            };

            var result = await handler.Handle(command, CancellationToken.None);
            mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Once());
            Assert.AreEqual(before + 1, mockContext.Object.WeterynarzSpecjalizacjas.Count());
        }

        [Test]
        public async Task RemoveWeterynarzSpecjalizacjaShouldBeCorrectTest()
        {
            var before = mockContext.Object.WeterynarzSpecjalizacjas.Count();
            var handler = new RemoveSpecjalizacjaWeterynarzCommandHandler(mockContext.Object, hash);

            RemoveVetSpecializationCommand command = new RemoveVetSpecializationCommand()
            {
                ID_weterynarz = hash.Encode(2),
                ID_specjalizacja = hash.Encode(1)
            };

            var result = await handler.Handle(command, CancellationToken.None);
            mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Once());
            Assert.AreEqual(before - 1, mockContext.Object.WeterynarzSpecjalizacjas.Count());
        }


        [Test]
        public void RemoveWeterynarzSpecjalizacjaShouldThrowAnExceptionTest()
        {
            var before = mockContext.Object.WeterynarzSpecjalizacjas.Count();
            var handler = new RemoveSpecjalizacjaWeterynarzCommandHandler(mockContext.Object, hash);

            RemoveVetSpecializationCommand command = new RemoveVetSpecializationCommand()
            {
                ID_weterynarz = hash.Encode(2),
                ID_specjalizacja = hash.Encode(-1)
            };

            Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, CancellationToken.None));
        }
    }
}