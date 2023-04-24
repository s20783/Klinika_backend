using Application.Interfaces;
using Application.Recepty.Commands;
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
    public class PrescriptionMockTests
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
        public async Task CreateReceptaShouldBeCorrectTest()
        {
            var before = mockContext.Object.Recepta.Count();
            var handler = new CreateReceptaCommandHandler(mockContext.Object, hash);

            var command = new CreatePrescriptionCommand()
            {
                ID_wizyta = hash.Encode(1),
                Zalecenia = "..."
            };

            await handler.Handle(command, CancellationToken.None);
            Assert.AreEqual(mockContext.Object.Recepta.Count(), before + 1);
        }


        [Test]
        public async Task UpdateReceptaShouldBeCorrectTest()
        {
            var before = mockContext.Object.Recepta.Count();
            var handler = new UpdateReceptaCommandHandler(mockContext.Object, hash);

            var command = new UpdatePrescriptionCommand()
            {
                ID_recepta = hash.Encode(1),
                Zalecenia = "abc"
            };

            await handler.Handle(command, CancellationToken.None);
            mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Once());
            Assert.AreEqual(mockContext.Object.Recepta.Where(x => x.IdWizyta == 1).First().Zalecenia, "abc");
        }


        [Test]
        public void UpdateReceptaShouldThrowAnExceptionTest()
        {
            var before = mockContext.Object.Recepta.Count();
            var handler = new UpdateReceptaCommandHandler(mockContext.Object, hash);

            var command = new UpdatePrescriptionCommand()
            {
                ID_recepta = hash.Encode(-1),
                Zalecenia = "abc"
            };

            Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, CancellationToken.None));
        }


        [Test]
        public async Task DeleteReceptaShouldBeCorrectTest()
        {
            var before = mockContext.Object.Recepta.Count();
            var handler = new DeleteReceptaCommandHandler(mockContext.Object, hash);

            var command = new DeletePrescriptionCommand()
            {
                ID_recepta = hash.Encode(1)
            };

            await handler.Handle(command, CancellationToken.None);
            mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Once());
            Assert.AreEqual(before - 1, mockContext.Object.Recepta.Count());
        }


        [Test]
        public void DeleteReceptaShouldThrowAnExceptionTest()
        {
            var handler = new DeleteReceptaCommandHandler(mockContext.Object, hash);

            var command = new DeletePrescriptionCommand()
            {
                ID_recepta = hash.Encode(-1)
            };

            Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, CancellationToken.None));
        }
    }
}