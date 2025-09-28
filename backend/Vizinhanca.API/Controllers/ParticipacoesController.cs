using Microsoft.AspNetCore.Mvc;
using Vizinhanca.API.Models;
using Vizinhanca.API.Services;
using Microsoft.AspNetCore.Authorization;

namespace Vizinhanca.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class ParticipacoesController : BaseApiController
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

            if (participacao is null)
            {
                return NotFound();
            }

            return Ok(participacao);
        }

        [HttpPost]
        public async Task<ActionResult<Participacao>> PostParticipacao(ParticipacaoCreateDto participacaoDto)
        {
            var novaParticipacao = await _participacaoService.CreateParticipacaoAsync(participacaoDto);

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