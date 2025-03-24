using ManejoPresupuesto.Models;
using ManejoPresupuesto.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace ManejoPresupuesto.Controllers
{
    public class CuentasController : Controller
    {
        private readonly ITiposCuentasRepository tiposCuentasRepository;
        private readonly IServicioUsuario servicioUsuario;
        private readonly ICuentasRepository cuentasRepository;

        public CuentasController(
            ITiposCuentasRepository tiposCuentasRepository,
            IServicioUsuario servicioUsuario,
            ICuentasRepository cuentasRepository)
        {
            this.tiposCuentasRepository = tiposCuentasRepository;
            this.servicioUsuario = servicioUsuario;
            this.cuentasRepository = cuentasRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            var usuarioId = servicioUsuario.ObtenerUusario();
            var tiposCuentas = await tiposCuentasRepository.Listar(usuarioId);
            var modelo = new CuentaCreacion();
            modelo.TiposCuentas = await ObtenerTipoCuenta(usuarioId);
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CuentaCreacion cuentaCreacion)
        {
            var usuarioId = servicioUsuario.ObtenerUusario();

            var tipoCuenta = await tiposCuentasRepository.ObtenerPorId(cuentaCreacion.TipoCuentaId, usuarioId);

            if(tipoCuenta is null)
            {
                return RedirectToAction("NoEncontrado","Home");
            }

            if (!ModelState.IsValid)
            {
                cuentaCreacion.TiposCuentas = await ObtenerTipoCuenta(usuarioId);
                foreach (var entry in ModelState)
                {
                    var key = entry.Key;
                    var errors = entry.Value.Errors;
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"Error en '{key}': {error.ErrorMessage}");
                    }
                }
                return View(cuentaCreacion);
            }

            await cuentasRepository.Crear(cuentaCreacion);

            return RedirectToAction("Index");
        }

        private async Task<IEnumerable<SelectListItem>> ObtenerTipoCuenta(int usuarioId)
        {
            var tiposCuentas = await tiposCuentasRepository.Listar(usuarioId);

            return tiposCuentas.Select(p => new SelectListItem(p.Nombre, p.TipoCuentaId.ToString()));
        } 
    }
}
