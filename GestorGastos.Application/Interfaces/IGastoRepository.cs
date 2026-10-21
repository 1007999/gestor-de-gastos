
using GestorGastos.Domain.Entities;

namespace GestorGastos.Application.Interfaces;

public interface IGastoRepository
{
    Task<Gasto> CrearAsync(Gasto gasto);

    Task<Gasto?> ObtenerPorIdAsync(
        int gastoId,
        int usuarioId);

    Task<(List<Gasto> Gastos, int TotalRegistros)>
        ListarAsync(
            int usuarioId,
            int? categoriaId,
            DateTime? fechaInicio,
            DateTime? fechaFin,
            string? texto,
            int pagina,
            int tamanoPagina);

    Task<bool> ActualizarAsync(Gasto gasto);

    Task<bool> EliminarAsync(
        int gastoId,
        int usuarioId);
}
