using System.Threading.Tasks;
using dotnet_rpg.Data;
using dotnet_rpg.Dtos.User;
using dotnet_rpg.Models;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_rpg.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;

        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegisterDto user)
        {
            ServiceResponse<int> serviceResponse = await _authRepository
            .Register(new User
            {
                Username = user.Username
            },
            user.Password);
            if (!serviceResponse.Success)
            {
                return BadRequest(serviceResponse);
            }
            return Ok(serviceResponse);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDto user)
        {
            ServiceResponse<string> serviceResponse = await _authRepository.Login(user.Username, user.Password);
            if (!serviceResponse.Success)
            {
                return BadRequest(serviceResponse);
            }
            return Ok(serviceResponse);

        }
    }
}