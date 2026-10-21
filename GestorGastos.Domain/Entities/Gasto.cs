
namespace GestorGastos.Domain.Entities;

public class Gasto
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public int CategoriaId { get; set; }

    public string Descripcion { get; private set; } = string.Empty;
    public decimal Importe { get; private set; }

    public DateTime Fecha { get; set; }
    public DateTime FechaCreacion { get; set; }

    public Gasto(
        int usuarioId,
        int categoriaId,
        string descripcion,
        decimal importe,
        DateTime fecha)
    {
        UsuarioId = usuarioId;
        CategoriaId = categoriaId;
        Fecha = fecha;

        ActualizarDatos(descripcion, importe);
    }

    public void ActualizarDatos(
        string descripcion,
        decimal importe)
    {
        if (string.IsNullOrWhiteSpace(descripcion))
            throw new ArgumentException(
                "La descripcion es obligatoria.");

        if (descripcion.Length > 200)
            throw new ArgumentException(
                "La descripcion no puede superar 200 caracteres.");

        if (importe <= 0)
            throw new ArgumentException(
                "El importe debe ser mayor a cero.");

        Descripcion = descripcion;
        Importe = importe;
    }
}
