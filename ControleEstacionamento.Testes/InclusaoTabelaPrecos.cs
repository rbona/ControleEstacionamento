using ControleEstacionamento.DAL.Repositorios;
using ControleEstacionamento.Entidades;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Entity.Validation;
using System.Linq;

namespace ControleEstacionamento.Testes
{
    [TestClass]
    public class InclusaoTabelaPrecos
    {
        [TestMethod]
        public void CamposObrigatoriosEVazios()
        {
            using (TabelaPrecoRepositorio tabPreRep = new TabelaPrecoRepositorio())
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
                catch (DbEntityValidationException dbEx)
                {
                    string erros = string.Join("\n", dbEx.EntityValidationErrors.Where(item => !item.IsValid).Select(item => string.Join("\n", item.ValidationErrors.Select(erro => erro.ErrorMessage).ToArray<string>())).ToArray<string>());
                    Assert.IsTrue(false, erros);
                }
            }
        }

        [TestMethod]
        public void DataInicialMenorDataFinal()
        {
            using (TabelaPrecoRepositorio tabPreRep = new TabelaPrecoRepositorio())
            {

                TabelaPreco tabela = new TabelaPreco();
                try
                {
                    tabela.descricao = "Teste";
                    tabela.inicioVigencia = new DateTime(2017, 06, 1);
                    tabela.finalVigencia = new DateTime(2017, 06, 10);
                    tabela.valorHoraInicial = 2;
                    tabela.valorHoraAdicional = 1;
                    tabPreRep.Adicionar(tabela);
                    tabPreRep.Salvar();

                    Assert.IsTrue(true);
                }
                catch (DbEntityValidationException dbEx)
                {
                    string erros = string.Join("\n", dbEx.EntityValidationErrors.Where(item => !item.IsValid).Select(item => string.Join("\n", item.ValidationErrors.Select(erro => erro.ErrorMessage).ToArray<string>())).ToArray<string>());
                    Assert.IsTrue(false, erros);
                }
            }
        }

        [TestMethod]
        public void DataInicialMaiorDataFinal()
        {
            using (TabelaPrecoRepositorio tabPreRep = new TabelaPrecoRepositorio())
            {

                TabelaPreco tabela = new TabelaPreco();
                try
                {
                    tabela.descricao = "Teste";
                    tabela.inicioVigencia = new DateTime(2017, 06, 10);
                    tabela.finalVigencia = new DateTime(2017, 06, 1);
                    tabela.valorHoraInicial = 2;
                    tabela.valorHoraAdicional = 1;
                    tabPreRep.Adicionar(tabela);
                    tabPreRep.Salvar();

                    Assert.IsTrue(false);
                }
                catch (DbEntityValidationException dbEx)
                {
                    Assert.IsTrue(true);
                }
            }
        }

        [TestMethod]
        public void DataInicialIgualDataFinal()
        {
            using (TabelaPrecoRepositorio tabPreRep = new TabelaPrecoRepositorio())
            {

                TabelaPreco tabela = new TabelaPreco();
                try
                {
                    tabela.descricao = "Teste";
                    tabela.inicioVigencia = new DateTime(2017, 06, 10);
                    tabela.finalVigencia = new DateTime(2017, 06, 10);
                    tabela.valorHoraInicial = 2;
                    tabela.valorHoraAdicional = 1;
                    tabPreRep.Adicionar(tabela);
                    tabPreRep.Salvar();

                    Assert.IsTrue(true);
                }
                catch (DbEntityValidationException dbEx)
                {
                    string erros = string.Join("\n", dbEx.EntityValidationErrors.Where(item => !item.IsValid).Select(item => string.Join("\n", item.ValidationErrors.Select(erro => erro.ErrorMessage).ToArray<string>())).ToArray<string>());
                    Assert.IsTrue(false, erros);
                }
            }
        }

        [TestMethod]
        public void HoraInicialMaiorQueZero()
        {
            using (TabelaPrecoRepositorio tabPreRep = new TabelaPrecoRepositorio())
            {
                TabelaPreco tabela = new TabelaPreco();
                try
                {
                    tabela.descricao = "Teste";
                    tabela.inicioVigencia = new DateTime(2017, 06, 10);
                    tabela.finalVigencia = new DateTime(2017, 06, 10);
                    tabela.valorHoraInicial = 2;
                    tabela.valorHoraAdicional = 1;
                    tabPreRep.Adicionar(tabela);
                    tabPreRep.Salvar();

                    Assert.IsTrue(true);
                }
                catch (DbEntityValidationException dbEx)
                {
                    string erros = string.Join("\n", dbEx.EntityValidationErrors.Where(item => !item.IsValid).Select(item => string.Join("\n", item.ValidationErrors.Select(erro => erro.ErrorMessage).ToArray<string>())).ToArray<string>());
                    Assert.IsTrue(false, erros);
                }
            }
        }

        [TestMethod]
        public void HoraInicialMenorZero()
        {
            using (TabelaPrecoRepositorio tabPreRep = new TabelaPrecoRepositorio())
            {
                TabelaPreco tabela = new TabelaPreco();
                try
                {
                    tabela.descricao = "Teste";
                    tabela.inicioVigencia = new DateTime(2017, 06, 10);
                    tabela.finalVigencia = new DateTime(2017, 06, 10);
                    tabela.valorHoraInicial = -1;
                    tabela.valorHoraAdicional = 1;
                    tabPreRep.Adicionar(tabela);
                    tabPreRep.Salvar();

                    Assert.IsTrue(false,"Foi gravado um valor negativo para a hora inicial");
                }
                catch (DbEntityValidationException dbEx)
                {
                    Assert.IsTrue(true);
                }
            }
        }

        [TestMethod]
        public void HoraInicialIgualZero()
        {
            using (TabelaPrecoRepositorio tabPreRep = new TabelaPrecoRepositorio())
            {
                TabelaPreco tabela = new TabelaPreco();
                try
                {
                    tabela.descricao = "Teste";
                    tabela.inicioVigencia = new DateTime(2017, 06, 10);
                    tabela.finalVigencia = new DateTime(2017, 06, 10);
                    tabela.valorHoraInicial = 0;
                    tabela.valorHoraAdicional = 1;
                    tabPreRep.Adicionar(tabela);
                    tabPreRep.Salvar();

                    Assert.IsTrue(false, "Foi gravado um valor zero para a hora inicial");
                }
                catch (DbEntityValidationException dbEx)
                {
                    Assert.IsTrue(true);
                }
            }
        }
    }
}
