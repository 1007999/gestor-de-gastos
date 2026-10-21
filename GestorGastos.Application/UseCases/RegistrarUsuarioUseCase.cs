
using GestorGastos.Application.Interfaces;
using GestorGastos.Domain.Entities;

namespace GestorGastos.Application.UseCases;

public class RegistrarUsuarioUseCase
{
    private readonly IUsuarioRepository _usuarios;
    private readonly IPasswordHasher _hasher;

    public RegistrarUsuarioUseCase(
        IUsuarioRepository usuarios,
        IPasswordHasher hasher)
    {
        _usuarios = usuarios;
        _hasher = hasher;
    }

    public async Task<Usuario> EjecutarAsync(
        string nombre,
        string correo,
        string password)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException(
                "El nombre es obligatorio.");

        if (string.IsNullOrWhiteSpace(correo))
            throw new ArgumentException(
                "El correo es obligatorio.");

        if (string.IsNullOrWhiteSpace(password)
            || password.Length < 8)
            throw new ArgumentException(
                "La contraseña debe tener al menos 8 caracteres.");

        nombre = nombre.Trim();
        correo = correo.Trim();

        var existente =
            await _usuarios.ObtenerPorCorreoAsync(correo);

        if (existente != null)
            throw new InvalidOperationException(
                "El correo ya esta registrado.");

        var usuario = new Usuario
        {
            Nombre = nombre,
            Correo = correo,
            PasswordHash = _hasher.HashPassword(password)
        };

        return await _usuarios.RegistrarAsync(usuario);
    }
}
