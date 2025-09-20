using Microsoft.EntityFrameworkCore;
using Vizinhanca.API.Data;
using Vizinhanca.API.Models;
using Vizinhanca.API.Exceptions;
using Microsoft.OpenApi.Any;

namespace Vizinhanca.API.Services
{
    public class ComentarioService
    {
        private readonly VizinhancaContext _context;

        public ComentarioService(VizinhancaContext context)
        {
            _context = context;
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
  
        public async Task<Comentario> CreateComentarioAsync(ComentarioCreateDto comentarioDto, int usuarioLogadoId)
        {
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
     

        public async Task<bool> UpdateComentarioAsync(int id, ComentarioUpdateDto comentarioDto, int usuarioLogadoId)
        {
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

        public async Task<bool> DeleteComentarioAsync(int id, int usuarioLogadoId)
        {
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