using ManejoPresupuesto.Models;
using ManejoPresupuesto.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ManejoPresupuesto.Controllers
{
    public class TiposCuentasController : Controller
    {
        private readonly ITiposCuentasRepository _tiposCuentasRepository;
        private readonly IServicioUsuario _servicioUsuario;

        public TiposCuentasController(ITiposCuentasRepository tiposCuentasRepository, IServicioUsuario servicioUsuario)
        {
            _tiposCuentasRepository = tiposCuentasRepository;
            _servicioUsuario = servicioUsuario;
        }

        [HttpGet]
        public IActionResult Crear()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(TipoCuenta tiposCuentas)
        {
            tiposCuentas.UsuarioId = _servicioUsuario.ObtenerUusario();

            if (!ModelState.IsValid)
            {
                return View(tiposCuentas);
            }

            var yaExiste = await _tiposCuentasRepository.Existe(tiposCuentas.Nombre, tiposCuentas.UsuarioId);

            if (yaExiste)
            {
                ModelState.AddModelError(nameof(tiposCuentas.Nombre), $"El nombre {tiposCuentas.Nombre} ya existe.");
                return View(tiposCuentas);
            }

            await _tiposCuentasRepository.Crear(tiposCuentas);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> VerificarTipCuenta(string nombre)
        {
            int usuario = _servicioUsuario.ObtenerUusario();
            var yaExiste = await _tiposCuentasRepository.Existe(nombre, usuario);
            if (yaExiste)
            {
                return Json($"La cuenta {nombre} ya existe");
            }

            return Json(true);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var usuarioId = 1;
            var tiposCuentas = await _tiposCuentasRepository.Listar(usuarioId);

            return View(tiposCuentas);
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var usuarioId = _servicioUsuario.ObtenerUusario();
            var tiposCuentas = await _tiposCuentasRepository.ObtenerPorId(id, usuarioId);

            if(tiposCuentas is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(tiposCuentas);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(TipoCuenta tipoCuenta)
        {
            var usuarioId = _servicioUsuario.ObtenerUusario();
            var tipoCuentaExiste = await _tiposCuentasRepository.ObtenerPorId(tipoCuenta.TipoCuentaId, usuarioId);

            if(tipoCuentaExiste is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await _tiposCuentasRepository.Editar(tipoCuenta);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            var usuarioId = _servicioUsuario.ObtenerUusario();
            var tipoCuentaExiste = await _tiposCuentasRepository.ObtenerPorId(id, usuarioId);

            if(tipoCuentaExiste is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(tipoCuentaExiste);
        }

        [HttpPost]
        public async Task<IActionResult> EliminarSeleccionado(int tipoCuentaId)
        {
            var usuarioId = _servicioUsuario.ObtenerUusario();
            var tipoCuentaExiste = await _tiposCuentasRepository.ObtenerPorId(tipoCuentaId, usuarioId);

            if (tipoCuentaExiste is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await _tiposCuentasRepository.Eliminar(tipoCuentaId);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Ordenar([FromBody] int[] ids)
        {
            var usuarioId = _servicioUsuario.ObtenerUusario();
            var tiposCuentas = await _tiposCuentasRepository.Listar(usuarioId);
            var idsTiposCuentas = tiposCuentas.Select(x => x.TipoCuentaId);

            var idsTiposCuentasQueNoPertenecenAlUsuario = ids.Except(idsTiposCuentas).ToList();

            if(idsTiposCuentasQueNoPertenecenAlUsuario.Count > 0)
            {
                return Forbid();
            }

            var tiposCuentasOrdenadas = ids.Select((valor,indice) => 
                new TipoCuenta() { TipoCuentaId = valor, Orden = indice + 1}).AsEnumerable();

            await _tiposCuentasRepository.Ordenar(tiposCuentasOrdenadas);

            return Ok();
        }
    }
}
