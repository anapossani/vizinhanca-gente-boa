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



        public async Task<bool> AceitarParticipacaoAsync(int participacaoId, int usuarioLogadoId)
        {
            var participacaoAlvo = await _context.Participacoes
                                                .Include(p => p.Pedido)
                                                .FirstOrDefaultAsync(p => p.Id == participacaoId);

            if (participacaoAlvo == null)
            {
                return false; 
            }

            if (participacaoAlvo.Pedido.UsuarioId != usuarioLogadoId)
            {
                throw new BusinessRuleException("Apenas o dono do pedido pode aceitar participantes.");
            }

            if (participacaoAlvo.Pedido.Status != StatusPedido.Aberto)
            {
                throw new BusinessRuleException("Este pedido não está mais aberto para aceitar participantes.");
            }

            participacaoAlvo.Status = StatusParticipacao.aceito;
            participacaoAlvo.Pedido.Status = StatusPedido.EmAndamento;

           var outrasParticipacoes = await _context.Participacoes
                .Where(p => p.PedidoId == participacaoAlvo.PedidoId && p.Id != participacaoId)
                .ToListAsync();

            foreach (var outra in outrasParticipacoes)
            {
                outra.Status = StatusParticipacao.recusado;
            }

            await _context.SaveChangesAsync();

            return true;
        }

    }
}