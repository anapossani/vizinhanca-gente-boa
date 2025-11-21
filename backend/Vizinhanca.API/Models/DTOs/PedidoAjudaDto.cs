using Vizinhanca.API.Models;
namespace Vizinhanca.API.DTOs;

public class PedidoAjudaDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = null!;
    public string? Descricao { get; set; }
    public string Status { get; set; } = null!; 
    public DateTime DataCriacao { get; set; }
    public int UsuarioId { get; set; }
    public string UsuarioNome { get; set; } = null!;
    public int CategoriaId { get; set; }
    public string CategoriaNome { get; set; } = null!;
    public ICollection<ParticipacaoDetalhesDto> Participacoes { get; set; }
    public int TotalComentarios { get; set; } 
    public int TotalParticipacoes { get; set; } 
}    


public class PedidoAjudaDetalhesDto 
{
    public int Id { get; set; }
    public string Titulo { get; set; } = null!;
    public string? Descricao { get; set; }
    public int Status { get; set; }  
    public DateTime DataCriacao { get; set; }
    public int UsuarioId { get; set; }
    public string UsuarioNome { get; set; } = null!;
    public int CategoriaId { get; set; }
    public string CategoriaNome { get; set; } = null!;
    public List<ComentarioDto> Comentarios { get; set; } = new();
    public DateTime DataConclusao { get; set; }

}

public class PedidoAjudaConclusaoDto
{
    public string? Comentario { get;set; }
}