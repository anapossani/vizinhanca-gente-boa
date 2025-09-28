using Microsoft.AspNetCore.Mvc;
using Vizinhanca.API.Models;
using Vizinhanca.API.Services; 
using Microsoft.AspNetCore.Authorization;
namespace Vizinhanca.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    
    public class UsuariosController : BaseApiController
    {
        private readonly UsuarioService _usuarioService;

        public UsuariosController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            var usuarios = await _usuarioService.GetUsuariosAsync();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _usuarioService.GetUsuarioByIdAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(UsuarioCreateDto usuarioDto)
        {
            var novoUsuario = await _usuarioService.CreateUsuarioAsync(usuarioDto);

            return CreatedAtAction(nameof(GetUsuario), new { id = novoUsuario.Id }, novoUsuario);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, UsuarioUpdateDto usuarioDto, int usuarioLogadoId)
        {
            var sucesso = await _usuarioService.UpdateUsuarioAsync(id, usuarioDto, usuarioLogadoId);

            if (!sucesso) // lembrar de repicar o repique
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id, int usuarioLogadoId)
        {
            var sucesso = await _usuarioService.DeleteUsuarioAsync(id, usuarioLogadoId);

            if (sucesso)
            {
                return NoContent();    
            }
            return NotFound();
            
        }
    }
}