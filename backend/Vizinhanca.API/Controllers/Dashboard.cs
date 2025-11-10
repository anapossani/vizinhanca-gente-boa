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

            var usuarioTask = _context.Usuarios.FindAsync(userId).AsTask(); 

            var pedidosCriadosTask = _context.PedidosAjuda
                .CountAsync(p => p.UsuarioId == userId);

            var ajudasOferecidasTask = _context.Participacoes
                .CountAsync(p => p.UsuarioId == userId);

            await Task.WhenAll(usuarioTask, pedidosCriadosTask, ajudasOferecidasTask);

            var usuario = await usuarioTask;

            var ultimosPedidos = await _context.PedidosAjuda
                .Where(p => p.UsuarioId == userId)
                .OrderByDescending(p => p.DataCriacao)
                .Take(3)
                .Select(p => new PedidoAjudaResumoDto
                {
                    Id = p.Id,
                    Titulo = p.Titulo,
                    Status = p.Status.ToString(), // <<-- CORREÇÃO 2
                    DataCriacao = p.DataCriacao
                })
                .ToListAsync();

            var ultimosComentarios = await _context.Comentarios
                .Include(c => c.Usuario)
                .Include(c => c.Pedido) 
                .Where(c => c.Pedido.UsuarioId == userId) 
                .OrderByDescending(c => c.DataCriacao)
                .Take(5)
                .Select(c => new ComentarioResumoDto
                {
                    Id = c.Id,
                    Texto = c.Mensagem,
                    NomeUsuario = c.Usuario.Nome,
                    PedidoId = c.PedidoId,
                    TituloPedido = c.Pedido.Titulo, 
                    DataCriacao = c.DataCriacao
                })
                .ToListAsync();

            var dashboardData = new DashboardDto
            {
                NomeUsuario = usuario?.Nome ?? "Usuário",
                Stats = new DashboardStatsDto
                {
                    PedidosCriados = await pedidosCriadosTask,
                    AjudasOferecidas = await ajudasOferecidasTask,
                    ConexoesFeitas = 0
                },
                UltimosPedidos = ultimosPedidos,
                UltimosComentarios = ultimosComentarios
            };

            return Ok(dashboardData);
        }
    }
}