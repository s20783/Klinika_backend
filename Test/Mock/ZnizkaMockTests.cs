using Application.Interfaces;
using Application.Znizki.Commands;
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
    public class ZnizkaMockTests
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
    public async Task UpdateZnizkaShouldBeCorrectTest()
    {
        var handler = new UpdateZnizkaCommandHandler(mockContext.Object, hash);

        var command = new UpdateZnizkaCommand()
        {
            ID_znizka = hash.Encode(1),
            Nazwa = "newNazwa",
            Procent = 36.6M
        };

        await handler.Handle(command, CancellationToken.None);
        mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Once());
    }


        [Test]
        public void UpdateZnizkaShouldThrowAnExceptionTest()
        {
            var handler = new UpdateZnizkaCommandHandler(mockContext.Object, hash);

            var command = new UpdateZnizkaCommand()
            {
                ID_znizka = hash.Encode(-1),
                Nazwa = "newNazwa",
                Procent = 36.6M
            };

            Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, CancellationToken.None));
        }
    }
}