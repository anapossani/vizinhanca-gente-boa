public class ComentarioDto
{
    public int Id { get; set; }
    public string Mensagem { get; set; } = null!;
    public DateTime DataCriacao { get; set; }

    public int UsuarioId { get; set; }
    public string UsuarioNome { get; set; } = null!;
}