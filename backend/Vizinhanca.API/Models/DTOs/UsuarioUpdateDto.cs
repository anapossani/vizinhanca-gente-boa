public class UsuarioUpdateDto
{
    public string Nome { get; set; } = null!;
    public string? Telefone { get; set; }
    public string Cidade { get; set; } = null!;
    public string Estado { get; set; } = null!;
    public string Bairro { get; set; } = null!;
}