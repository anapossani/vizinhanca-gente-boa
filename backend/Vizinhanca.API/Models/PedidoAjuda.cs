using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vizinhanca.API.Models
{
    public enum StatusPedido
    {
        Aberto = 0,
        EmAndamento = 1,
        Concluido = 2,
        Cancelado = 3
    }

    [Table("pedido_ajuda")]
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

        public virtual Usuario Usuario { get; set; } = null!;
        public virtual CategoriaAjuda Categoria { get; set; } = null!;

        public virtual ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();

        public virtual ICollection<Participacao> Participacoes { get; set; } = new List<Participacao>();
    }
}