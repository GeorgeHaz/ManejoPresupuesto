using Dapper;
using ManejoPresupuesto.Models;
using ManejoPresupuesto.Services.Interface;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace ManejoPresupuesto.Services.Repository
{
    public class TiposCuentasRepository : ITiposCuentasRepository
    {
        protected readonly string? _connectionString;

        public TiposCuentasRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(TipoCuenta tipoCuenta)
        {
            using var connection = new SqlConnection(_connectionString);
            var id = await connection.QuerySingleAsync<int>
                                            ("TIPOS_CUENTAS_INSERTAR", new {usuarioId = tipoCuenta.UsuarioId,
                                            nombre = tipoCuenta.Nombre},
                                            commandType: System.Data.CommandType.StoredProcedure);
            tipoCuenta.TipoCuentaId = id;
        }

        public async Task<bool> Existe(string nombre, int usuarioId)
        {
            using var connection = new SqlConnection(_connectionString);
            var existe = await connection.QueryFirstOrDefaultAsync<int>($@"SELECT 1 FROM TiposCuentas
                                        WHERE Nombre=@Nombre AND UsuarioId=@UsuarioId;", new {nombre,usuarioId});

            return existe == 1;
        }

        public async Task<IEnumerable<TipoCuenta>> Listar(int usuarioId)
        {
            using var conecction = new SqlConnection(_connectionString);
            return await conecction.QueryAsync<TipoCuenta>($@"SELECT * FROM TiposCuentas
                                        WHERE UsuarioId = @UsuarioId ORDER BY Orden;", new {usuarioId});


        }

        public async Task<TipoCuenta?> ObtenerPorId(int? id,int? usuarioId)
        {
            using var connection = new SqlConnection(_connectionString);

            return await connection.QueryFirstOrDefaultAsync<TipoCuenta>(
                        $@"SELECT TipoCuentaId,Nombre,Orden FROM TiposCuentas
                        WHERE TipoCuentaId = @Id AND UsuarioId=@UsuarioId", new {id,usuarioId});
        }

        public async Task Editar(TipoCuenta tipoCuenta)
        {
            using var connection = new SqlConnection(_connectionString);

            await connection.ExecuteAsync(@$"UPDATE TiposCuentas SET Nombre=@Nombre
                                                    WHERE TipoCuentaId=@TipoCuentaId", tipoCuenta);
        }

        public async Task Eliminar(int tipoCuentaId)
        {
            using var connection = new SqlConnection(_connectionString);

            await connection.ExecuteAsync($@"DELETE TiposCuentas WHERE TipoCuentaId = @tipoCuentaId;", new { tipoCuentaId });
        }

        public async Task Ordenar(IEnumerable<TipoCuenta> tipoCuentas)
        {
            var query = "UPDATE TiposCuentas SET Orden = @Orden WHERE TipoCuentaId = @TipoCuentaId";

            using var connetion = new SqlConnection(_connectionString);

            await connetion.ExecuteAsync(query,tipoCuentas);
        }
    }
}
