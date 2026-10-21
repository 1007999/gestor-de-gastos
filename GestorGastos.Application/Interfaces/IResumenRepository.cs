
namespace GestorGastos.Application.Interfaces;

public interface IResumenRepository
{
    Task<decimal> ObtenerTotalMensualAsync(
        int usuarioId,
        int anio,
        int mes);

    Task<List<ResumenCategoria>>
        ObtenerPorCategoriaAsync(
            int usuarioId,
            int anio,
            int mes);
}

public class ResumenCategoria
{
    public int CategoriaId { get; set; }

    public string Categoria { get; set; } = string.Empty;

    public int CantidadGastos { get; set; }

    public decimal Total { get; set; }
}
