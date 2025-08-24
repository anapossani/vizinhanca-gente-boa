using Microsoft.EntityFrameworkCore;
using Vizinhanca.API.Models;

namespace Vizinhanca.API.Data
{
    public class VizinhancaContext : DbContext
    {
        public VizinhancaContext(DbContextOptions<VizinhancaContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<CategoriaAjuda> CategoriasAjuda { get; set; }
        public DbSet<PedidoAjuda> PedidosAjuda { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<Participacao> Participacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>().ToTable("usuario");
            modelBuilder.Entity<CategoriaAjuda>().ToTable("categoria_ajuda");
            modelBuilder.Entity<PedidoAjuda>().ToTable("pedidoajuda");
            modelBuilder.Entity<Comentario>().ToTable("comentario");
            modelBuilder.Entity<Participacao>().ToTable("participacao");
        }
    }    
}