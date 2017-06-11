using ControleEstacionamento.DAL.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControleEstacionamento.Models
{
    public class TabelaPrecosModelo
    {
        private TabelaPrecoRepositorio _tabePrecRepo = new TabelaPrecoRepositorio();

        public string VericarExisteConfiguracoes()
        {
            string mensagem = "";

            if (_tabePrecRepo.Buscar(tabe => DateTime.Compare(tabe.inicioVigencia.Date, DateTime.Now.Date) <= 0
                                                        && DateTime.Compare(tabe.finalVigencia.Date, DateTime.Now.Date) >= 0).Count() == 0)
                mensagem += "Não foi encontrado tabela de preços configurada." + Environment.NewLine;

            return mensagem;
        }

        public long BuscarIdTabelaPreco(DateTime dataHoraAtual)
        {
            long id = 0;
            var tabelas = _tabePrecRepo.Buscar(tabe => DateTime.Compare(tabe.inicioVigencia, dataHoraAtual.Date) <= 0
                                                        && DateTime.Compare(tabe.finalVigencia, dataHoraAtual.Date) >= 0 )
                                        .ToList();
            if (tabelas.Count() > 0)
                id = tabelas.First().handle;

            return id;
        }
    }
}