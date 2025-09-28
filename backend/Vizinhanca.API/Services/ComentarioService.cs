using Microsoft.EntityFrameworkCore;
using Vizinhanca.API.Data;
using Vizinhanca.API.Models;
using Vizinhanca.API.Exceptions;

namespace Vizinhanca.API.Services
{
    public class ComentarioService
    {
        private readonly VizinhancaContext _context;
        private readonly IdentityService _identityService;

        public ComentarioService(VizinhancaContext context, IdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<IEnumerable<Comentario>> GetComentariosAsync(int? pedidoId, int? usuarioId)
        {
            var query = _context.Comentarios.AsQueryable();

            if (pedidoId.HasValue)
            {
                query = query.Where(c => c.PedidoId == pedidoId.Value);
            }

            if (usuarioId.HasValue)
            {
                query = query.Where(c => c.UsuarioId == usuarioId.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<Comentario?> GetComentarioByIdAsync(int id)
        {
            return await _context.Comentarios.FindAsync(id);
        }
  
        public async Task<Comentario> CreateComentarioAsync(ComentarioCreateDto comentarioDto)
        {
            var usuarioLogadoId = _identityService.GetUserId();
            var novoComentario = new Comentario
            {
                Mensagem = comentarioDto.Mensagem,
                PedidoId = comentarioDto.PedidoId,
                UsuarioId = usuarioLogadoId
            };
            
            _context.Comentarios.Add(novoComentario);
            await _context.SaveChangesAsync();
            return novoComentario;
        }
     

        public async Task<bool> UpdateComentarioAsync(int id, ComentarioUpdateDto comentarioDto)
        {
            var usuarioLogadoId = _identityService.GetUserId();
            var comentarioExistente = await _context.Comentarios.FindAsync(id);
            if (comentarioExistente is null)
            {
                return false;
            }
            if (usuarioLogadoId != comentarioExistente.UsuarioId)
            {
                throw new BusinessRuleException("Somente o criador do comentário pode realizar alterações.");                
            }

            comentarioExistente.Mensagem = !string.IsNullOrWhiteSpace(comentarioDto.Mensagem) ? comentarioDto.Mensagem : comentarioExistente.Mensagem;            

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteComentarioAsync(int id)
        {
            var usuarioLogadoId = _identityService.GetUserId();
            var comentarioParaDeletar = await _context.Comentarios.FindAsync(id);

            if (comentarioParaDeletar is null)
            {
                return false;
            }
            if (usuarioLogadoId != comentarioParaDeletar.UsuarioId)
            {
                throw new BusinessRuleException("Somente o criador do comentário pode realizar alterações.");                                
            }

            _context.Comentarios.Remove(comentarioParaDeletar);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}