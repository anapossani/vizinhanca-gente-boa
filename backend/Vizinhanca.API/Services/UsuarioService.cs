using Microsoft.EntityFrameworkCore;
using Vizinhanca.API.Data;
using Vizinhanca.API.Models;
using Vizinhanca.API.Exceptions;

namespace Vizinhanca.API.Services
{
    public class UsuarioService
    {
        private readonly VizinhancaContext _context;

        public UsuarioService(VizinhancaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Usuario>> GetUsuariosAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task<Usuario?> GetUsuarioByIdAsync(int id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        public async Task<Usuario> CreateUsuarioAsync(UsuarioCreateDto usuarioDto)
        {
            var novoUsuario = new Usuario
            {
                Nome = usuarioDto.Nome,
                Telefone = usuarioDto.Telefone,
                Bairro = usuarioDto.Bairro,
                Email = usuarioDto.Email,
                Senha = BCrypt.Net.BCrypt.HashPassword(usuarioDto.Senha),
                Cidade = usuarioDto.Cidade,
                Estado = usuarioDto.Estado,
                DataCriacao = DateTime.UtcNow
            };
            _context.Usuarios.Add(novoUsuario);
            await _context.SaveChangesAsync();
            return novoUsuario;
        }


        public async Task<bool> UpdateUsuarioAsync(int id, UsuarioUpdateDto usuarioDto, int usuarioLogadoId)
        {
            var usuarioExistente = await _context.Usuarios.FindAsync(id);
            if (usuarioExistente is null)
            {
                return false;
            }
            if (id != usuarioLogadoId)
            {
                throw new BusinessRuleException("Ação não permitida: você está tentando alterar os dados de outro usuário. Por favor, faça login com o usuário correto para continuar.");
            }

            usuarioExistente.Nome = !string.IsNullOrWhiteSpace(usuarioDto.Nome) ? usuarioDto.Nome : usuarioExistente.Nome;
            usuarioExistente.Telefone = usuarioDto.Telefone ?? usuarioExistente.Telefone;
            usuarioExistente.Bairro = usuarioDto.Bairro ?? usuarioExistente.Bairro;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUsuarioAsync(int id, int usuarioLogadoId)
        {
            var usuarioParaDeletar = await _context.Usuarios.FindAsync(id);

            if (usuarioParaDeletar is null)
            {
                return false;
            }
            if (usuarioLogadoId != id)
            {
                throw new BusinessRuleException("Ação não permitida: você está remover outro usuário. Por favor, faça login com o usuário correto para continuar.");

            }

            _context.Usuarios.Remove(usuarioParaDeletar);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Usuario?> GetUsuarioByEmailAsync(string email)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        } 
    }      
}