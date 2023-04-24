using Application.Common.Exceptions;
using Application.DTO.Request;
using Application.DTO.Requests;
using Application.DTO.Responses;
using Application.Interfaces;
using Application.Konto.Commands;
using HashidsNet;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
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
    public class AccountMockTests
    {

        private Mock<IKlinikaContext> mockContext;
        public HashService hash;
        public IConfiguration configuration;

        [SetUp]
        public void SetUp()
        {
            mockContext = MockKlinikaContext.GetMockDbContext();
            hash = new HashService(new Hashids("zscfhulp36", 7));


            var inMemorySettings = new Dictionary<string, string> {
                {"SecretKey", "q4Ze7tyWVopasdfghjkPnr6uvpapajwEz3m18nqu6cA41qaz2wsx3edc4rfvplijygrdwa2137xd2OChybfthvFcdf"},
                {"PasswordIterations", "150000"}
            };

            configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }

        [Test]
        public async Task LoginShouldBeCorrectTest()
        {
            var handler = new LoginCommandHandle(mockContext.Object, new TokenService(configuration), new PasswordService(), configuration, hash, new LoginService());

            LoginCommand command = new LoginCommand()
            {
                request = new LoginRequest
                {
                    NazwaUzytkownika = "Adam1",
                    Haslo = "Adam1"
                }
            };
            
            var result = await handler.Handle(command, CancellationToken.None);
            mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Once());
            Assert.IsNotNull(result.Token);
        }

        [TestCase("Adam1", "...")]
        [TestCase("...", "Adam1")]
        public void LoginShouldThrowAnExceptionTest(string a, string b)
        {
            var handler = new LoginCommandHandle(mockContext.Object, new TokenService(configuration), new PasswordService(), configuration, hash, new LoginService());

            LoginCommand command = new LoginCommand()
            {
                request = new LoginRequest
                {
                    NazwaUzytkownika = a,
                    Haslo = b
                }
            };

            Assert.ThrowsAsync<UserNotAuthorizedException>(async () => await handler.Handle(command, CancellationToken.None));
        }


        [Test]
        public async Task ChangePasswordCorrectTest()
        {
            var handler = new ChangePasswordCommandHandle(mockContext.Object, new PasswordService(), configuration, hash, new LoginService());

            var command = new ChangePasswordCommand()
            {
                ID_osoba = hash.Encode(1),
                request = new ChangePasswordRequest
                {
                    CurrentHaslo = "Adam1",
                    NewHaslo = "Adam123",
                    NewHaslo2 = "Adam123"
                }
            };

            await handler.Handle(command, CancellationToken.None);
            mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Once());
        }


        [Test]
        public void ChangePasswordNotCorrectTest()
        {
            var handler = new ChangePasswordCommandHandle(mockContext.Object, new PasswordService(), configuration, hash, new LoginService());

            var command = new ChangePasswordCommand()
            {
                ID_osoba = hash.Encode(1),
                request = new ChangePasswordRequest
                {
                    CurrentHaslo = "...",
                    NewHaslo = "Adam123",
                    NewHaslo2 = "Adam123"
                }
            };

            Assert.ThrowsAsync<UserNotAuthorizedException>(async () => await handler.Handle(command, CancellationToken.None));
        }


        [Test]
        public async Task UpdateKontoCorrectTestAsync()
        {
            var handler = new UpdateKontoCommandHandle(mockContext.Object, new PasswordService(), configuration, hash, new LoginService(), new MemoryMockCache<GetClientListResponse>());

            var command = new UpdateAccountCommand()
            {
                ID_osoba = hash.Encode(1),
                request = new AccountUpdateRequest
                {
                    Email = "new@email.com",
                    NumerTelefonu = "123123123",
                    Haslo = "Adam1"
                }
            };

            await handler.Handle(command, CancellationToken.None);
            mockContext.Verify(m => m.SaveChangesAsync(CancellationToken.None), Times.Once());
        }


        [Test]
        public void UpdateKontoThrowsAnExceptionTest()
        {
            var handler = new UpdateKontoCommandHandle(mockContext.Object, new PasswordService(), configuration, hash, new LoginService(), new MemoryMockCache<GetClientListResponse>());

            var command = new UpdateAccountCommand()
            {
                ID_osoba = hash.Encode(999),
                request = new AccountUpdateRequest
                {
                    Email = "new@email.com",
                    NumerTelefonu = "123123123"
                }
            };
            
            Assert.ThrowsAsync<UserNotAuthorizedException>(async () => await handler.Handle(command, CancellationToken.None));
        }
    }
}