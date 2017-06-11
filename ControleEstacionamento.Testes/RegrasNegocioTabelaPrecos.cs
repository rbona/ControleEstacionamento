using ControleEstacionamento.DAL.Repositorios;
using ControleEstacionamento.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ControleEstacionamento.Testes
{
    [TestClass]
    public class RegrasNegocioTabelaPrecos
    {
        private TabelaPrecosModelo _tabePrecMode = new TabelaPrecosModelo();
        private TabelaPrecoRepositorio _tabePrecRepo = new TabelaPrecoRepositorio();

        [TestMethod]
        public void ExisteConfiguracao()
        {
            this.ZerarTabelaPrecos();

            _tabePrecRepo.Adicionar(new Entidades.TabelaPreco()
            {
                descricao = "Teste Tabela Existe",
                inicioVigencia = DateTime.Now.AddDays(-1).Date,
                finalVigencia = DateTime.Now.AddDays(1).Date,
                valorHoraAdicional = 1,
                valorHoraInicial = 1
            });
            _tabePrecRepo.Salvar();

            Assert.IsTrue(string.IsNullOrEmpty(_tabePrecMode.VericarExisteConfiguracoes()), _tabePrecMode.VericarExisteConfiguracoes());

            _tabePrecRepo.Excluir(tab => true);
            _tabePrecRepo.Salvar();
            _tabePrecRepo.Adicionar(new Entidades.TabelaPreco()
            {
                descricao = "Teste Tabela Existe",
                inicioVigencia = DateTime.Now.Date,
                finalVigencia = DateTime.Now.Date,
                valorHoraAdicional = 1,
                valorHoraInicial = 1
            });
            _tabePrecRepo.Salvar();

            Assert.IsTrue(string.IsNullOrEmpty(_tabePrecMode.VericarExisteConfiguracoes()), _tabePrecMode.VericarExisteConfiguracoes());
        }

        [TestMethod]
        public void NaoExisteConfiguracao()
        {
            ZerarTabelaPrecos();

            _tabePrecRepo.Adicionar(new Entidades.TabelaPreco()
            {
                descricao = "Teste Tabela Inexiste",
                inicioVigencia = DateTime.Now.AddDays(-1).Date,
                finalVigencia = DateTime.Now.AddDays(-1).Date,
                valorHoraAdicional = 1,
                valorHoraInicial = 1
            });
            _tabePrecRepo.Salvar();

            Assert.IsFalse(string.IsNullOrEmpty(_tabePrecMode.VericarExisteConfiguracoes()), "Não deveria estar encontrando tabela de preços.");

            ZerarTabelaPrecos();

            _tabePrecRepo.Adicionar(new Entidades.TabelaPreco()
            {
                descricao = "Teste Tabela Inexiste",
                inicioVigencia = DateTime.Now.AddDays(1).Date,
                finalVigencia = DateTime.Now.AddDays(1).Date,
                valorHoraAdicional = 1,
                valorHoraInicial = 1
            });
            _tabePrecRepo.Salvar();

            Assert.IsFalse(string.IsNullOrEmpty(_tabePrecMode.VericarExisteConfiguracoes()), "Não deveria estar encontrando tabela de preços.");
        }

        [TestMethod]
        public void BuscarIdTabelaPelaDataExistente()
        {
            ZerarTabelaPrecos();

            _tabePrecRepo.Adicionar(new Entidades.TabelaPreco()
            {
                descricao = "Teste Busca Tabela",
                inicioVigencia = new DateTime(DateTime.Now.Year, 1, 1),
                finalVigencia = new DateTime(DateTime.Now.Year, 12, 31),
                valorHoraAdicional = 1,
                valorHoraInicial = 1,
            });
            _tabePrecRepo.Salvar();

            Assert.IsTrue(_tabePrecMode.BuscarIdTabelaPreco(DateTime.Now) > 0, "Não foi encontrado a tabela de preços");
        }

        [TestMethod]
        public void BuscarIdTabelaPelaDataInexistente()
        {
            ZerarTabelaPrecos();

            Assert.IsFalse(_tabePrecMode.BuscarIdTabelaPreco(new DateTime(DateTime.Now.Year + 1, 12, 31)) > 0, "Não foi encontrado a tabela de preços");
        }

        private void ZerarTabelaPrecos()
        {
            _tabePrecRepo.Excluir(tab => true);
            _tabePrecRepo.Salvar();
        }
    }
}
