using Microsoft.AspNetCore.Mvc;
using Vizinhanca.API.Models;
using Vizinhanca.API.Services;

namespace Vizinhanca.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComentariosController : BaseApiController
    {
        private readonly ComentarioService _comentarioService;

        public ComentariosController(ComentarioService comentarioService)
        {
            _comentarioService = comentarioService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comentario>>> GetComentarios(
        [FromQuery] int? pedidoId,
        [FromQuery] int? usuarioId)
        {
            var comentarios = await _comentarioService.GetComentariosAsync(pedidoId, usuarioId);
            return Ok(comentarios);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Comentario>> GetComentario(int id)
        {
            var comentario = await _comentarioService.GetComentarioByIdAsync(id);

            if (comentario is null)
            {
                return NotFound();
            }

            return Ok(comentario);
        }
        
        [HttpPost]
        public async Task<ActionResult<Comentario>> PostComentario(ComentarioCreateDto comentarioDto)
        {
                var novoComentario = await _comentarioService.CreateComentarioAsync(comentarioDto, UsuarioLogadoId);

                return CreatedAtAction(nameof(GetComentario), new { id = novoComentario.Id }, novoComentario);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutComentario(int id, ComentarioUpdateDto comentarioDto)
        {
            var sucesso = await _comentarioService.UpdateComentarioAsync(id, comentarioDto, UsuarioLogadoId);           

            if (!sucesso)
            {
                return NotFound();
            }
            return NoContent();
        }   

         [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComentario(int id)
        {
            var sucesso = await _comentarioService.DeleteComentarioAsync(id, UsuarioLogadoId);

            if (!sucesso)
            {
                return NotFound();
            }
            return NoContent();
        }      
    }    
}