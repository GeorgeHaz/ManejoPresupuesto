﻿using ManejoPresupuesto.Models.Validations;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ManejoPresupuesto.Models
{
    public class TipoCuenta
    {
        public int TipoCuentaId { get; set; }

        [Required(ErrorMessage ="El campo {0} es requerido")]
        [PrimeraLetraMayuscula]
        [Remote(action: "VerificarTipCuenta", controller:"TiposCuentas")]
        public string? Nombre { get; set; }
        public int UsuarioId { get; set; }
        public int Orden { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if(Nombre is not null && Nombre.Length > 0)
        //    {
        //        var primeraLetra = Nombre[0].ToString();

        //        if(primeraLetra != primeraLetra.ToUpper())
        //        {
        //            yield return new ValidationResult("La primera letra debe ser mayuscula", new[] { nameof(Nombre) });
        //        }
        //    }
        //}
    }
}
