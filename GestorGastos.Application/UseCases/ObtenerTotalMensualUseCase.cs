
using GestorGastos.Application.Interfaces;

namespace GestorGastos.Application.UseCases;

public class ObtenerTotalMensualUseCase
{
    private readonly IResumenRepository _resumen;

    public ObtenerTotalMensualUseCase(
        IResumenRepository resumen)
    {
        _resumen = resumen;
    }

    public async Task<decimal> EjecutarAsync(
        int usuarioId,
        int anio,
        int mes)
    {
        if (usuarioId <= 0)
            throw new ArgumentException(
                "Usuario no valido.");

        if (anio < 1 || anio > 9999 ||
            mes < 1 || mes > 12)
            throw new ArgumentException(
                "Anio o mes no valido.");

        return await _resumen.ObtenerTotalMensualAsync(
            usuarioId, anio, mes);
    }
}
