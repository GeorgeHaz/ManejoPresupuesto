using Dapper;
using ManejoPresupuesto.Models;
using ManejoPresupuesto.Services.Interface;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Services.Repository
{
    public class CuentasRepository:ICuentasRepository
    {
        private readonly string? _conectionString;
        public CuentasRepository(IConfiguration configuration)
        {
            _conectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(Cuenta cuenta)
        {
            using var connection = new SqlConnection(_conectionString);

            var id = await connection.QuerySingleAsync<int>(@"INSERT INTO Cuentas 
                            (Nombre,TipoCuentaId,Balance,Descripcion) 
                            VALUES (@Nombre,@TipoCuentaId,@Balance,@Descripcion);
                            SELECT SCOPE_IDENTITY();", cuenta);
            cuenta.TipoCuentaId = id;
        }

        public async Task<IEnumerable<Cuenta>> Buscar(int usuarioId)
        {
            using var connection = new SqlConnection(_conectionString);
            var response = await connection.QueryAsync<Cuenta>(@"Select 
	                                                                CuentaId,
	                                                                Cuentas.Nombre,
	                                                                Balance,
	                                                                tc.Nombre AS TipoCuenta
                                                                from Cuentas
                                                                INNER JOIN TiposCuentas tc ON tc.TipoCuentaId = Cuentas.TipoCuentaId
                                                                WHERE tc.UsuarioId = @UsuarioId
                                                                ORDER BY tc.Orden;", new { usuarioId });

            return response;
        }
    }
}
