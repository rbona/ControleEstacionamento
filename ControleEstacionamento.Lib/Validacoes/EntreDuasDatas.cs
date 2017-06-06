using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ControleEstacionamento.Lib.Enumerados;

namespace ControleEstacionamento.Lib.Validacoes
{
    public class EntreDuasDatas : ValidationAttribute
    {
        public string NomeCampoDataValidar { get; set; }
        public OperacaoIgualdade Operacao { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ValidationResult retorno = null;

            if (string.IsNullOrEmpty(NomeCampoDataValidar))
                retorno = new ValidationResult("Deve ser informado o campo de data para validação");
            else
            {
                try
                {
                    DateTime data1 = Convert.ToDateTime(value);
                    DateTime data2 = Convert.ToDateTime(validationContext.ObjectInstance.GetType().GetProperty(NomeCampoDataValidar).GetValue(validationContext.ObjectInstance));

                    if ((Operacao == OperacaoIgualdade.igual && DateTime.Compare(data1, data2) == 0)
                        || (Operacao == OperacaoIgualdade.diferente && DateTime.Compare(data1, data2) != 0)
                        || (Operacao == OperacaoIgualdade.maior && DateTime.Compare(data1, data2) == 1)
                        || (Operacao == OperacaoIgualdade.maior_ou_igual && (new List<int> { 0, 1 }).Contains(DateTime.Compare(data1, data2)))
                        || (Operacao == OperacaoIgualdade.menor && DateTime.Compare(data1, data2) == -1)
                        || (Operacao == OperacaoIgualdade.menor_ou_igual && (new List<int> { 0, -1 }).Contains(DateTime.Compare(data1, data2)))
                        )
                        retorno = ValidationResult.Success;
                    else
                        retorno = new ValidationResult(ErrorMessage);
                }catch(Exception ex)
                {
                    retorno = new ValidationResult("Houve um erro na validação do campo. " + ex.Message);
                }
            }

            return retorno;
        }
    }
}
