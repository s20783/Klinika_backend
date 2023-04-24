using Application.Common.Exceptions;
using Application.DTO.Request;
using Application.DTO.Requests;
using Application.Konto.Commands;
using Application.Konto.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PRO_API.Controllers
{
    public class AccountController : ApiControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetKonto(CancellationToken token)
        {
            return Ok(await Mediator.Send(new AccountQuery
            {
                ID_osoba = GetUserId()
            }, token));
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new LoginCommand
                {
                    request = request
                }, token));
            }
            catch (Exception e)
            {
                switch (e)
                {
                    case UserNotAuthorizedException:
                        return Unauthorized(new
                        {
                            message = e.Message
                        });
                    default:
                        return NotFound(new
                        {
                            message = e.Message
                        });
                }
            }
        }


        [HttpPost("refresh")]
        public async Task<IActionResult> GetToken(RefreshTokenRequest request, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new RefreshCommand
                {
                    request = request
                }, token));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateKontoCredentials(AccountUpdateRequest request, CancellationToken token)
        {
            try
            {
                return Ok(await Mediator.Send(new UpdateAccountCommand
                {
                    ID_osoba = GetUserId(),
                    request = request
                }, token));
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    message = e.Message
                });
            }
        }


        [Authorize]
        [HttpPut("password")]
        public async Task<IActionResult> ChangeKontoPassword(ChangePasswordRequest request)
        {
            try
            {
                return Ok(await Mediator.Send(new ChangePasswordCommand
                {
                    ID_osoba = GetUserId(),
                    request = request
                }));
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    message = e.Message
                });
            }
        }
    }
}