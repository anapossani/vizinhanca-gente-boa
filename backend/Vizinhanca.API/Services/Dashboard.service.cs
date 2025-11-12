using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Vizinhanca.API.Data;
using Vizinhanca.API.DTOs;

namespace Vizinhanca.API.Services
{
    public class DashboardService
    {
        private readonly VizinhancaContext _context;

        public DashboardService(VizinhancaContext context)
        {
            _context = context;
        }

        public async Task<DashboardDto?> GetDashboardDataAsync(int userId)
        {
            var usuario = await _context.Usuarios.FindAsync(userId);
            if (usuario == null) return null;

            var pedidosCriados = await _context.PedidosAjuda.CountAsync(p => p.UsuarioId == userId);
            var ajudasOferecidas = await _context.Participacoes.CountAsync(p => p.UsuarioId == userId);

            var ultimosPedidos = await _context.PedidosAjuda
                .Where(p => p.UsuarioId == userId)
                .OrderByDescending(p => p.DataCriacao)
                .Take(3)
                .Select(p => new PedidoAjudaResumoDto { /* ... */ })
                .ToListAsync();

            var ultimosComentarios = await _context.Comentarios
                .Where(c => c.Pedido.UsuarioId == userId)
                .OrderByDescending(c => c.DataCriacao)
                .Take(5)
                .Select(c => new ComentarioResumoDto { /* ... */ })
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
                UltimosPedidos = ultimosPedidos,
                UltimosComentarios = ultimosComentarios
            };

            return dashboardData;
        }
    }
}