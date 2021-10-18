using System;
using System.Threading.Tasks;
using BattleshipGame.BLL.DTOs;
using BattleshipGame.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BattleshipGame.API.Controllers
{

    [ApiController]
    [Route("api/[controller]/")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(UserForRegisterDTO user)
        {
            try
            {
                await _authService.Register(user);
                return Ok();
            } 
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserForAuthResponseDTO>> Login(UserForLoginDTO user)
        {
            try
            {
                return Ok(await _authService.Login(user));
            } 
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}