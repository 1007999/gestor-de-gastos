
using GestorGastos.Application.Interfaces;
using GestorGastos.Domain.Entities;

namespace GestorGastos.Application.UseCases;

public class ListarGastosUseCase
{
    private readonly IGastoRepository _gastos;

    public ListarGastosUseCase(IGastoRepository gastos)
    {
        _gastos = gastos;
    }

    public async Task<(List<Gasto> Gastos, int TotalRegistros)>
        EjecutarAsync(
            int usuarioId,
            int? categoriaId = null,
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null,
            string? texto = null,
            int pagina = 1,
            int tamanoPagina = 10)
    {
        if (usuarioId <= 0)
            throw new ArgumentException(
                "Usuario no valido.");

        if (pagina < 1 || tamanoPagina < 1 || tamanoPagina > 100)
            throw new ArgumentException(
                "Paginacion no valida.");

        if (fechaInicio.HasValue && fechaFin.HasValue
            && fechaInicio > fechaFin)
            throw new ArgumentException(
                "Rango de fechas no valido.");

        return await _gastos.ListarAsync(
            usuarioId,
            categoriaId,
            fechaInicio,
            fechaFin,
            texto,
            pagina,
            tamanoPagina);
    }
}
