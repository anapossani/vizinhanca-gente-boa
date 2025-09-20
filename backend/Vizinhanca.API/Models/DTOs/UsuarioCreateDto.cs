
public class UsuarioCreateDto
{
    public string Nome { get; set; } = null!;
    public string? Telefone { get; set; }
    public string? Bairro { get; set; }
    public string Email { get; set; } = null!;
    public string Senha { get; set; } = null!;
}