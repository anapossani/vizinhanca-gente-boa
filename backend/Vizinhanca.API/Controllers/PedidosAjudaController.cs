using Microsoft.AspNetCore.Mvc;
using Vizinhanca.API.Exceptions;
using Vizinhanca.API.Models;
using Vizinhanca.API.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Vizinhanca.API.DTOs;

namespace Vizinhanca.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class PedidosAjudaController : BaseApiController
    {
        private readonly IdentityService _identityService;
        private readonly PedidoAjudaService _pedidoAjudaService;


        public PedidosAjudaController(PedidoAjudaService pedidoAjudaService, IdentityService identityService
            )
        {
            _pedidoAjudaService = pedidoAjudaService;
            _identityService = identityService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PedidoAjudaDto>>> GetPedidos(
            [FromQuery] int? usuarioId,
            [FromQuery] StatusPedido? status,
            [FromQuery] DateTime? dataInicial,
            [FromQuery] DateTime? dataFinal,
            [FromQuery] bool apenasDeOutrosUsuarios = false,
            [FromQuery] bool? apenasPedidosComParticipacao = null)

        {
            int? usuarioLogadoId = null;
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdString, out var userId))
               {
                  usuarioLogadoId = userId;
               }

            var pedidosAjuda = await _pedidoAjudaService.GetPedidosAjudaAsync(
                usuarioId,
                status,
                dataInicial,
                dataFinal,
                apenasDeOutrosUsuarios,
                usuarioLogadoId
            );
            return Ok(pedidosAjuda);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PedidoAjudaDetalhesDto>> GetPedidoAjuda(int id)
        {
            var pedidoAjuda = await _pedidoAjudaService.GetPedidoAjudaByIdAsync(id);

            if (pedidoAjuda == null)
            {
                return NotFound();
            }

            return Ok(pedidoAjuda);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutPedidoAjuda(int id, PedidoAjudaUpdateDto dto)
        {
            try
            {
                var sucesso = await _pedidoAjudaService.UpdatePedidoAjudaAsync(id, dto);
                if (!sucesso)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (BusinessRuleException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<PedidoAjuda>> PostPedidoAjuda(PedidoAjudaCreateDto pedidoAjudaDto)
        {
            var novoPedidoAjuda = await _pedidoAjudaService.CreatePedidoAjudaAsync(pedidoAjudaDto);

            return CreatedAtAction(nameof(GetPedidoAjuda), new { id = novoPedidoAjuda.Id }, novoPedidoAjuda);
        }

        [HttpPost("{id}/cancelar")]
        [Authorize]
        public async Task<IActionResult> CancelarPedido(int id)
        {
            try
            {
                var userId = _identityService.GetUserId(); 
                if (userId == 0) return Unauthorized();

                await _pedidoAjudaService.CancelarPedidoAsync(id, userId);

                return NoContent(); 
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); 
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);             }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); 
            }
        }

        [HttpPost("{id}/concluir")]
        [Authorize]
        public async Task<IActionResult> ConcluirPedido(int id, [FromBody] PedidoAjudaConclusaoDto dto)
        {
            try
            {
                var userId = _identityService.GetUserId();
                if (userId == 0) return Unauthorized();

                await _pedidoAjudaService.ConcluirPedidoAsync(id, userId, dto);

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}       