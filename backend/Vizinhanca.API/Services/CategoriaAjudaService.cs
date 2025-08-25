using Microsoft.EntityFrameworkCore;
using Vizinhanca.API.Data;
using Vizinhanca.API.Models;

namespace Vizinhanca.API.Services
{
    public class CategoriaAjudaService
    {
        private readonly VizinhancaContext _context;

        public CategoriaAjudaService(VizinhancaContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<CategoriaAjuda>> GetCategoriasAjudaAsync()
        {
           return await _context.CategoriasAjuda.ToListAsync();
        }

        public async Task<CategoriaAjuda?> GetCategoriaAjudaByIdAsync(int id)
        {
            return await _context.CategoriasAjuda.FindAsync(id);
        }
  
        public async Task<CategoriaAjuda> CreateCategoriaAjudaAsync(CategoriaAjudaCreateDto categoriaAjudaDto)
        {
            var novaCategoriaAjuda = new CategoriaAjuda
            {
                Nome = categoriaAjudaDto.Nome,
                Descricao = categoriaAjudaDto.Descricao
            };
            
            _context.CategoriasAjuda.Add(novaCategoriaAjuda);
            await _context.SaveChangesAsync();
            return novaCategoriaAjuda;
        }        
    }
}       