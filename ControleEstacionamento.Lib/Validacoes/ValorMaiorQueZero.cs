using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleEstacionamento.Lib.Validacoes
{
    public class ValorMaiorQueZero : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ValidationResult retorno = new ValidationResult(ErrorMessage);

            if (value is decimal && Convert.ToDecimal(value) > 0)
                retorno = ValidationResult.Success;

            return retorno;
        }
    }
}
