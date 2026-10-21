
using GestorGastos.Application.Interfaces;

namespace GestorGastos.Application.UseCases;

public class ActualizarGastoUseCase
{
    private readonly IGastoRepository _gastos;

    public ActualizarGastoUseCase(IGastoRepository gastos)
    {
        _gastos = gastos;
    }

    public async Task<bool> EjecutarAsync(
        int gastoId,
        int usuarioId,
        int categoriaId,
        string descripcion,
        decimal importe,
        DateTime fecha)
    {
        if (gastoId <= 0 || usuarioId <= 0)
            throw new ArgumentException(
                "Identificador no valido.");

        if (categoriaId <= 0)
            throw new ArgumentException(
                "Categoria no valida.");

        var gasto = await _gastos.ObtenerPorIdAsync(
            gastoId, usuarioId);

        if (gasto == null)
            return false;

        gasto.CategoriaId = categoriaId;
        gasto.Fecha = fecha;
        gasto.ActualizarDatos(descripcion, importe);

        return await _gastos.ActualizarAsync(gasto);
    }
}
