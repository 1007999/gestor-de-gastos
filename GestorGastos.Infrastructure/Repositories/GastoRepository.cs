
using System.Data;
using Microsoft.Data.SqlClient;
using GestorGastos.Application.Interfaces;
using GestorGastos.Domain.Entities;

namespace GestorGastos.Infrastructure.Repositories;

public class GastoRepository : IGastoRepository
{
    private readonly string _connectionString;

    public GastoRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    // 1. CREAR GASTO
    public async Task<Gasto> CrearAsync(Gasto gasto)
    {
        using var conexion = new SqlConnection(_connectionString);
        using var comando = new SqlCommand(
            "dbo.sp_Gasto_Crear", conexion);

        comando.CommandType = CommandType.StoredProcedure;

        comando.Parameters.Add("@UsuarioId", SqlDbType.Int)
            .Value = gasto.UsuarioId;

        comando.Parameters.Add("@CategoriaId", SqlDbType.Int)
            .Value = gasto.CategoriaId;

        comando.Parameters.Add("@Descripcion", SqlDbType.NVarChar, 200)
            .Value = gasto.Descripcion;

        var importe = comando.Parameters.Add(
            "@Importe", SqlDbType.Decimal);
        importe.Precision = 18;
        importe.Scale = 2;
        importe.Value = gasto.Importe;

        comando.Parameters.Add("@Fecha", SqlDbType.Date)
            .Value = gasto.Fecha.Date;

        await conexion.OpenAsync();

        using var reader = await comando.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            throw new InvalidOperationException(
                "No se pudo crear el gasto.");

        return MapearGasto(reader, gasto.UsuarioId);
    }

    // 2. OBTENER GASTO POR ID
    public async Task<Gasto?> ObtenerPorIdAsync(
        int gastoId, int usuarioId)
    {
        using var conexion = new SqlConnection(_connectionString);
        using var comando = new SqlCommand(
            "dbo.sp_Gasto_ObtenerPorId", conexion);

        comando.CommandType = CommandType.StoredProcedure;

        comando.Parameters.Add("@Id", SqlDbType.Int)
            .Value = gastoId;

        comando.Parameters.Add("@UsuarioId", SqlDbType.Int)
            .Value = usuarioId;

        await conexion.OpenAsync();

        using var reader = await comando.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            return null;

        return MapearGasto(reader, usuarioId);
    }

    // 3. LISTAR GASTOS CON FILTROS
    public async Task<(List<Gasto> Gastos, int TotalRegistros)>
        ListarAsync(
            int usuarioId,
            int? categoriaId,
            DateTime? fechaInicio,
            DateTime? fechaFin,
            string? texto,
            int pagina,
            int tamanoPagina)
    {
        var gastos = new List<Gasto>();
        int total = 0;

        using var conexion = new SqlConnection(_connectionString);
        using var comando = new SqlCommand(
            "dbo.sp_Gasto_Listar", conexion);

        comando.CommandType = CommandType.StoredProcedure;

        comando.Parameters.Add("@UsuarioId", SqlDbType.Int)
            .Value = usuarioId;

        comando.Parameters.Add("@CategoriaId", SqlDbType.Int)
            .Value = (object?)categoriaId ?? DBNull.Value;

        comando.Parameters.Add("@FechaInicio", SqlDbType.Date)
            .Value = (object?)fechaInicio?.Date ?? DBNull.Value;

        comando.Parameters.Add("@FechaFin", SqlDbType.Date)
            .Value = (object?)fechaFin?.Date ?? DBNull.Value;

        comando.Parameters.Add("@Texto", SqlDbType.NVarChar, 200)
            .Value = (object?)texto ?? DBNull.Value;

        comando.Parameters.Add("@Pagina", SqlDbType.Int)
            .Value = pagina;

        comando.Parameters.Add("@TamanoPagina", SqlDbType.Int)
            .Value = tamanoPagina;

        await conexion.OpenAsync();

        using var reader = await comando.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            gastos.Add(MapearGasto(reader, usuarioId));

            total = reader.GetInt32(
                reader.GetOrdinal("TotalRegistros"));
        }

        return (gastos, total);
    }

    // 4. ACTUALIZAR GASTO
    public async Task<bool> ActualizarAsync(Gasto gasto)
    {
        using var conexion = new SqlConnection(_connectionString);
        using var comando = new SqlCommand(
            "dbo.sp_Gasto_Actualizar", conexion);

        comando.CommandType = CommandType.StoredProcedure;

        comando.Parameters.Add("@Id", SqlDbType.Int)
            .Value = gasto.Id;

        comando.Parameters.Add("@UsuarioId", SqlDbType.Int)
            .Value = gasto.UsuarioId;

        comando.Parameters.Add("@CategoriaId", SqlDbType.Int)
            .Value = gasto.CategoriaId;

        comando.Parameters.Add("@Descripcion", SqlDbType.NVarChar, 200)
            .Value = gasto.Descripcion;

        var importe = comando.Parameters.Add(
            "@Importe", SqlDbType.Decimal);
        importe.Precision = 18;
        importe.Scale = 2;
        importe.Value = gasto.Importe;

        comando.Parameters.Add("@Fecha", SqlDbType.Date)
            .Value = gasto.Fecha.Date;

        await conexion.OpenAsync();

        try
        {
            using var reader = await comando.ExecuteReaderAsync();
            return await reader.ReadAsync();
        }
        catch (SqlException ex) when (ex.Number == 50011)
        {
            return false;
        }
    }

    // 5. ELIMINAR GASTO
    public async Task<bool> EliminarAsync(
        int gastoId, int usuarioId)
    {
        using var conexion = new SqlConnection(_connectionString);
        using var comando = new SqlCommand(
            "dbo.sp_Gasto_Eliminar", conexion);

        comando.CommandType = CommandType.StoredProcedure;

        comando.Parameters.Add("@Id", SqlDbType.Int)
            .Value = gastoId;

        comando.Parameters.Add("@UsuarioId", SqlDbType.Int)
            .Value = usuarioId;

        await conexion.OpenAsync();

        try
        {
            using var reader = await comando.ExecuteReaderAsync();
            return await reader.ReadAsync();
        }
        catch (SqlException ex) when (ex.Number == 50012)
        {
            return false;
        }
    }

    // CONVERTIR RESULTADOS SQL A OBJETOS C#
    private static Gasto MapearGasto(
        SqlDataReader reader, int usuarioId)
    {
        int id = reader.GetInt32(
            reader.GetOrdinal("Id"));

        int categoriaId = reader.GetInt32(
            reader.GetOrdinal("CategoriaId"));

        string descripcion = reader.GetString(
            reader.GetOrdinal("Descripcion"));

        decimal importe = reader.GetDecimal(
            reader.GetOrdinal("Importe"));

        DateTime fecha = reader.GetDateTime(
            reader.GetOrdinal("Fecha"));

        var gasto = new Gasto(
            usuarioId,
            categoriaId,
            descripcion,
            importe,
            fecha);

        gasto.Id = id;

        // Algunos procedimientos devuelven FechaCreacion,
        // mientras que el listado no la incluye.
        try
        {
            int indice = reader.GetOrdinal("FechaCreacion");

            if (!reader.IsDBNull(indice))
                gasto.FechaCreacion =
                    reader.GetDateTime(indice);
        }
        catch (IndexOutOfRangeException)
        {
            // La columna no viene en este resultado.
        }

        return gasto;
    }
}
