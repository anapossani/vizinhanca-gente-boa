using System.ComponentModel.DataAnnotations;
namespace Vizinhanca.API.Models
{
    public enum StatusPedido

    {
        aberto,
        em_andamento,
        concluido,
        cancelado
    }

    public class PedidoAjuda
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string? Descricao { get; set; }
        public StatusPedido Status { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataConclusao { get; set; }        
        public int UsuarioId { get; set; }
        public int CategoriaId { get; set; }


        public Usuario Usuario { get; set; } = null!;
        public CategoriaAjuda Categoria { get; set; } = null!;
    }
}