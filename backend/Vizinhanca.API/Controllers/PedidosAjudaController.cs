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
        private readonly PedidoAjudaService _pedidoAjudaService;

        public PedidosAjudaController(PedidoAjudaService pedidoAjudaService)
        {
            _pedidoAjudaService = pedidoAjudaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PedidoAjudaDto>>> GetPedidos(
            [FromQuery] int? usuarioId,
            [FromQuery] StatusPedido? status,
            [FromQuery] DateTime? dataInicial,
            [FromQuery] DateTime? dataFinal,
            [FromQuery] bool apenasDeOutrosUsuarios = false) 

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

        [HttpPost("{id}/concluir")]
        public async Task<IActionResult> ConcluirPedido(int id)
        {
            var sucesso = await _pedidoAjudaService.ConcluirPedidoAsync(id);

            if (!sucesso)
            {
                return NotFound();
            }

            return NoContent();
        }

    }
}       