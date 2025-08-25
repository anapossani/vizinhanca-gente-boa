using Microsoft.AspNetCore.Mvc;
using Vizinhanca.API.Models;
using Vizinhanca.API.Services;

namespace Vizinhanca.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParticipacoesController : ControllerBase
    {
        private readonly ParticipacaoService _participacaoService;

        public ParticipacoesController(ParticipacaoService participacaoService)
        {
            _participacaoService = participacaoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Participacao>>> GetParticipacoes(
        [FromQuery] StatusParticipacao? status)
        {
            var participacoes = await _participacaoService.GetParticipacoesAsync(status);
            return Ok(participacoes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Participacao>> GetParticipacao(int id)
        {
            var participacao = await _participacaoService.GetParticipacaoByIdAsync(id);

            if (participacao == null)
            {
                return NotFound();
            }

            return Ok(participacao);
        }
        
        [HttpPost]
        public async Task<ActionResult<Participacao>> PostComentario(ParticipacaoCreateDto participacaoDto)
        {
            var usuarioLogadoId = 1;
            var novaParticipacao = await _participacaoService.CreateParticipacaoAsync(participacaoDto, usuarioLogadoId);

                return CreatedAtAction(nameof(GetParticipacao), new { id = novaParticipacao.Id }, novaParticipacao);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutParticipacao(int id, ParticipacaoUpdateDto participacaoDto)
        {
            var sucesso = await _participacaoService.UpdateParticipacaoAsync(id, participacaoDto);

            if (!sucesso)
            {
                return NotFound();
            }
            return NoContent();
        }     
    }    
}