using ControleEstacionamento.DAL.Repositorios;
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

        // GET: MovimentacaoVeiculo
        public ActionResult Index()
        {
            string mensagemErro = tabePrecMode.VericarExisteConfiguracoes();

            if (!string.IsNullOrEmpty(mensagemErro))
                ModelState.AddModelError("", mensagemErro);

            return View();
        }
    }
}