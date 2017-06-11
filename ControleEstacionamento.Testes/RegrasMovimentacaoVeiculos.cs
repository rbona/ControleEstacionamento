using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControleEstacionamento.DAL.Repositorios;
using ControleEstacionamento.Models;
using System.Data.Entity.Validation;
using System.Linq;
using ControleEstacionamento.Entidades;

namespace ControleEstacionamento.Testes
{
    [TestClass]
    public class RegrasMovimentacaoVeiculos
    {
        private TabelaPrecoRepositorio _tabePrecRepo = new TabelaPrecoRepositorio();
        private MovimentacaoVeiculoRepositorio _moviVeicRepo = new MovimentacaoVeiculoRepositorio();
        private MovimentacaoVeiculoModelo _moviVeicMode = new MovimentacaoVeiculoModelo();
        private TabelaPrecosModelo _tabePrecMode = new TabelaPrecosModelo();

        [TestMethod]
        public void PlacaVeiculoForaDoPadrao()
        {
            string placaTeste = "AAA12345";

            try
            {
                _moviVeicRepo.Adicionar(new Entidades.MovimentacaoVeiculo() { placa = placaTeste, entrada = DateTime.Now });
                _moviVeicRepo.Salvar();
                Assert.IsTrue(false, "Quantidade de números da placa é maior que o padrão");
            }
            catch (DbEntityValidationException dbEx)
            {
                Assert.IsTrue(true);
            }

            placaTeste = "AAA123";
            try
            {
                _moviVeicRepo.Adicionar(new Entidades.MovimentacaoVeiculo() { placa = placaTeste, entrada = DateTime.Now });
                _moviVeicRepo.Salvar();
                Assert.IsTrue(false, "Quantidade de números da placa é menor que o padrão");
            }
            catch (DbEntityValidationException dbEx)
            {
                Assert.IsTrue(true);
            }

            placaTeste = "AA1234";
            try
            {
                _moviVeicRepo.Adicionar(new Entidades.MovimentacaoVeiculo() { placa = placaTeste, entrada = DateTime.Now });
                _moviVeicRepo.Salvar();
                Assert.IsTrue(false, "Quantidade de letras da placa é menor que o padrão");
            }
            catch (DbEntityValidationException dbEx)
            {
                Assert.IsTrue(true);
            }

            placaTeste = "AAAA1234";
            try
            {
                _moviVeicRepo.Adicionar(new Entidades.MovimentacaoVeiculo() { placa = placaTeste, entrada = DateTime.Now });
                _moviVeicRepo.Salvar();
                Assert.IsTrue(false, "Quantidade de letras da placa é maior que o padrão");
            }
            catch (DbEntityValidationException dbEx)
            {
                Assert.IsTrue(true);
            }

            placaTeste = "A_A1234";
            try
            {
                _moviVeicRepo.Adicionar(new Entidades.MovimentacaoVeiculo() { placa = placaTeste, entrada = DateTime.Now });
                _moviVeicRepo.Salvar();
                Assert.IsTrue(false, "Caracter inválido");
            }
            catch (DbEntityValidationException dbEx)
            {
                Assert.IsTrue(true);
            }

            placaTeste = "AAA12_4";
            try
            {
                _moviVeicRepo.Adicionar(new Entidades.MovimentacaoVeiculo() { placa = placaTeste, entrada = DateTime.Now });
                _moviVeicRepo.Salvar();
                Assert.IsTrue(false, "Caracter inválido");
            }
            catch (DbEntityValidationException dbEx)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void PlacaVeiculoDentroDoPadrao()
        {
            string placaTeste = "AAA1234";

            try
            {
                _moviVeicRepo.Adicionar(new Entidades.MovimentacaoVeiculo() { placa = placaTeste, entrada = DateTime.Now });
                _moviVeicRepo.Salvar();
                Assert.IsTrue(true);
            }
            catch (DbEntityValidationException dbEx)
            {
                string erros = string.Join("\n", dbEx.EntityValidationErrors.Where(item => !item.IsValid).Select(item => string.Join("\n", item.ValidationErrors.Select(erro => erro.ErrorMessage).ToArray<string>())).ToArray<string>());
                Assert.IsTrue(false, erros);
            }
        }


        [TestMethod]
        public void PlacaSemMovimentacaoEmAberto()
        {
            string placaTeste = "AAA12345";

            ZerarMovimentacao();

            Assert.IsNull(_moviVeicMode.BuscarMovimentacaoVeiculoEmAberto(placaTeste), "Não deveria ter retornado movimentação para a placa.");
        }

        [TestMethod]
        public void PlacaComMovimentacaoEmAberto()
        {
            string placaTeste = "AAA1234";

            ZerarMovimentacao();

            _moviVeicRepo.Adicionar(new Entidades.MovimentacaoVeiculo() { placa = placaTeste, entrada = DateTime.Now });
            _moviVeicRepo.Salvar();

            Assert.IsNotNull(_moviVeicMode.BuscarMovimentacaoVeiculoEmAberto(placaTeste), "Deveria ter retornado movimentação para a placa.");
        }

        [TestMethod]
        public void ValidarCalculoValorTotalEstacionamentoMenorIgualTrintaMinutos()
        {
            string placaTeste = "ASD1234";
            DateTime dataHoraAtual = DateTime.Now;

            ZerarMovimentacao();
            ZerarTabelaPrecos();

            _tabePrecRepo.Adicionar(new Entidades.TabelaPreco()
            {
                descricao = "Teste Movimentacao",
                inicioVigencia = new DateTime(DateTime.Now.Year, 01, 01),
                finalVigencia = new DateTime(DateTime.Now.Year, 12, 31),
                valorHoraAdicional = 2,
                valorHoraInicial = 4
            });
            _tabePrecRepo.Salvar();

            _moviVeicRepo.Adicionar(_moviVeicMode.CriarAtualizarMovimentacaoVeiculo(placaTeste, dataHoraAtual));
            _moviVeicRepo.Salvar();

            MovimentacaoVeiculo movimentacao = _moviVeicMode.CriarAtualizarMovimentacaoVeiculo(placaTeste, dataHoraAtual.AddMinutes(29));
            Assert.IsTrue(movimentacao.valorTotal == 2, "Deveria estar cobrando metado do valor da hora inicial pois se passou menos que 30 minutos.");

            movimentacao = _moviVeicMode.CriarAtualizarMovimentacaoVeiculo(placaTeste, dataHoraAtual.AddMinutes(30));
            Assert.IsTrue(movimentacao.valorTotal == 2, "Deveria estar cobrando metado do valor da hora inicial pois se passou menos que 30 minutos.");
        }

        [TestMethod]
        public void ValidarCalculoValorTotalEstacionamentoSemDataSaida()
        {
            string placaTeste = "ASD1234";
            DateTime dataHoraAtual = DateTime.Now;

            ZerarMovimentacao();
            ZerarTabelaPrecos();

            _tabePrecRepo.Adicionar(new Entidades.TabelaPreco()
            {
                descricao = "Teste Movimentacao",
                inicioVigencia = new DateTime(DateTime.Now.Year, 01, 01),
                finalVigencia = new DateTime(DateTime.Now.Year, 12, 31),
                valorHoraAdicional = 2,
                valorHoraInicial = 4
            });
            _tabePrecRepo.Salvar();

            MovimentacaoVeiculo movimentacao = _moviVeicMode.CriarAtualizarMovimentacaoVeiculo(placaTeste, dataHoraAtual);

            Assert.IsTrue(movimentacao.valorTotal == 0, "Deveria estar retornando zero, não tam data de saída para calcular o tempo total e o valor a ser cobrado.");
        }

        [TestMethod]
        public void ValidarCalculoValorTotalEstacionamentoMenor71minutos()
        {
            string placaTeste = "ASD1234";
            DateTime dataHoraAtual = DateTime.Now;

            ZerarMovimentacao();
            ZerarTabelaPrecos();

            _tabePrecRepo.Adicionar(new Entidades.TabelaPreco()
            {
                descricao = "Teste Movimentacao",
                inicioVigencia = new DateTime(DateTime.Now.Year, 01, 01),
                finalVigencia = new DateTime(DateTime.Now.Year, 12, 31),
                valorHoraAdicional = 2,
                valorHoraInicial = 4
            });
            _tabePrecRepo.Salvar();

            _moviVeicRepo.Adicionar(_moviVeicMode.CriarAtualizarMovimentacaoVeiculo(placaTeste, dataHoraAtual));
            _moviVeicRepo.Salvar();

            MovimentacaoVeiculo movimentacao = _moviVeicMode.CriarAtualizarMovimentacaoVeiculo(placaTeste, dataHoraAtual.AddMinutes(70));
            Assert.IsTrue(movimentacao.valorTotal == 4, "Deveria estar cobrando o valor da hora inicial.");
        }

        [TestMethod]
        public void ValidarCalculoValorTotalEstacionamentoHoraAdicional()
        {
            string placaTeste = "ASD1234";
            DateTime dataHoraAtual = DateTime.Now;

            ZerarMovimentacao();
            ZerarTabelaPrecos();

            _tabePrecRepo.Adicionar(new Entidades.TabelaPreco()
            {
                descricao = "Teste Movimentacao",
                inicioVigencia = new DateTime(DateTime.Now.Year, 01, 01),
                finalVigencia = new DateTime(DateTime.Now.Year, 12, 31),
                valorHoraAdicional = 2,
                valorHoraInicial = 4
            });
            _tabePrecRepo.Salvar();

            _moviVeicRepo.Adicionar(_moviVeicMode.CriarAtualizarMovimentacaoVeiculo(placaTeste, dataHoraAtual));
            _moviVeicRepo.Salvar();

            MovimentacaoVeiculo movimentacao = _moviVeicMode.CriarAtualizarMovimentacaoVeiculo(placaTeste, dataHoraAtual.AddMinutes(71));
            Assert.IsTrue(movimentacao.valorTotal == 6, "Deveria estar cobrando o valor da hora inicial.");

            movimentacao = _moviVeicMode.CriarAtualizarMovimentacaoVeiculo(placaTeste, dataHoraAtual.AddMinutes(141));
            Assert.IsTrue(movimentacao.valorTotal == 6, "Deveria estar cobrando o valor da hora inicial.");

            movimentacao = _moviVeicMode.CriarAtualizarMovimentacaoVeiculo(placaTeste, dataHoraAtual.AddMinutes(142));
            Assert.IsTrue(movimentacao.valorTotal == 8, "Deveria estar cobrando o valor da hora inicial.");

            movimentacao = _moviVeicMode.CriarAtualizarMovimentacaoVeiculo(placaTeste, dataHoraAtual.AddMinutes(213));
            Assert.IsTrue(movimentacao.valorTotal == 10, "Deveria estar cobrando o valor da hora inicial.");
        }

        [TestMethod]
        public void IncluirMovimentacaoSemSegundosNaDataHora()
        {
            ZerarMovimentacao();

            DateTime dataHoraAtual = new DateTime();
            string placaTeste = "ASD1234";

            do
            {
                dataHoraAtual = DateTime.Now;
            } while (dataHoraAtual.Second == 0);

            _moviVeicRepo.Adicionar(_moviVeicMode.CriarAtualizarMovimentacaoVeiculo(placaTeste, dataHoraAtual));
            _moviVeicRepo.Salvar();

            Assert.IsTrue(_moviVeicRepo.Buscar(movi => movi.placa.Equals(placaTeste)).FirstOrDefault().entrada.Second == 0);

            _moviVeicRepo = new MovimentacaoVeiculoRepositorio();
            _moviVeicRepo.Atualizar(_moviVeicMode.CriarAtualizarMovimentacaoVeiculo(placaTeste, dataHoraAtual));
            _moviVeicRepo.Salvar();

            Assert.IsTrue(_moviVeicRepo.Buscar(movi => movi.placa.Equals(placaTeste)).FirstOrDefault().saida.Second == 0);
        }

        private void ZerarMovimentacao()
        {
            _moviVeicRepo.Excluir(movi => true);
            _moviVeicRepo.Salvar();
        }

        private void ZerarTabelaPrecos()
        {
            _tabePrecRepo.Excluir(tab => true);
            _tabePrecRepo.Salvar();
        }
    }
}
