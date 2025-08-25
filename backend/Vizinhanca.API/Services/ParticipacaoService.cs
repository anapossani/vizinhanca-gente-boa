using Microsoft.EntityFrameworkCore;
using Vizinhanca.API.Data;
using Vizinhanca.API.Models;

namespace Vizinhanca.API.Services
{
    public class ParticipacaoService
    {
        private readonly VizinhancaContext _context;

        public ParticipacaoService(VizinhancaContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Participacao>> GetParticipacoesAsync(StatusParticipacao? status)
        {
            var query = _context.Participacoes.AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(c => c.Status == status.Value);
            }

            return await query.ToListAsync();
        }


        public async Task<Participacao?> GetParticipacaoByIdAsync(int id)
        {
            return await _context.Participacoes.FindAsync(id);
        }

        public async Task<Participacao> CreateParticipacaoAsync(ParticipacaoCreateDto participacaoDto, int usuarioLogadoId)
        {
            var novaParticipacao = new Participacao
            {
                PedidoId = participacaoDto.PedidoId,
                UsuarioId = usuarioLogadoId,
                Status = StatusParticipacao.interessado,
                DataParticipacao = DateTime.UtcNow

            };

            _context.Participacoes.Add(novaParticipacao);
            await _context.SaveChangesAsync();
            return novaParticipacao;
        }

        public async Task<bool> UpdateParticipacaoAsync(int id, ParticipacaoUpdateDto participacaoDto)
        {
            var participacaoExistente = await _context.Participacoes.FindAsync(id);
            if (participacaoExistente == null)
            {
                return false;
            }

            participacaoExistente.Status = participacaoDto.Status;

            await _context.SaveChangesAsync();
            return true;
        }        

    }
}