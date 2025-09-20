using Microsoft.AspNetCore.Mvc;
using Vizinhanca.API.Exceptions;
using Vizinhanca.API.Models;
using Vizinhanca.API.Services;

namespace Vizinhanca.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosAjudaController : BaseApiController
    {
        private readonly PedidoAjudaService _pedidoAjudaService;

        public PedidosAjudaController(PedidoAjudaService pedidoAjudaService)
        {
            _pedidoAjudaService = pedidoAjudaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PedidoAjuda>>> GetPedidosAjuda(
            [FromQuery] int? usuarioId,
            [FromQuery] StatusPedido? status,
            [FromQuery] DateTime? dataInicial,
            [FromQuery] DateTime? dataFinal)
        {
            var pedidosAjuda = await _pedidoAjudaService.GetPedidosAjudaAsync(usuarioId, status, dataInicial, dataFinal);
            return Ok(pedidosAjuda);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PedidoAjuda>> GetPedidoAjuda(int id)
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
                var sucesso = await _pedidoAjudaService.UpdatePedidoAjudaAsync(id, dto, UsuarioLogadoId);
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
        
        [HttpPost("{id}/concluir")] 
        public async Task<IActionResult> ConcluirPedido(int id)
        {
            var sucesso = await _pedidoAjudaService.ConcluirPedidoAsync(id, UsuarioLogadoId);

            if (!sucesso)
            {
                return NotFound(); 
            }

            return NoContent(); 
        }        

    }
}       