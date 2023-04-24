using Application.Choroby.Commands;
using Application.DTO.Requests;
using Application.DTO.Responses;
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
    public class DiseaseMockTests
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
            var before = mockContext.Object.Chorobas.Count();
            var handler = new CreateChorobaCommandHandler(mockContext.Object, hash, new MemoryMockCache<GetDiseaseResponse>());

            var command = new CreateDiseaseCommand()
            {
                request = new DiseaseRequest
                {
                    Nazwa = "aaa"
                }
            };

            await handler.Handle(command, CancellationToken.None);
            Assert.AreEqual(mockContext.Object.Chorobas.Count(), before + 1);
        }


        [Test]
        public async Task UpdateChorobaShouldBeCorrectTest()
        {
            var handler = new UpdateChorobaCommandHandler(mockContext.Object, hash, new MemoryMockCache<GetDiseaseResponse>());

            var command = new UpdateDiseaseCommand()
            {
                ID_Choroba = hash.Encode(1),
                request = new DiseaseRequest
                {
                    Nazwa = "aaa"
                }
            };

            await handler.Handle(command, CancellationToken.None);
            mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Once());
        }


        [Test]
        public void UpdateChorobaShouldThrowAnExceptionTest()
        {
            var handler = new UpdateChorobaCommandHandler(mockContext.Object, hash, new MemoryMockCache<GetDiseaseResponse>());

            var command = new UpdateDiseaseCommand()
            {
                ID_Choroba = hash.Encode(-1),
                request = new DiseaseRequest
                {
                    Nazwa = "aaa"
                }
            };

            Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, CancellationToken.None));
        }


        [Test]
        public async Task DeleteChorobaShouldBeCorrectTest()
        {
            var before = mockContext.Object.Chorobas.Count();
            var handler = new DeleteChorobaCommandHandler(mockContext.Object, hash, new MemoryMockCache<GetDiseaseResponse>());

            var command = new DeleteDiseaseCommand()
            {
                ID_Choroba = hash.Encode(1)
            };

            await handler.Handle(command, CancellationToken.None);
            mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Once());
            Assert.AreEqual(before - 1, mockContext.Object.Chorobas.Count());
        }


        [Test]
        public void DeleteChorobaShouldThrowAnExceptionTest()
        {
            var handler = new DeleteChorobaCommandHandler(mockContext.Object, hash, new MemoryMockCache<GetDiseaseResponse>());

            var command = new DeleteDiseaseCommand()
            {
                ID_Choroba = hash.Encode(-1)
            };

            Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, CancellationToken.None));
        }
    }
}