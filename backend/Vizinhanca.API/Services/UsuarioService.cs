using Microsoft.EntityFrameworkCore;
using Vizinhanca.API.Data;
using Vizinhanca.API.Models;

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

        public async Task<Usuario> CreateUsuarioAsync(UsuarioCreateDto usuarioDto )
        {
            var novoUsuario = new Usuario
            {
                Nome = usuarioDto.Nome,
                Telefone = usuarioDto.Telefone,
                Bairro = usuarioDto.Bairro,
                Email = usuarioDto.Email,
                Senha = usuarioDto.Senha,
                DataCriacao = DateTime.UtcNow
            };
            _context.Usuarios.Add(novoUsuario);
            await _context.SaveChangesAsync();
            return novoUsuario;
        }
        

        public async Task<bool> UpdateUsuarioAsync(int id, UsuarioUpdateDto usuarioDto)
        {
            var usuarioExistente = await _context.Usuarios.FindAsync(id);
            if (usuarioExistente == null)
            {
                return false;
            }

            usuarioExistente.Nome = !string.IsNullOrWhiteSpace(usuarioDto.Nome) ? usuarioDto.Nome : usuarioExistente.Nome;
            usuarioExistente.Telefone = usuarioDto.Telefone ?? usuarioExistente.Telefone;
            usuarioExistente.Bairro = usuarioDto.Bairro ?? usuarioExistente.Bairro;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUsuarioAsync(int id)
        {
            var usuarioParaDeletar = await _context.Usuarios.FindAsync(id);

            if (usuarioParaDeletar == null)
            {
                return false;
            }

            _context.Usuarios.Remove(usuarioParaDeletar);

            await _context.SaveChangesAsync();

            return true;
        }


    }
}