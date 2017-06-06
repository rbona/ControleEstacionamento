using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ControleEstacionamento.DAL.Contexto;
using ControleEstacionamento.Entidades;
using ControleEstacionamento.DAL.Repositorios;

namespace ControleEstacionamento.Controllers
{
    public class TabelaPrecoController : Controller
    {
        private readonly TabelaPrecoRepositorio tabePrecRepo = new TabelaPrecoRepositorio();

        // GET: TabelaPreco
        public ActionResult Index()
        {
            return View(tabePrecRepo.BuscarTodos().OrderBy(item=> item.inicioVigencia).ToList());
        }

        // GET: TabelaPreco/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TabelaPreco tabelaPreco = tabePrecRepo.BuscarChave(id);
            if (tabelaPreco == null)
            {
                return HttpNotFound();
            }
            return View(tabelaPreco);
        }

        // GET: TabelaPreco/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TabelaPreco/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "handle,descricao,inicioVigencia,finalVigencia,valorHoraInicial,valorHoraAdicional")] TabelaPreco tabelaPreco)
        {
            if (ModelState.IsValid)
            {
                tabePrecRepo.Adicionar(tabelaPreco);
                tabePrecRepo.Salvar();
                return RedirectToAction("Index");
            }

            return View(tabelaPreco);
        }

        // GET: TabelaPreco/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TabelaPreco tabelaPreco = tabePrecRepo.BuscarChave(id);
            if (tabelaPreco == null)
            {
                return HttpNotFound();
            }
            return View(tabelaPreco);
        }

        // POST: TabelaPreco/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "handle,descricao,inicioVigencia,finalVigencia,valorHoraInicial,valorHoraAdicional")] TabelaPreco tabelaPreco)
        {
            if (ModelState.IsValid)
            {
                tabePrecRepo.Atualizar(tabelaPreco);
                tabePrecRepo.Salvar();
                return RedirectToAction("Index");
            }
            return View(tabelaPreco);
        }

        // GET: TabelaPreco/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TabelaPreco tabelaPreco = tabePrecRepo.BuscarChave(id);
            if (tabelaPreco == null)
            {
                return HttpNotFound();
            }
            return View(tabelaPreco);
        }

        // POST: TabelaPreco/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            tabePrecRepo.Excluir(item => item.handle == id);
            tabePrecRepo.Salvar();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                tabePrecRepo.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
