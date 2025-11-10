namespace Vizinhanca.API.DTOs
{
    public class DashboardDto
    {
        public string NomeUsuario { get; set; }
        public DashboardStatsDto Stats { get; set; }
        public List<PedidoAjudaResumoDto> UltimosPedidos { get; set; }
        public List<ComentarioResumoDto> UltimosComentarios { get; set; }
    }

    public class DashboardStatsDto
    {
        public int PedidosCriados { get; set; }
        public int AjudasOferecidas { get; set; }
        public int ConexoesFeitas { get; set; } 
    }

    public class PedidoAjudaResumoDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Status { get; set; }
        public DateTime DataCriacao { get; set; }
    }

    public class ComentarioResumoDto
    {
        public int Id { get; set; }
        public string Texto { get; set; }
        public string NomeUsuario { get; set; } 
        public int PedidoId { get; set; }
        public string TituloPedido { get; set; } 
        public DateTime DataCriacao { get; set; }
    }
}