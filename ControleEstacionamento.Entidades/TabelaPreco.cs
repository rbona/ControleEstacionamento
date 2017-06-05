using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControleEstacionamento.Entidades
{
    [Table(name:"TabelasPrecos")]
    public class TabelaPreco
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long handle { get; set; }

        [Required(ErrorMessage ="Deve ser informada a descrição da tabela de preços.")]
        public string descricao { get; set; }

        [Required(ErrorMessage ="Deve ser informado o início da vigência da tabela de preços.")]
        public DateTime inicioVigencia { get; set; }

        [Required(ErrorMessage = "Deve ser informado o final da vigência da tabela de preços.")]
        public DateTime finalVigencia { get; set; }

        [Required(ErrorMessage = "Deve ser informado o valor da hora inicial.")]
        public decimal valorHoraInicial { get; set; }

        [Required(ErrorMessage = "Deve ser informado o adicional por hora.")]
        public decimal valorHoraAdicional { get; set; }
    }
}