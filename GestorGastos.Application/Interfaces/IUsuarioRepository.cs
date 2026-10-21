
using GestorGastos.Domain.Entities;

namespace GestorGastos.Application.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> ObtenerPorCorreoAsync(string correo);

    Task<Usuario?> ObtenerPerfilAsync(int usuarioId);

    Task<Usuario> RegistrarAsync(Usuario usuario);
}

