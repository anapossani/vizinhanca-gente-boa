using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Vizinhanca.API.Data;
using Vizinhanca.API.DTOs;

namespace Vizinhanca.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly VizinhancaContext _context;

        public DashboardController(VizinhancaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<DashboardDto>> GetDashboardData()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out var userId))
            {
                return Unauthorized();
            }
            var usuario = await _context.Usuarios.FindAsync(userId);
            if (usuario == null) return NotFound("Usuário não encontrado.");

            var pedidosCriados = await _context.PedidosAjuda
                .CountAsync(p => p.UsuarioId == userId);

            var ajudasOferecidas = await _context.Participacoes
                .CountAsync(p => p.UsuarioId == userId);

            var ultimosPedidos = await _context.PedidosAjuda
                .Where(p => p.UsuarioId == userId)
                .OrderByDescending(p => p.DataCriacao)
                .Take(5)
                .Select(p => new PedidoAjudaResumoDto
                {
                    Id = p.Id,
                    Titulo = p.Titulo,
                    Status = p.Status.ToString(),
                    DataCriacao = p.DataCriacao,
                    ContagemComentarios = p.Comentarios.Count(),
                    ContagemParticipacoes = p.Participacoes.Count()
                })
                .ToListAsync();

            var dashboardData = new DashboardDto
            {
                NomeUsuario = usuario.Nome,
                Stats = new DashboardStatsDto
                {
                    PedidosCriados = pedidosCriados,
                    AjudasOferecidas = ajudasOferecidas,
                    ConexoesFeitas = 0
                },
                UltimosPedidos = ultimosPedidos
            };

            return Ok(dashboardData);
        }

    }
}