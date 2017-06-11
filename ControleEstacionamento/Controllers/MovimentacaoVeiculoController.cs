using ControleEstacionamento.DAL.Contexto;
using ControleEstacionamento.DAL.Repositorios;
using ControleEstacionamento.Entidades;
using ControleEstacionamento.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ControleEstacionamento.Controllers
{
    public class MovimentacaoVeiculoController : Controller
    {
        private TabelaPrecosModelo tabePrecMode = new TabelaPrecosModelo();
        private TabelaPrecoRepositorio tabePrecRepo = new TabelaPrecoRepositorio();
        private MovimentacaoVeiculoRepositorio moviVeicRepo = new MovimentacaoVeiculoRepositorio();
        private Banco db = new Banco();

        [HttpGet]
        // GET: MovimentacaoVeiculo
        public ActionResult Index()
        {
            string mensagemErro = tabePrecMode.VericarExisteConfiguracoes();
            ViewBag.FaltaConfiguracao = false;

            if (!string.IsNullOrEmpty(mensagemErro))
            {
                ModelState.AddModelError("", mensagemErro);
                ViewBag.FaltaConfiguracao = !string.IsNullOrEmpty(mensagemErro);
            }


            return View();
        }

        [HttpGet]
        public ActionResult BuscarVeiculosNoPatio()
        {
            List<MovimentacaoVeiculo> veiculosNoPatio = moviVeicRepo.BuscarTodos().Include(movi=>movi.tabelaPreco).Where(movi => DateTime.Compare(movi.saida, new DateTime()) == 0).OrderBy(movi => movi.entrada).ToList();
            return PartialView("VeiculosNoPatio", veiculosNoPatio);
        }

        [HttpGet]
        public ActionResult Registro(string numeroPlaca)
        {
            MovimentacaoVeiculoModelo moviVeicMode = new MovimentacaoVeiculoModelo();
            TabelaPrecosModelo tabePrecMode = new TabelaPrecosModelo();
            ViewBag.FaltaConfiguracao = false;

            DateTime dataHoraAtual = DateTime.Now;

            MovimentacaoVeiculo moviVeic = moviVeicMode.CriarAtualizarMovimentacaoVeiculo(numeroPlaca, dataHoraAtual);// BuscarMovimentacaoVeiculoEmAberto(numeroPlaca);

            if (moviVeic.idTabelaPreco <= 0)
            {
                ModelState.AddModelError("", string.Format("Não foi possível determinar uma tabela de preços para a movimentação. Data e Hora da movimentação {0}", moviVeic.entrada));
                ViewBag.FaltaConfiguracao = true;
            }

            return PartialView(moviVeic);
        }

        [HttpPost]
        public ActionResult Gravar(MovimentacaoVeiculo moviVeic)
        {
            if (moviVeic.handle > 0)
                moviVeicRepo.Atualizar(moviVeic);
            else
                moviVeicRepo.Adicionar(moviVeic);

            moviVeicRepo.Salvar();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                tabePrecRepo.Dispose();
                moviVeicRepo.Dispose();
                db.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}