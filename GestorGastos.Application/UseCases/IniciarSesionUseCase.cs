
using GestorGastos.Application.Interfaces;

namespace GestorGastos.Application.UseCases;

public class IniciarSesionUseCase
{
    private readonly IUsuarioRepository _usuarios;
    private readonly IPasswordHasher _hasher;
    private readonly ITokenService _tokens;

    public IniciarSesionUseCase(
        IUsuarioRepository usuarios,
        IPasswordHasher hasher,
        ITokenService tokens)
    {
        _usuarios = usuarios;
        _hasher = hasher;
        _tokens = tokens;
    }

    public async Task<string> EjecutarAsync(
        string correo,
        string password)
    {
        if (string.IsNullOrWhiteSpace(correo) ||
            string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException(
                "Correo y contraseña obligatorios.");
        }

        var usuario = await _usuarios
            .ObtenerPorCorreoAsync(correo.Trim());

        if (usuario == null ||
            !_hasher.VerifyPassword(
                password, usuario.PasswordHash))
        {
            throw new UnauthorizedAccessException(
                "Credenciales incorrectas.");
        }

        return _tokens.GenerarToken(usuario);
    }
}
