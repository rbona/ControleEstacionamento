using ControleEstacionamento.DAL.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControleEstacionamento.Models
{
    public class TabelaPrecosModelo
    {
        public string VericarExisteConfiguracoes()
        {
            TabelaPrecoRepositorio tabePrecRepo = new TabelaPrecoRepositorio();
            string mensagem = "";

            if (tabePrecRepo.Buscar(tabe => DateTime.Compare(tabe.inicioVigencia, DateTime.Now) <= 0
                                                        && DateTime.Compare(tabe.finalVigencia, DateTime.Now) >= 0).Count() == 0)
                mensagem += "Não foi encontrado tabela de preços configurada." + Environment.NewLine;

            return mensagem;
        }
    }
}