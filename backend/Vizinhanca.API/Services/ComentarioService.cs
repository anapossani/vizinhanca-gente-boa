using Microsoft.EntityFrameworkCore;
using Vizinhanca.API.Data;
using Vizinhanca.API.Models;

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
  
        public async Task<Comentario> CreateComentarioAsync(ComentarioCreateDto comentarioDto)
        {
            var novoComentario = new Comentario
            {
                Mensagem = comentarioDto.Mensagem,
                PedidoId = comentarioDto.PedidoId,
                UsuarioId = 1 
            };
            
            _context.Comentarios.Add(novoComentario);
            await _context.SaveChangesAsync();
            return novoComentario;
        }
     

        public async Task<bool> UpdateComentarioAsync(int id, ComentarioUpdateDto comentarioDto)
        {
            var comentarioExistente = await _context.Comentarios.FindAsync(id);
            if (comentarioExistente == null)
            {
                return false;
            }

            comentarioExistente.Mensagem = !string.IsNullOrWhiteSpace(comentarioDto.Mensagem) ? comentarioDto.Mensagem : comentarioExistente.Mensagem;            

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteComentarioAsync(int id)
        {
            var comentarioParaDeletar = await _context.Comentarios.FindAsync(id);

            if (comentarioParaDeletar == null)
            {
                return false;
            }

            _context.Comentarios.Remove(comentarioParaDeletar);

            await _context.SaveChangesAsync();

            return true;
        }


    }
}