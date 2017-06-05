using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControleEstacionamento.DAL.Repositorios;
using ControleEstacionamento.Entidades;
using System.Data.Entity.Validation;
using System.Linq;

namespace ControleEstacionamento.Testes
{
    [TestClass]
    public class InclusaoTabelaPrecos
    {
        [TestMethod]
        public void CamposObrigatorios()
        {
            using( TabelaPrecoRepositorio tabPreRep = new TabelaPrecoRepositorio())
            {
                TabelaPreco tabela = new TabelaPreco();

                try
                {
                    tabela.descricao = "Teste";
                    tabela.inicioVigencia = new DateTime(2017, 06, 01);
                    tabela.finalVigencia = new DateTime(2017, 06, 10);
                    tabela.valorHoraInicial = 2;
                    tabela.valorHoraAdicional = 1;
                    tabPreRep.Adicionar(tabela);
                    tabPreRep.Salvar();
                    
                    Assert.IsTrue(true);
                }
                catch(DbEntityValidationException dbEx)
                {
                    Assert.IsTrue(false);
                }
            }
        }
    }
}
