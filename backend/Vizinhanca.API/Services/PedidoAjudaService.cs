using Microsoft.EntityFrameworkCore;
using Vizinhanca.API.Data;
using Vizinhanca.API.Models;
using Vizinhanca.API.DTOs;
using Vizinhanca.API.Exceptions;

namespace Vizinhanca.API.Services
{
    public class PedidoAjudaService
    {
        private readonly VizinhancaContext _context;
        private readonly IdentityService _identityService;

        public PedidoAjudaService(VizinhancaContext context, IdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }
        public async Task<IEnumerable<PedidoAjudaDto>> GetPedidosAjudaAsync(
        int? usuarioId, 
        StatusPedido? status, 
        DateTime? dataInicial,
        DateTime? dataFinal,
        bool apenasDeOutrosUsuarios = false, 
        int? usuarioLogadoId = null) 
        {
        var query = _context.PedidosAjuda
            .Include(p => p.Usuario)
            .Include(p => p.Categoria)
            .AsQueryable(); 

        if (usuarioId.HasValue)
        {
            query = query.Where(p => p.UsuarioId == usuarioId.Value);
        }

        if (status.HasValue)
        {
            query = query.Where(p => p.Status == status.Value);
        }

        if (dataInicial.HasValue)
        {
            query = query.Where(p => p.DataCriacao >= dataInicial.Value);
        }

        if (dataFinal.HasValue)
        {
            query = query.Where(p => p.DataCriacao <= dataFinal.Value);
        }

        if (apenasDeOutrosUsuarios && usuarioLogadoId.HasValue)
        {
            query = query.Where(p => p.UsuarioId != usuarioLogadoId.Value);
        }

        var pedidos = await query
            .OrderByDescending(p => p.DataCriacao)
            .Select(p => new PedidoAjudaDto
            {
                Id = p.Id,
                Titulo = p.Titulo,
                UsuarioNome = p.Usuario.Nome,                
                CategoriaNome = p.Categoria.Nome,
            })
            .ToListAsync();
             return pedidos;
        }
        
        public async Task<PedidoAjudaDetalhesDto?> GetPedidoAjudaByIdAsync(int id)
        {
            var pedidoDto = await _context.PedidosAjuda
                .Include(p => p.Usuario)
                .Include(p => p.Categoria)
                .Include(p => p.Comentarios)
                    .ThenInclude(c => c.Usuario)
                
                .Where(p => p.Id == id) 
                .Select(p => new PedidoAjudaDetalhesDto 
                {
                    Id = p.Id,
                    Titulo = p.Titulo,
                    Descricao = p.Descricao,
                    Status = p.Status.ToString(),
                    DataCriacao = p.DataCriacao,
                    UsuarioId = p.UsuarioId,
                    UsuarioNome = p.Usuario.Nome,

                    CategoriaId = p.CategoriaId,
                    CategoriaNome = p.Categoria.Nome,
                    
                    Comentarios = p.Comentarios.Select(c => new ComentarioDto
                    {
                        Id = c.Id,
                        Mensagem = c.Mensagem,
                        DataCriacao = c.DataCriacao,
                        UsuarioId = c.UsuarioId,
                        UsuarioNome = c.Usuario.Nome 
                    }).ToList()
                })
                .FirstOrDefaultAsync(); 

            return pedidoDto;
        }        



        public async Task<PedidoAjuda> CreatePedidoAjudaAsync(PedidoAjudaCreateDto pedidoAjudaDto)
        {
            var usuarioLogadoId = _identityService.GetUserId();
            var novoPedidoAjuda = new PedidoAjuda
            {
                Titulo = pedidoAjudaDto.Titulo,
                Descricao = pedidoAjudaDto.Descricao,
                CategoriaId = pedidoAjudaDto.CategoriaId,
                DataCriacao = DateTime.UtcNow,
                Status = StatusPedido.aberto,
                UsuarioId = usuarioLogadoId
            };
            _context.PedidosAjuda.Add(novoPedidoAjuda);
            await _context.SaveChangesAsync();
            return novoPedidoAjuda;
        }

        public async Task<bool> UpdatePedidoAjudaAsync(int id, PedidoAjudaUpdateDto pedidoAjudaDto)
        {
            var pedidoExistente = await _context.PedidosAjuda.FindAsync(id);
            var usuarioLogadoId = _identityService.GetUserId();

            if (pedidoExistente is null)
            {
                return false;
            }
            if (usuarioLogadoId != pedidoExistente.UsuarioId)
            {
                throw new BusinessRuleException("Somente o criador do pedido pode realizar alterações.");
            }

            if (pedidoAjudaDto.Status == StatusPedido.concluido)
            {
                throw new BusinessRuleException("A conclusão de um pedido deve ser feita através do endpoint específico '/concluir'.");
            }
            if (pedidoExistente.Status == StatusPedido.concluido || pedidoExistente.Status == StatusPedido.cancelado ) 
            {
                throw new BusinessRuleException($"Não é possível alterar um pedido com status {pedidoExistente.Status} ");
            }

            pedidoExistente.Titulo = !string.IsNullOrWhiteSpace(pedidoAjudaDto.Titulo) ? pedidoAjudaDto.Titulo : pedidoExistente.Titulo;
            pedidoExistente.Descricao = pedidoAjudaDto.Descricao ?? pedidoExistente.Descricao; 
            pedidoExistente.CategoriaId = pedidoAjudaDto.CategoriaId > 0 ? pedidoAjudaDto.CategoriaId : pedidoExistente.CategoriaId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ConcluirPedidoAsync(int id)
        {
            var pedido = await _context.PedidosAjuda.FindAsync(id);
            var usuarioLogadoId = _identityService.GetUserId();
            
            if (pedido is null || pedido.Status != StatusPedido.em_andamento)
            {
                return false;
            }            

            if (usuarioLogadoId != pedido.UsuarioId)
            {
                throw new BusinessRuleException("A conclusão de um pedido deve ser pelo dono do pedido.'.");

            }

            pedido.Status = StatusPedido.concluido;
            pedido.DataConclusao = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }     

    }
}       