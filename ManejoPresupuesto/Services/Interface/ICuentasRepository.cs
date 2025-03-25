using ManejoPresupuesto.Models;

namespace ManejoPresupuesto.Services.Interface
{
    public interface ICuentasRepository
    {
        Task<IEnumerable<Cuenta>> Buscar(int usuarioId);
        Task Crear(Cuenta cuenta);
    }
}
