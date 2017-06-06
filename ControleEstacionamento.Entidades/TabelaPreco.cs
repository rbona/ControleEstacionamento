using ControleEstacionamento.Lib.Validacoes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControleEstacionamento.Entidades
{
    [Table(name: "TabelasPrecos")]
    public class TabelaPreco
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long handle { get; set; }

        [Required(ErrorMessage = "Deve ser informada uma descrição.")]
        [Vazio(ErrorMessage = "Deve ser informada uma descrição.")]
        public string descricao { get; set; }

        [Required(ErrorMessage = "Deve ser informado o início da vigência da tabela de preços.")]
        [Vazio(ErrorMessage = "Deve ser informado o início da vigência da tabela de preços.")]
        [EntreDuasDatas(ErrorMessage = "A data de início da vigência deve ser menor ou igual ao do final da vigência", NomeCampoDataValidar = "finalVigencia", Operacao = Lib.Enumerados.OperacaoIgualdade.menor_ou_igual)]
        public DateTime inicioVigencia { get; set; }

        [Required(ErrorMessage = "Deve ser informado o final da vigência da tabela de preços.")]
        [Vazio(ErrorMessage = "Deve ser informado o final da vigência da tabela de preços.")]
        [EntreDuasDatas(ErrorMessage = "A data de final da vigência deve ser maior ou igual ao de início da vigência", NomeCampoDataValidar = "inicioVigencia", Operacao = Lib.Enumerados.OperacaoIgualdade.maior_ou_igual)]
        public DateTime finalVigencia { get; set; }

        [Required(ErrorMessage = "Deve ser informado o valor da hora inicial.")]
        [Vazio(ErrorMessage = "Deve ser informado o valor da hora inicial.")]
        [ValorMaiorQueZero(ErrorMessage ="O valor da hora inicial deve ser maior do que zero.")]
        public decimal valorHoraInicial { get; set; }

        [Required(ErrorMessage = "Deve ser informado o adicional por hora.")]
        [Vazio(ErrorMessage = "Deve ser informado o adicional por hora.")]
        [ValorMaiorQueZero(ErrorMessage ="O valor da hora adicional deve ser maior do que zero.")]
        public decimal valorHoraAdicional { get; set; }
    }
}