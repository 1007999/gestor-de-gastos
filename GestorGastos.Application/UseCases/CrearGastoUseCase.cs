
using GestorGastos.Application.Interfaces;
using GestorGastos.Domain.Entities;

namespace GestorGastos.Application.UseCases;

public class CrearGastoUseCase
{
    private readonly IGastoRepository _gastos;

    public CrearGastoUseCase(
        IGastoRepository gastos)
    {
        _gastos = gastos;
    }

    public async Task<Gasto> EjecutarAsync(
        int usuarioId,
        int categoriaId,
        string descripcion,
        decimal importe,
        DateTime fecha)
    {
        if (usuarioId <= 0)
            throw new ArgumentException(
                "El usuario no es valido.");

        if (categoriaId <= 0)
            throw new ArgumentException(
                "La categoria no es valida.");

        var gasto = new Gasto(
            usuarioId,
            categoriaId,
            descripcion,
            importe,
            fecha
        );

        return await _gastos.CrearAsync(gasto);
    }
}
