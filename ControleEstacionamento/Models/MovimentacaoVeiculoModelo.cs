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
    }
}