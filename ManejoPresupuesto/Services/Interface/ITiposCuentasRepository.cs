using ManejoPresupuesto.Models;

namespace ManejoPresupuesto.Services.Interface
{
    public interface ITiposCuentasRepository
    {
        Task Crear(TipoCuenta tipoCuenta);
        Task Editar(TipoCuenta tipoCuenta);
        Task Eliminar(int id);
        Task<bool> Existe(string nombre, int usuarioId);
        Task<IEnumerable<TipoCuenta>> Listar(int usuarioId);
        Task<TipoCuenta?> ObtenerPorId(int? id, int? usuarioId);
        Task Ordenar(IEnumerable<TipoCuenta> tipoCuentas);
    }
}
