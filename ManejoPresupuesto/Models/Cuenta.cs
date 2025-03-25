using ManejoPresupuesto.Models.Validations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace ManejoPresupuesto.Models
{
    public class Cuenta
    {
        public int CuentaId { get; set; }
        [Required(ErrorMessage ="El campo {0} es requerdio")]
        [StringLength(maximumLength:50)]
        [PrimeraLetraMayuscula]
        public string? Nombre { get; set; }
        [Display(Name ="Tipo de Cuenta")]
        public int TipoCuentaId { get; set; }
        public decimal Balance { get; set; }
        [StringLength(maximumLength:1000)]
        public string? Descripcion { get; set; }
        public string? TipoCuenta { get; set; }
    }
}