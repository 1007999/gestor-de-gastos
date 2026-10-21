
using GestorGastos.Domain.Entities;

namespace GestorGastos.Application.Interfaces;

public interface ITokenService
{
    string GenerarToken(Usuario usuario);
}
