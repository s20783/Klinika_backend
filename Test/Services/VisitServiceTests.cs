using Domain.Enums;
using Domain.Models;
using Infrastructure.Services;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Services
{
    public class VisitServiceTests
    {
        private static object[] GetwizytaLists1()
        {
            IList<Wizytum> wizytaList = new List<Wizytum>(){
                new Wizytum {
                    Status = VisitStatus.Zaplanowana.ToString(),
                },
                new Wizytum {
                    Status = VisitStatus.Zaplanowana.ToString()
                },
                new Wizytum {
                    Status = VisitStatus.Zaplanowana.ToString()
                }
            };

            return new[] { wizytaList };
        }

        private static object[] GetwizytaLists2()
        {
            IList<Wizytum> wizytaList = new List<Wizytum>(){
                new Wizytum {
                    Status = VisitStatus.Zaplanowana.ToString()
                },
                new Wizytum {
                    Status = VisitStatus.Zaplanowana.ToString()
                },
                new Wizytum {
                    Status = VisitStatus.Zaplanowana.ToString()
                },
                new Wizytum {
                    Status = VisitStatus.Zaplanowana.ToString()
                },
                new Wizytum {
                    Status = VisitStatus.Zaplanowana.ToString()
                },
                new Wizytum {
                    Status = VisitStatus.Zaplanowana.ToString()
                },
                new Wizytum {
                    Status = VisitStatus.Zaplanowana.ToString()
                },
                new Wizytum {
                    Status = VisitStatus.Zaplanowana.ToString()
                },
                new Wizytum {
                    Status = VisitStatus.Zaplanowana.ToString()
                },
                new Wizytum {
                    Status = VisitStatus.Zaplanowana.ToString()
                },
                new Wizytum {
                    Status = VisitStatus.Zaplanowana.ToString()
                },
                new Wizytum {
                    Status = VisitStatus.Zaplanowana.ToString()
                }
            };

            return new[] { wizytaList };
        }

        private static object[] GetHarmonogramList1()
        {
            IList<Harmonogram> harmonograms = new List<Harmonogram>(){
                new Harmonogram {
                    IdWizyta = 1,
                    DataRozpoczecia = new DateTime(2022, 10, 18, 12, 00, 00),
                    DataZakonczenia = new DateTime(2022, 10, 18, 12, 30, 00)
                },
                new Harmonogram {
                    IdWizyta = 1,
                    DataRozpoczecia = new DateTime(2022, 10, 18, 13, 00, 00),
                    DataZakonczenia = new DateTime(2022, 10, 18, 13, 30, 00)
                }
            };

            return new[] { harmonograms };
        }

        private static object[] GetHarmonogramList2()
        {
            IList<Harmonogram> harmonograms = new List<Harmonogram>(){
                new Harmonogram {
                    IdWizyta = 1,
                    DataRozpoczecia = new DateTime(2022, 10, 18, 12, 00, 00),
                    DataZakonczenia = new DateTime(2022, 10, 18, 12, 30, 00)
                },
                new Harmonogram {
                    IdWizyta = 1,
                    DataRozpoczecia = new DateTime(2022, 10, 18, 11, 00, 00),
                    DataZakonczenia = new DateTime(2022, 10, 18, 11, 30, 00)
                },
                new Harmonogram {
                    IdWizyta = 1,
                    DataRozpoczecia = new DateTime(2022, 10, 18, 11, 30, 00),
                    DataZakonczenia = new DateTime(2022, 10, 18, 12, 00, 00)
                }
            };

            return new[] { harmonograms };
        }



        [Test]
        [TestCaseSource("GetwizytaLists1")]
        public void WizytaAbleToCreateTest(List<Wizytum> a)
        {
            var result = new VisitService().IsVisitAbleToCreate(a);
            Assert.IsTrue(result);
        }

        [Test]
        [TestCaseSource("GetwizytaLists2")]
        public void WizytaNotAbleToCreateTest(List<Wizytum> a)
        {
            var result = new VisitService().IsVisitAbleToCreate(a);
            Assert.IsFalse(result);
        }

        [Test]
        [TestCase(-5,true)]
        [TestCase(-3, false)]
        [TestCase(-4, true)]
        public void IsWizytaAbleToCancelTest(int hour, bool expectedResult)
        {
            var result = new VisitService().IsWizytaAbleToCancel(DateTime.Now.AddHours(hour));
            Assert.AreEqual(expectedResult, result);
        }




        [Test]
        [TestCaseSource("GetHarmonogramList1")]
        public void HarmonogramCorrectDatesTest(List<Harmonogram> a)
        {
            (DateTime result, DateTime result2) = new VisitService().GetVisitDates(a);
            Assert.AreEqual(new DateTime(2022, 10, 18, 12, 00, 00), result);
            Assert.AreEqual(new DateTime(2022, 10, 18, 13, 30, 00), result2);
        }

        [Test]
        [TestCaseSource("GetHarmonogramList2")]
        public void HarmonogramWrongDatesTest(List<Harmonogram> a)
        {
            (DateTime result, DateTime result2) = new VisitService().GetVisitDates(a);
            Assert.AreNotEqual(new DateTime(2022, 10, 18, 12, 00, 00), result);
            Assert.AreNotEqual(new DateTime(2022, 10, 18, 13, 30, 00), result2);
        }




        [Test]
        [TestCaseSource("GetHarmonogramList2")]
        public void WizytaRescheduleReturnsTrue(List<Harmonogram> a)
        {
            var result = new VisitService().IsVisitAbleToReschedule(a, new DateTime(2022, 10, 18, 11, 00, 00));
            Assert.IsTrue(result);
        }

        [Test]
        [TestCaseSource("GetHarmonogramList1")]
        public void WizytaRescheduleReturnsFalse(List<Harmonogram> a)
        {
            var result = new VisitService().IsVisitAbleToReschedule(a, new DateTime(2022, 10, 18, 12, 00, 00));
            Assert.IsFalse(result);
        }

        [Test]
        [TestCaseSource("GetHarmonogramList2")]
        public void WizytaRescheduleReturnsFalse2(List<Harmonogram> a)
        {
            var result = new VisitService().IsVisitAbleToReschedule(a, new DateTime(2022, 10, 18, 12, 00, 00));
            Assert.IsFalse(result);
        }
    }
}