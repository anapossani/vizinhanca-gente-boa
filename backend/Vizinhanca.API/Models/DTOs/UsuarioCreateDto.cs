
public class UsuarioCreateDto
{
    public string Nome { get; set; } = null!;
    public string? Telefone { get; set; }
    public string Email { get; set; } = null!;
    public string Senha { get; set; } = null!;
    public string Cidade { get; set; } = null!;
    public string Estado { get; set; } = null!;   
    public string Bairro { get; set; } = null!;
}