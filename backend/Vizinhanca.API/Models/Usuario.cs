using System.ComponentModel.DataAnnotations;
namespace Vizinhanca.API.Models

{    
    public class Usuario
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string Nome { get; set; } = null!;
        [StringLength(150)]
        public string Email { get; set; } = null!;
        public string Senha { get; set; } = null!;
        [StringLength(20)]
        public string? Telefone { get; set; }
        [StringLength(100)]
        public string? Bairro { get; set; }
        [StringLength(100)]
        public string Cidade { get; set; } = null!;
        [StringLength(2)]
        public string Estado { get; set; } = null!;        
        public DateTime DataCriacao { get; set; }
        public DateTime? UltimoLogin { get; set; }
    }
}