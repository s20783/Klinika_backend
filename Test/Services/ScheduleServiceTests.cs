using Application.Interfaces;
using Domain.Enums;
using Domain.Models;
using Infrastructure.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Services
{
    public class ScheduleServiceTests
    {
        private Mock<IKlinikaContext> mockContext;
        private IEmailSender emailSender;
        [SetUp]
        public void SetUp()
        {
            emailSender = new MockEmailSender();
            mockContext = MockKlinikaContext.GetMockDbContext();
        }


        private static object[] GetGodzinyPracy1()
        {
            GodzinyPracy godzinyPracy =
                new GodzinyPracy
                {
                    GodzinaRozpoczecia = new TimeSpan(9, 0, 0),
                    GodzinaZakonczenia = new TimeSpan(12, 0, 0)
                };

            return new[] { godzinyPracy };
        }

        private static object[] GetGodzinyPracy2()
        {
            GodzinyPracy godzinyPracy =
                new GodzinyPracy
                {
                    GodzinaRozpoczecia = new TimeSpan(9, 12, 0),
                    GodzinaZakonczenia = new TimeSpan(12, 0, 0)
                };

            return new[] { godzinyPracy };
        }

        [Test]
        [TestCaseSource("GetGodzinyPracy1")]
        public void HarmonogramCountShouldBeCorrectTest(GodzinyPracy a)
        {
            var result = new ScheduleService(emailSender).ScheduleCount(a);
            Assert.AreEqual(result, 6);
        }

        [Test]
        [TestCaseSource("GetGodzinyPracy2")]
        public void HarmonogramCountThrowsAnExceptionTest(GodzinyPracy a)
        {
            Assert.Throws<Exception>(() => new ScheduleService(emailSender).ScheduleCount(a));
        }

        [Test]
        public void HarmonogramCreateShouldBeCorrectTest()
        {
            var before = mockContext.Object.Harmonograms.Count();
            new ScheduleService(emailSender).CreateVetSchedules(mockContext.Object, new DateTime(2022,11,9), 2);
            Assert.AreEqual(before + 6, mockContext.Object.Harmonograms.Count());
        }

        [Test]
        public void HarmonogramCreateShouldBeNullTest()
        {
            var before = mockContext.Object.Harmonograms.Count();
            new ScheduleService(emailSender).CreateVetSchedules(mockContext.Object, new DateTime(2022,11,16), 2);
            Assert.AreEqual(before, mockContext.Object.Harmonograms.Count());
        }

        [Test]
        public async Task HarmonogramDeleteShouldBeCorrectTest()
        {
            await new ScheduleService(emailSender).DeleteSchedules(mockContext.Object.Harmonograms.ToList(), mockContext.Object);
            Assert.AreEqual(0, mockContext.Object.Harmonograms.Count());
            Assert.AreEqual(VisitStatus.AnulowanaKlinika.ToString(), mockContext.Object.Wizyta.Where(x => x.IdWizyta == 1).First().Status);
        }
    }
}