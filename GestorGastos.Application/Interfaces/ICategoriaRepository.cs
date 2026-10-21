
using GestorGastos.Domain.Entities;

namespace GestorGastos.Application.Interfaces;

public interface ICategoriaRepository
{
    Task<List<Categoria>> ListarAsync();
}
