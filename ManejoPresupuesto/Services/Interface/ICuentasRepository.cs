using ManejoPresupuesto.Models;

namespace ManejoPresupuesto.Services.Interface
{
    public interface ICuentasRepository
    {
        Task Crear(Cuenta cuenta);
    }
}
