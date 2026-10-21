
using GestorGastos.Application.Interfaces;
using GestorGastos.Domain.Entities;

namespace GestorGastos.Application.UseCases;

public class ObtenerGastoUseCase
{
    private readonly IGastoRepository _gastos;

    public ObtenerGastoUseCase(
        IGastoRepository gastos)
    {
        _gastos = gastos;
    }

    public async Task<Gasto?> EjecutarAsync(
        int gastoId,
        int usuarioId)
    {
        if (gastoId <= 0 || usuarioId <= 0)
            throw new ArgumentException(
                "Identificador no valido.");

        return await _gastos.ObtenerPorIdAsync(
            gastoId,
            usuarioId);
    }
}
