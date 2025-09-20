using Vizinhanca.API.Models;

public class PedidoAjudaCreateDto
{
    public string Titulo { get; set; } = null!;
    public string ?Descricao { get; set; } 
    public StatusPedido Status { get; set; }
    public int CategoriaId { get; set; } 
}