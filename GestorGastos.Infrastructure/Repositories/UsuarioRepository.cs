
using System.Data;
using Microsoft.Data.SqlClient;
using GestorGastos.Application.Interfaces;
using GestorGastos.Domain.Entities;

namespace GestorGastos.Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly string _connectionString;

    public UsuarioRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<Usuario?> ObtenerPorCorreoAsync(
        string correo)
    {
        using var conexion =
            new SqlConnection(_connectionString);

        using var comando =
            new SqlCommand(
                "dbo.sp_Usuario_ObtenerPorCorreo",
                conexion);

        comando.CommandType = CommandType.StoredProcedure;

        comando.Parameters.Add(
            "@Correo", SqlDbType.NVarChar, 150
        ).Value = correo;

        await conexion.OpenAsync();

        using var reader =
            await comando.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            return null;

        return MapearUsuario(reader);
    }

    public async Task<Usuario?> ObtenerPerfilAsync(
        int usuarioId)
    {
        using var conexion =
            new SqlConnection(_connectionString);

        using var comando =
            new SqlCommand(
                "dbo.sp_Usuario_ObtenerPerfil",
                conexion);

        comando.CommandType = CommandType.StoredProcedure;

        comando.Parameters.Add(
            "@UsuarioId", SqlDbType.Int
        ).Value = usuarioId;

        await conexion.OpenAsync();

        using var reader =
            await comando.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            return null;

        return MapearUsuario(reader);
    }

    public async Task<Usuario> RegistrarAsync(
        Usuario usuario)
    {
        using var conexion =
            new SqlConnection(_connectionString);

        using var comando =
            new SqlCommand(
                "dbo.sp_Usuario_Registrar",
                conexion);

        comando.CommandType = CommandType.StoredProcedure;

        comando.Parameters.Add(
            "@Nombre", SqlDbType.NVarChar, 100
        ).Value = usuario.Nombre;

        comando.Parameters.Add(
            "@Correo", SqlDbType.NVarChar, 150
        ).Value = usuario.Correo;

        comando.Parameters.Add(
            "@PasswordHash", SqlDbType.NVarChar, 255
        ).Value = usuario.PasswordHash;

        await conexion.OpenAsync();

        using var reader =
            await comando.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            throw new InvalidOperationException(
                "No se pudo registrar el usuario.");

        var registrado = MapearUsuario(reader);

        // El procedimiento de registro no devuelve
        // el hash, por lo que se conserva internamente.
        registrado.PasswordHash = usuario.PasswordHash;

        return registrado;
    }

    private static Usuario MapearUsuario(
        SqlDataReader reader)
    {
        var usuario = new Usuario
        {
            Id = reader.GetInt32(
                reader.GetOrdinal("Id")),

            Nombre = reader.GetString(
                reader.GetOrdinal("Nombre")),

            Correo = reader.GetString(
                reader.GetOrdinal("Correo")),

            FechaCreacion = reader.GetDateTime(
                reader.GetOrdinal("FechaCreacion"))
        };

        // El perfil y el registro no devuelven
        // PasswordHash; la consulta por correo sí.
        try
        {
            int indice = reader.GetOrdinal("PasswordHash");

            if (!reader.IsDBNull(indice))
                usuario.PasswordHash =
                    reader.GetString(indice);
        }
        catch (IndexOutOfRangeException)
        {
            // Columna no incluida en este resultado.
        }

        return usuario;
    }
}
