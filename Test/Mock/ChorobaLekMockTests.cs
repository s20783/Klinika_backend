using Application.ChorobaLeki.Commands;
using Application.Interfaces;
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
    public class ChorobaLekMockTests
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
        public async Task CreateChorobaShouldBeCorrectTest()
        {
            var before = mockContext.Object.ChorobaLeks.Count();
            var handler = new AddChorobaLekCommandHandler(mockContext.Object, hash);

            var command = new AddChorobaLekCommand()
            {
                ID_choroba = hash.Encode(2),
                ID_lek = hash.Encode(2)
            };

            await handler.Handle(command, CancellationToken.None);
            Assert.AreEqual(mockContext.Object.ChorobaLeks.Count(), before + 1);
        }


        [Test]
        public async Task DeleteChorobaShouldBeCorrectTest()
        {
            var before = mockContext.Object.ChorobaLeks.Count();
            var handler = new RemoveChorobaLekCommandHandler(mockContext.Object, hash);

            var command = new RemoveChorobaLekCommand()
            {
                ID_choroba = hash.Encode(1),
                ID_lek = hash.Encode(1)
            };

            await handler.Handle(command, CancellationToken.None);
            mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Once());
            Assert.AreEqual(before - 1, mockContext.Object.ChorobaLeks.Count());
        }


        [Test]
        public void DeleteChorobaShouldThrowAnExceptionTest()
        {
            var handler = new RemoveChorobaLekCommandHandler(mockContext.Object, hash);

            var command = new RemoveChorobaLekCommand()
            {
                ID_choroba = hash.Encode(3),
                ID_lek = hash.Encode(-1)
            };

            Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, CancellationToken.None));
        }
    }
}