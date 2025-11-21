public class ParticipacaoCreateDto
{
    public int PedidoId { get; set; } 
}

public class ParticipacaoDetalhesDto
{
    public int Id { get; set; }
    public string UsuarioNome { get; set; } = null!;
    public DateTime DataParticipacao { get; set; }
    public string Status { get; set; } = null!;
    
}