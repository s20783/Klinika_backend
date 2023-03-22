using Application.DTO.Requests;
using Application.GodzinaPracy.Commands;
using Application.Interfaces;
using Domain;
using Domain.Enums;
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
    public class GodzinyPracyMockTests
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
        public async Task CreateDefaultGodzinyPracyShouldBeCorrectTest()
        {
            var before = mockContext.Object.GodzinyPracies.Count();
            var handler = new CreateDefaultGodzinyPracyCommandHandle(mockContext.Object, hash);

            var command = new CreateDefaultGodzinyPracyCommand()
            {
                ID_osoba = hash.Encode(2)
            };

            await handler.Handle(command, CancellationToken.None);
            Assert.AreEqual(mockContext.Object.GodzinyPracies.Count(), before + GlobalValues.DNI_PRACY);
        }


        [Test]
        public async Task CreateGodzinyPracyShouldBeCorrectTest()
        {
            var before = mockContext.Object.GodzinyPracies.Count();
            var handler = new CreateGodzinyPracyCommandHandle(mockContext.Object, hash);

            var command = new CreateGodzinyPracyCommand()
            {
                ID_osoba = hash.Encode(2),
                requestList = new List<GodzinyPracyRequest>{
                    new GodzinyPracyRequest
                    {
                        DzienTygodnia = 7,
                        GodzinaRozpoczecia = new TimeSpan(9, 0, 0),
                        GodzinaZakonczenia = new TimeSpan(17, 0, 0)
                    }
                }
            };
            
            await handler.Handle(command, CancellationToken.None);
            Assert.AreEqual(mockContext.Object.GodzinyPracies.Count(), before + 1);
        }


        [Test]
        public async Task UpdateGodzinyPracyShouldBeCorrectTest()
        {
            var handler = new UpdateGodzinyPracyCommandHandle(mockContext.Object, hash);

            var command = new UpdateGodzinyPracyCommand()
            {
                ID_osoba = hash.Encode(2),
                requestList = new List<GodzinyPracyRequest>{
                    new GodzinyPracyRequest
                    {
                        DzienTygodnia = 1,
                        GodzinaRozpoczecia = new TimeSpan(9, 0, 0),
                        GodzinaZakonczenia = new TimeSpan(17, 0, 0)
                    }
                }
            };

            await handler.Handle(command, CancellationToken.None);
        }


        [Test]
        public void UpdateGodzinyPracyShouldNotChangeTest()
        {
            var handler = new UpdateGodzinyPracyCommandHandle(mockContext.Object, hash);

            var command = new UpdateGodzinyPracyCommand()
            {
                ID_osoba = hash.Encode(2),
                requestList = new List<GodzinyPracyRequest>{
                    new GodzinyPracyRequest
                    {
                        DzienTygodnia = 6,
                        GodzinaRozpoczecia = new TimeSpan(9, 0, 0),
                        GodzinaZakonczenia = new TimeSpan(17, 0, 0)
                    }
                }
            };

            Assert.That(!mockContext.Object.GodzinyPracies.Where(x => x.DzienTygodnia == 6 && x.IdOsoba == 2).Any());
        }


        [Test]
        public async Task DeleteGodzinyPracyShouldBeCorrectTest()
        {
            var before = mockContext.Object.GodzinyPracies.Count();
            var handler = new DeleteGodzinyPracyCommandHandle(mockContext.Object, hash);

            var command = new DeleteGodzinyPracyCommand()
            {
                ID_osoba = hash.Encode(2),
                dzien = 1
            };
            
            await handler.Handle(command, CancellationToken.None);
            Assert.AreEqual(mockContext.Object.GodzinyPracies.Count(), before - 1);
        }


        [Test]
        public void DeleteGodzinyPracyShouldBeIncorrectTest()
        {
            var before = mockContext.Object.GodzinyPracies.Count();
            var handler = new DeleteGodzinyPracyCommandHandle(mockContext.Object, hash);

            var command = new DeleteGodzinyPracyCommand()
            {
                ID_osoba = hash.Encode(2),
                dzien = 6
            };

            Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, CancellationToken.None));
        }
    }
}