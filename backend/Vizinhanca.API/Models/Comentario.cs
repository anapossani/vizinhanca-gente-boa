// Dentro de Vizinhanca.API/Models/Comentario.cs

namespace Vizinhanca.API.Models
{
    public class Comentario
    {
        public int Id { get; set; }
        public string Mensagem { get; set; } = null!;
        public DateTime DataCriacao { get; set; }

        // Chaves Estrangeiras
        public int PedidoId { get; set; }
        public int UsuarioId { get; set; }

        // Propriedades de Navegação
        public PedidoAjuda Pedido { get; set; } = null!;
        public Usuario Usuario { get; set; } = null!;
    }
}