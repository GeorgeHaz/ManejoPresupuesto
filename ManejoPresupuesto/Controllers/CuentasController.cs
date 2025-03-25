using ManejoPresupuesto.Models;
using ManejoPresupuesto.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
                return View(cuentaCreacion);
            }

            await cuentasRepository.Crear(cuentaCreacion);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = servicioUsuario.ObtenerUusario();

            var cuentasConTipoCuenta = await cuentasRepository.Buscar(usuarioId);
            var model = cuentasConTipoCuenta
                .GroupBy(x => x.TipoCuenta)
                .Select(grupo => new IndiceCuenta
                {
                    TipoCuenta = grupo.Key,
                    Cuentas = grupo.AsEnumerable()
                }).ToList();

            return View(model);
        }

        private async Task<IEnumerable<SelectListItem>> ObtenerTipoCuenta(int usuarioId)
        {
            var tiposCuentas = await tiposCuentasRepository.Listar(usuarioId);

            return tiposCuentas.Select(p => new SelectListItem(p.Nombre, p.TipoCuentaId.ToString()));
        } 
    }
}
