using Microsoft.EntityFrameworkCore;
using Vizinhanca.API.Data;
using Vizinhanca.API.Models;
using Vizinhanca.API.Exceptions;

namespace Vizinhanca.API.Services
{
    public class ParticipacaoService
    {
        private readonly VizinhancaContext _context;
        private readonly IdentityService _identityService;

        public ParticipacaoService(VizinhancaContext context, IdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
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

        public async Task<Participacao> CreateParticipacaoAsync(ParticipacaoCreateDto participacaoDto)
        {
            var usuarioLogadoId = _identityService.GetUserId();
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
            var usuarioLogadoId = _identityService.GetUserId();
            var participacaoExistente = await _context.Participacoes.FindAsync(id);
            if (participacaoExistente is null)
            {
                return false;
            }
            if (usuarioLogadoId != participacaoExistente.UsuarioId)
            {
                throw new BusinessRuleException("Somente o criador pode realizar alterações.");                                
            }

            participacaoExistente.Status = participacaoDto.Status;

            await _context.SaveChangesAsync();
            return true;
        }        

    }
}