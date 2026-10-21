
using GestorGastos.Application.Interfaces;
using GestorGastos.Domain.Entities;

namespace GestorGastos.Application.UseCases;

public class ObtenerPerfilUseCase
{
    private readonly IUsuarioRepository _usuarios;

    public ObtenerPerfilUseCase(
        IUsuarioRepository usuarios)
    {
        _usuarios = usuarios;
    }

    public async Task<Usuario?> EjecutarAsync(
        int usuarioId)
    {
        if (usuarioId <= 0)
        {
            throw new ArgumentException(
                "El usuario no es valido.");
        }

        return await _usuarios.ObtenerPerfilAsync(usuarioId);
    }
}
