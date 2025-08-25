using Microsoft.AspNetCore.Mvc;
using Vizinhanca.API.Models;
using Vizinhanca.API.Services; 

namespace Vizinhanca.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasAjudaController : ControllerBase
    {
        private readonly CategoriaAjudaService _categoriaAjudaService;

        public CategoriasAjudaController(CategoriaAjudaService categoriaAjudaService)
        {
            _categoriaAjudaService = categoriaAjudaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaAjuda>>> GetCategoriasAjuda()
        {
            var categoriasAjuda = await _categoriaAjudaService.GetCategoriasAjudaAsync();
            return Ok(categoriasAjuda);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaAjuda>> GetCategoriaAjuda(int id)
        {
            var categoriaAjuda = await _categoriaAjudaService.GetCategoriaAjudaByIdAsync(id);

            if (categoriaAjuda == null)
            {
                return NotFound();
            }

            return Ok(categoriaAjuda);
        }

        [HttpPost]
        public async Task<ActionResult<CategoriaAjuda>> PostCategoriaAjuda(CategoriaAjudaCreateDto categoriaAjudaDto)
        {
                var novaCategoriaAjuda = await _categoriaAjudaService.CreateCategoriaAjudaAsync(categoriaAjudaDto);

                return CreatedAtAction(nameof(GetCategoriaAjuda), new { id = novaCategoriaAjuda.Id }, novaCategoriaAjuda);
        }

    }
}