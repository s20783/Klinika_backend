using Application.DTO.Requests;
using Application.Interfaces;
using Application.LekiWMagazynie.Commands;
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
    public class MedicamentWarehouseMockTests
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
        public async Task CreateStanLekuShouldBeCorrectTest()
        {
            var before = mockContext.Object.LekWMagazynies.Count();
            var handler = new CreateStanLekuCommandHandler(mockContext.Object, hash);

            var command = new CreateMedicamentWarehouseCommand()
            {
                ID_lek = hash.Encode(1),
                request = new Application.DTO.Request.MedicamentWarehouseRequest
                {
                    Ilosc = 10,
                    DataWaznosci = DateTime.Now.AddDays(10)
                }
            };

            await handler.Handle(command, CancellationToken.None);
            Assert.AreEqual(mockContext.Object.LekWMagazynies.Count(), before + 1);
        }


        [Test]
        public async Task UpdateStanLekuShouldBeCorrectTest()
        {
            var before = mockContext.Object.LekWMagazynies.Count();
            var handler = new UpdateStanLekuCommandHandler(mockContext.Object, hash);

            var command = new UpdateMedicamentWarehouseCommand()
            {
                ID_stan_leku = hash.Encode(1),
                request = new Application.DTO.Request.MedicamentWarehouseRequest
                {
                    Ilosc = 10,
                    DataWaznosci = DateTime.Now.AddDays(10)
                }
            };

            await handler.Handle(command, CancellationToken.None);
            Assert.AreEqual(mockContext.Object.LekWMagazynies.Where(x => x.IdStanLeku == 1).First().Ilosc, 10);
        }


        [Test]
        public async Task DeleteStanLekuShouldBeCorrectTest()
        {
            var before = mockContext.Object.LekWMagazynies.Count();
            var handler = new DeleteStanLekuCommandHandler(mockContext.Object, hash);

            var command = new DeleteMedicamentWarehouseCommand()
            {
                ID_stan_leku = hash.Encode(1)
            };

            await handler.Handle(command, CancellationToken.None);
            Assert.AreEqual(mockContext.Object.LekWMagazynies.Count(), before - 1);
        }
    }
}