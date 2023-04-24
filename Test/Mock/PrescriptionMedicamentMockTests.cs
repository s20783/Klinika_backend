using Application.Interfaces;
using Application.ReceptaLeki.Commands;
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
    public class PrescriptionMedicamentMockTests
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
        public async Task CreateReceptaLekShouldBeCorrectTest()
        {
            var before = mockContext.Object.ReceptaLeks.Count();
            var handler = new CreateReceptaLekCommandHandler(mockContext.Object, hash);

            var command = new AddPrescriptionMedicamentCommand()
            {
                ID_Lek = hash.Encode(1),
                ID_Recepta = hash.Encode(1),
                Ilosc = 2
            };

            await handler.Handle(command, CancellationToken.None);
            Assert.AreEqual(mockContext.Object.ReceptaLeks.Count(), before + 1);
        }


        [Test]
        public async Task DeleteReceptaLekShouldBeCorrectTest()
        {
            var before = mockContext.Object.ReceptaLeks.Count();
            var handler = new DeleteReceptaLekCommandHandler(mockContext.Object, hash);

            var command = new DeletePrescriptionMedicamentCommand()
            {
                ID_Recepta = hash.Encode(1),
                ID_Lek = hash.Encode(1)
            };

            await handler.Handle(command, CancellationToken.None);
            mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Once());
            Assert.AreEqual(before - 1, mockContext.Object.ReceptaLeks.Count());
        }


        [Test]
        public void DeleteReceptaLekShouldThrowAnExceptionTest()
        {
            var handler = new DeleteReceptaLekCommandHandler(mockContext.Object, hash);

            var command = new DeletePrescriptionMedicamentCommand()
            {
                ID_Recepta = hash.Encode(-1),
                ID_Lek = hash.Encode(1)
            };

            Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, CancellationToken.None));
        }
    }
}