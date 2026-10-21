
using System.Data;
using Microsoft.Data.SqlClient;
using GestorGastos.Application.Interfaces;
using GestorGastos.Domain.Entities;

namespace GestorGastos.Infrastructure.Repositories;

public class CategoriaRepository : ICategoriaRepository
{
    private readonly string _connectionString;

    public CategoriaRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<List<Categoria>> ListarAsync()
    {
        var categorias = new List<Categoria>();

        using var conexion =
            new SqlConnection(_connectionString);

        using var comando =
            new SqlCommand(
                "dbo.sp_Categoria_Listar",
                conexion);

        comando.CommandType =
            CommandType.StoredProcedure;

        await conexion.OpenAsync();

        using var reader =
            await comando.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var categoria = new Categoria
            {
                Id = reader.GetInt32(
                    reader.GetOrdinal("Id")),

                Nombre = reader.GetString(
                    reader.GetOrdinal("Nombre"))
            };

            categorias.Add(categoria);
        }

        return categorias;
    }
}
