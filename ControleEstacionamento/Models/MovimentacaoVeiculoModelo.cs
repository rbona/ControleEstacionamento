using ControleEstacionamento.DAL.Repositorios;
using ControleEstacionamento.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControleEstacionamento.Models
{
    public class MovimentacaoVeiculoModelo
    {
        private TabelaPrecosModelo _tabePrecMode = new TabelaPrecosModelo();
        private TabelaPrecoRepositorio _tabePrecRepo = new TabelaPrecoRepositorio();

        public MovimentacaoVeiculo BuscarMovimentacaoVeiculoEmAberto(string placa)
        {
            MovimentacaoVeiculoRepositorio moviVeicRepo = new MovimentacaoVeiculoRepositorio();
            MovimentacaoVeiculo movimentacao = null;

            var movimentacoes = moviVeicRepo.Buscar(movi => movi.placa.Equals(placa)
                                                            && movi.saida.Equals(new DateTime())
                                            ).ToList();

            if (movimentacoes.Count() > 0)
                movimentacao = movimentacoes.First();

            return movimentacao;
        }

        public MovimentacaoVeiculo CriarAtualizarMovimentacaoVeiculo(string placa, DateTime dataHoraAtual)
        {
            MovimentacaoVeiculo moviVeic = BuscarMovimentacaoVeiculoEmAberto(placa);

            dataHoraAtual = dataHoraAtual.AddSeconds(dataHoraAtual.Second * -1);
            dataHoraAtual = dataHoraAtual.AddMilliseconds(dataHoraAtual.Millisecond * -1);

            if (moviVeic == null)
                moviVeic = new MovimentacaoVeiculo() {
                    placa = placa,
                    entrada = dataHoraAtual,
                    idTabelaPreco = _tabePrecMode.BuscarIdTabelaPreco(dataHoraAtual)
                };

            else if (moviVeic.idTabelaPreco <= 0)
                moviVeic.idTabelaPreco = _tabePrecMode.BuscarIdTabelaPreco(moviVeic.entrada);

            if (moviVeic.handle > 0)
                moviVeic.saida = dataHoraAtual;

            if (moviVeic.tabelaPreco == null)
                moviVeic.tabelaPreco = _tabePrecRepo.Buscar(tabe => tabe.handle == moviVeic.idTabelaPreco).FirstOrDefault();

            return moviVeic;
        }
    }
}