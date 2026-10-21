
using GestorGastos.Application.Interfaces;

namespace GestorGastos.Application.UseCases;

public class EliminarGastoUseCase
{
    private readonly IGastoRepository _gastos;

    public EliminarGastoUseCase(
        IGastoRepository gastos)
    {
        _gastos = gastos;
    }

    public async Task<bool> EjecutarAsync(
        int gastoId,
        int usuarioId)
    {
        if (gastoId <= 0 || usuarioId <= 0)
            throw new ArgumentException(
                "Identificador no valido.");

        return await _gastos.EliminarAsync(
            gastoId,
            usuarioId);
    }
}
