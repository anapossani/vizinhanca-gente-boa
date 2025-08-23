using Microsoft.EntityFrameworkCore;
using Vizinhanca.API.Models; 

namespace Vizinhanca.API.Data
{
    public class VizinhançaContext : DbContext
    {
        public VizinhançaContext(DbContextOptions<VizinhançaContext> options) : base(options)
        {
        }
        
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<CategoriaAjuda> CategoriasAjuda { get; set; }
        public DbSet<PedidoAjuda> PedidosAjuda { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<Participacao> Participacoes { get; set; }
       
    }
}