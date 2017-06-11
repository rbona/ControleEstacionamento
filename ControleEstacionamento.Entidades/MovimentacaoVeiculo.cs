using ControleEstacionamento.Lib.Validacoes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleEstacionamento.Entidades
{
    [Table(name: "MovimentacoesVeiculos")]
    public class MovimentacaoVeiculo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public long handle { get; set; }

        [Required(ErrorMessage = "Deve ser informada a placa do veículo.")]
        [Vazio(ErrorMessage = "Deve ser informada a placa do veículo.")]
        [Display(Name = "Placa")]
        [RegularExpression(@"^[a-zA-Z]{3}[0-9]{4}$", ErrorMessage = "A placa informada está fora do padrão. (Exemplo: AAA9999)")]
        public string placa { get; set; }

        [Required(ErrorMessage = "Deve ser informado a data e hora de entrada.")]
        [Vazio(ErrorMessage = "Deve ser informado a data e hora de entrada.")]
        [Display(Name = "Entrada Veículo")]
        [DataType(DataType.DateTime)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-ddTHH:mm:ss}")]
        public DateTime entrada { get; set; }

        [Display(Name = "Saída Veículo")]
        [DataType(DataType.DateTime)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-ddTHH:mm:ss}")]
        public DateTime saida { get; set; }

        [Display(Name = "ID Tabela Preço")]
        [Column("tabelaPreco")]
        public long idTabelaPreco { get; set; }

        [Display(Name = "Tabela de Preço")]
        [ForeignKey("idTabelaPreco")]
        public TabelaPreco tabelaPreco { get; set; }

        [Display(Name = "Valor Total")]
        [NotMapped]
        [DataType(DataType.Currency)]
        public decimal valorTotal
        {
            get
            {
                return CalcularTotalEstacionamento();
            }
        }

        [Display(Name = "Tempo Total")]
        [NotMapped]
        public int tempoTotal
        {
            get
            {
                int tempo = 0;
                if (DateTime.Compare(entrada, new DateTime()) != 0
                    && DateTime.Compare(saida, new DateTime()) != 0)
                    tempo = Convert.ToInt32(Math.Truncate(saida.Subtract(entrada).TotalMinutes));
                return tempo;
            }
        }

        private decimal CalcularTotalEstacionamento()
        {
            decimal valorTotal = 0;

            if (tempoTotal > 0 && tabelaPreco != null)
            {
                if (tempoTotal <= 30)
                    valorTotal = tabelaPreco.valorHoraInicial / 2;
                else
                    valorTotal = tabelaPreco.valorHoraInicial;

                valorTotal += Convert.ToInt32(Math.Truncate(Convert.ToDecimal(tempoTotal) / 71)) * tabelaPreco.valorHoraAdicional;
            }

            return valorTotal;
        }
    }
}
