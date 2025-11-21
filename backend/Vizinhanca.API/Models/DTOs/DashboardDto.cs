namespace Vizinhanca.API.DTOs
{
    public class DashboardDto
    {
        public string NomeUsuario { get; set; } = string.Empty;
        public DashboardStatsDto Stats { get; set; } = new();

        public List<PedidoAjudaResumoDto> UltimosPedidos { get; set; } = new();
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
        public string Titulo { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; }
        public int ContagemComentarios { get; set; }
        public int ContagemParticipacoes { get; set; }
    }
}