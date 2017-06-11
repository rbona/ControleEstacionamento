using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControleEstacionamento.DAL.Repositorios;
using ControleEstacionamento.Models;
using System.Data.Entity.Validation;
using System.Linq;

namespace ControleEstacionamento.Testes
{
    [TestClass]
    public class RegrasMovimentacaoVeiculos
    {
        [TestMethod]
        public void PlacaVeiculoForaDoPadrao()
        {
            MovimentacaoVeiculoRepositorio moviVeicRepo = new MovimentacaoVeiculoRepositorio();
            string placaTeste = "AAA12345";

            try
            {
                moviVeicRepo.Adicionar(new Entidades.MovimentacaoVeiculo() { placa = placaTeste, entrada = DateTime.Now });
                moviVeicRepo.Salvar();
                Assert.IsTrue(false, "Quantidade de números da placa é maior que o padrão");
            }
            catch (DbEntityValidationException dbEx)
            {
                Assert.IsTrue(true);
            }

            placaTeste = "AAA123";
            try
            {
                moviVeicRepo.Adicionar(new Entidades.MovimentacaoVeiculo() { placa = placaTeste, entrada = DateTime.Now });
                moviVeicRepo.Salvar();
                Assert.IsTrue(false, "Quantidade de números da placa é menor que o padrão");
            }
            catch (DbEntityValidationException dbEx)
            {
                Assert.IsTrue(true);
            }

            placaTeste = "AA1234";
            try
            {
                moviVeicRepo.Adicionar(new Entidades.MovimentacaoVeiculo() { placa = placaTeste, entrada = DateTime.Now });
                moviVeicRepo.Salvar();
                Assert.IsTrue(false, "Quantidade de letras da placa é menor que o padrão");
            }
            catch (DbEntityValidationException dbEx)
            {
                Assert.IsTrue(true);
            }

            placaTeste = "AAAA1234";
            try
            {
                moviVeicRepo.Adicionar(new Entidades.MovimentacaoVeiculo() { placa = placaTeste, entrada = DateTime.Now });
                moviVeicRepo.Salvar();
                Assert.IsTrue(false, "Quantidade de letras da placa é maior que o padrão");
            }
            catch (DbEntityValidationException dbEx)
            {
                Assert.IsTrue(true);
            }

            placaTeste = "A_A1234";
            try
            {
                moviVeicRepo.Adicionar(new Entidades.MovimentacaoVeiculo() { placa = placaTeste, entrada = DateTime.Now });
                moviVeicRepo.Salvar();
                Assert.IsTrue(false, "Caracter inválido");
            }
            catch (DbEntityValidationException dbEx)
            {
                Assert.IsTrue(true);
            }

            placaTeste = "AAA12_4";
            try
            {
                moviVeicRepo.Adicionar(new Entidades.MovimentacaoVeiculo() { placa = placaTeste, entrada = DateTime.Now });
                moviVeicRepo.Salvar();
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
            MovimentacaoVeiculoRepositorio moviVeicRepo = new MovimentacaoVeiculoRepositorio();
            string placaTeste = "AAA1234";

            try
            {
                moviVeicRepo.Adicionar(new Entidades.MovimentacaoVeiculo() { placa = placaTeste, entrada = DateTime.Now });
                moviVeicRepo.Salvar();
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
            MovimentacaoVeiculoRepositorio moviVeicRepo = new MovimentacaoVeiculoRepositorio();
            MovimentacaoVeiculoModelo moviVeicMode = new MovimentacaoVeiculoModelo();
            string placaTeste = "AAA12345";

            moviVeicRepo.Excluir(movi => movi.placa.Equals(placaTeste));
            moviVeicRepo.Salvar();

            Assert.IsNull( moviVeicMode.BuscarMovimentacaoVeiculoEmAberto(placaTeste), "Não deveria ter retornado movimentação para a placa.");
        }

        [TestMethod]
        public void PlacaComMovimentacaoEmAberto()
        {
            MovimentacaoVeiculoRepositorio moviVeicRepo = new MovimentacaoVeiculoRepositorio();
            MovimentacaoVeiculoModelo moviVeicMode = new MovimentacaoVeiculoModelo();
            string placaTeste = "AAA1234";

            moviVeicRepo.Excluir(movi => movi.placa.Equals(placaTeste));
            moviVeicRepo.Salvar();

            moviVeicRepo.Adicionar(new Entidades.MovimentacaoVeiculo() { placa = placaTeste, entrada = DateTime.Now });
            moviVeicRepo.Salvar();

            Assert.IsNotNull(moviVeicMode.BuscarMovimentacaoVeiculoEmAberto(placaTeste), "Deveria ter retornado movimentação para a placa.");
        }
    }
}
