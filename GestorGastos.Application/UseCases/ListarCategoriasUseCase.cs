
using GestorGastos.Application.Interfaces;
using GestorGastos.Domain.Entities;

namespace GestorGastos.Application.UseCases;

public class ListarCategoriasUseCase
{
    private readonly ICategoriaRepository _categorias;

    public ListarCategoriasUseCase(
        ICategoriaRepository categorias)
    {
        _categorias = categorias;
    }

    public async Task<List<Categoria>> EjecutarAsync()
    {
        return await _categorias.ListarAsync();
    }
}
