using ControleEstacionamento.DAL.Repositorios;
using ControleEstacionamento.Entidades;
using ControleEstacionamento.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ControleEstacionamento.Controllers
{
    public class MovimentacaoVeiculoController : Controller
    {
        private TabelaPrecosModelo tabePrecMode = new TabelaPrecosModelo();
        private TabelaPrecoRepositorio tabePrecRepo = new TabelaPrecoRepositorio();

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
        public ActionResult Registro(string numeroPlaca)
        {
            MovimentacaoVeiculoModelo moviVeicMode = new MovimentacaoVeiculoModelo();
            MovimentacaoVeiculo moviVeic = moviVeicMode.BuscarMovimentacaoVeiculoEmAberto(numeroPlaca);
            TabelaPrecosModelo tabePrecMode = new TabelaPrecosModelo();
            ViewBag.FaltaConfiguracao = false;

            DateTime dataHoraAtual = DateTime.Now;

            if (moviVeic == null)
                moviVeic = new MovimentacaoVeiculo() { placa = numeroPlaca, entrada = dataHoraAtual, idTabelaPreco = tabePrecMode.BuscarIdTabelaPreco(dataHoraAtual) };
            else if (moviVeic.idTabelaPreco <= 0)
                moviVeic.idTabelaPreco = tabePrecMode.BuscarIdTabelaPreco(moviVeic.entrada);

            if (moviVeic.idTabelaPreco <= 0)
            {
                ModelState.AddModelError("", string.Format("Não foi possível determinar uma tabela de preços para a movimentação. Data e Hora da movimentação {0}", moviVeic.entrada));
                ViewBag.FaltaConfiguracao = true;
            }
            else
                moviVeic.tabelaPreco = tabePrecRepo.Buscar(tabe => tabe.handle == moviVeic.idTabelaPreco).FirstOrDefault();

            return PartialView(moviVeic);
        }
    }
}