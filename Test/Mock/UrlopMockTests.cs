using Application.DTO.Requests;
using Application.Interfaces;
using Application.Urlopy.Commands;
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
    public class UrlopMockTests
    {
        private Mock<IKlinikaContext> mockContext;
        public HashService hash;
        private HarmonogramService harmonogramService;

        [SetUp]
        public void SetUp()
        {
            mockContext = MockKlinikaContext.GetMockDbContext();
            hash = new HashService(new Hashids("zscfhulp36", 7));
            harmonogramService = new HarmonogramService(new MockEmailSender());
        }


        [Test]
        public async Task CreateUrlopShouldBeCorrectTest()
        {
            var before = mockContext.Object.Urlops.Count();
            var handler = new CreateUrlopCommandHandler(mockContext.Object, hash, harmonogramService);

            var command = new CreateUrlopCommand()
            {
                request = new UrlopRequest
                {
                    ID_weterynarz = hash.Encode(2),
                    Dzien = new DateTime(2022,11,09)
                }
            };

            await handler.Handle(command, CancellationToken.None);
            Assert.AreEqual(mockContext.Object.Urlops.Count(), before + 1);
        }


        /*[Test]
        public void CreateUrlopShouldThrowAnExceptionTest()
        {
            var handler = new CreateUrlopCommandHandler(mockContext.Object, hash, harmonogramService);

            var command = new CreateUrlopCommand()
            {
                request = new UrlopRequest
                {
                    ID_weterynarz = hash.Encode(2),
                    Dzien = new DateTime(2022, 11, 16)
                }
            };

            Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, CancellationToken.None));
        }*/


        [Test]
        public async Task UpdateUrlopShouldBeCorrectTest()
        {
            var handler = new UpdateUrlopCommandHandler(mockContext.Object, hash, harmonogramService);

            var command = new UpdateUrlopCommand()
            {
                ID_urlop = hash.Encode(1),
                request = new UrlopRequest
                {
                    ID_weterynarz = hash.Encode(2),
                    Dzien = new DateTime(2022, 11, 09)
                }
            };

            await handler.Handle(command, CancellationToken.None);
            mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Once());
        }


        [Test]
        public async Task DeleteUrlopShouldBeCorrectTest()
        {
            var before = mockContext.Object.Urlops.Count();
            var handler = new DeleteUrlopCommandHandler(mockContext.Object, hash, harmonogramService);

            var command = new DeleteUrlopCommand()
            {
                ID_urlop = hash.Encode(1)
            };

            await handler.Handle(command, CancellationToken.None);
            mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Once());
            Assert.AreEqual(before - 1, mockContext.Object.Urlops.Count());
        }


        [Test]
        public void DeleteUrlopShouldThrowAnExceptionTest()
        {
            var handler = new DeleteUrlopCommandHandler(mockContext.Object, hash, harmonogramService);

            var command = new DeleteUrlopCommand()
            {
                ID_urlop = hash.Encode(-1)
            };

            Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, CancellationToken.None));
        }
    }
}