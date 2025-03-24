using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ManejoPresupuesto.Models
{
    public class CuentaCreacion:Cuenta
    {
        public IEnumerable<SelectListItem> TiposCuentas { get; set; }
    }
}
