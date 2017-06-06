using System;
using System.ComponentModel.DataAnnotations;

namespace ControleEstacionamento.Lib.Validacoes
{
    public class Vazio : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ValidationResult retorno = null;

            if ((value is string && string.IsNullOrEmpty(value.ToString()))
                || (value is long && Convert.ToInt64(value) == 0)
                || (value is int && Convert.ToInt32(value) == 0)
                || (value is DateTime && Convert.ToDateTime(value).Equals(new DateTime()))
                || (value is decimal && Convert.ToDecimal(value) == 0)
                )
                retorno = new ValidationResult(this.ErrorMessage);
            else
                retorno = ValidationResult.Success;

            return retorno;
        }
    }
}
