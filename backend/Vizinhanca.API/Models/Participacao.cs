// Dentro de Vizinhanca.API/Models/Participacao.cs

namespace Vizinhanca.API.Models
{
    public enum StatusParticipacao
    {
        interessado,
        aceito,
        recusado
    }

    public class Participacao
    {
        public int Id { get; set; }
        public StatusParticipacao Status { get; set; }
        public DateTime DataParticipacao { get; set; }

        public int PedidoId { get; set; }
        public int UsuarioId { get; set; }
        
        public PedidoAjuda Pedido { get; set; } = null!;
        public Usuario Usuario { get; set; } = null!;
    }
}