using Microsoft.AspNetCore.Mvc;
using Vizinhanca.API.Models; 
using Vizinhanca.API.Services;

namespace Vizinhanca.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;
        private readonly TokenService _tokenService;

        public AuthController(UsuarioService usuarioService, TokenService tokenService)
        {
            _usuarioService = usuarioService;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var usuario = await _usuarioService.GetUsuarioByEmailAsync(loginDto.Email);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(loginDto.Senha, usuario.Senha))
            {
                return Unauthorized("Email ou senha inválidos.");
            }

            var token = _tokenService.GenerateToken(usuario);

            return Ok(new { token = token });
        }
    }
}