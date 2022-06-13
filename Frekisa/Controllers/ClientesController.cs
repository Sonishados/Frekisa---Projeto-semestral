﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Frekisa.Models;

namespace Frekisa.Controllers
{
    public class ClientesController : Controller
    {
        private dbFrekisaEntities db = new dbFrekisaEntities();

        public ActionResult Planos(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clientes cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }

            var renda = Convert.ToDouble(cliente.renda_mensal);

            analiseCredito analise = new analiseCredito();
            var plano = analise.ScoreFrekisa(renda);

            //Sessão para guardar o valor do id do cliente
            Session["Cliente"] = cliente.id_cliente;

            ViewData["nome"] = cliente.nome_cliente;
            ViewData["renda"] = renda;
            ViewData["plano"] = plano;

            return View();
        }

        [HttpPost]

        //Criação do contrato com a seleção do plano!!
        public ActionResult Planos(string botaoPlano,string plano, string parcelas, string valorParcela, string valor, string plano2, string parcelas2, string valorParcela2, string valor2, string plano3, string parcelas3, string valorParcela3, string valor3)
        {
            Contrato contrato = new Contrato();

            Clientes cliente = db.Clientes.Find(Convert.ToInt32(Session["Cliente"]));
            Funcionarios funcionario = db.Funcionarios.Find(Convert.ToInt32(Session["ID"]));

            contrato.id_cliente = cliente.id_cliente;
            contrato.id_funcionario = funcionario.id_funcionario;

            if(botaoPlano == "1")
            {
                contrato.Plano = plano.Trim();
                contrato.parcelas = parcelas.Trim();
                contrato.valor_parcelas = valorParcela.Trim();
                contrato.valor_recebido = valor.Trim();
            }
            else if(botaoPlano == "2")
            {
                contrato.Plano = plano2.Trim();
                contrato.parcelas = parcelas2.Trim();
                contrato.valor_parcelas = valorParcela2.Trim();
                contrato.valor_recebido = valor2.Trim();
            }
            else if(botaoPlano == "3")
            {
                contrato.Plano = plano3.Trim();
                contrato.parcelas = parcelas3.Trim();
                contrato.valor_parcelas = valorParcela3.Trim();
                contrato.valor_recebido = valor3.Trim();
            }

            db.Contrato.Add(contrato);
            db.SaveChanges();
            TempData["confirm"] = "Contrato criado com sucesso!";
            return RedirectToAction("Index","Contrato");
        }
        // GET: Clientes
        public ActionResult Index()
        {
            if(Session.Count != 0)
            {
                return View(db.Clientes.ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Clientes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clientes clientes = db.Clientes.Find(id);
            if (clientes == null)
            {
                return HttpNotFound();
            }
            return View(clientes);
        }

        // GET: Clientes/Create
        public ActionResult Create()
        {
            if (Session.Count != 0)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Clientes/Create
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Create(string nome, string cpf, string telefone, string email, string renda, string endereco)
        {
            Clientes cliente = new Clientes();

            cliente.nome_cliente = nome.Trim();
            cliente.cpf_cliente = cpf.Trim();
            cliente.telefone_cliente = telefone.Trim();
            cliente.email_cliente = email.Trim();
            cliente.renda_mensal = Convert.ToDecimal(renda.Trim());
            cliente.endereco = endereco.Trim();
            db.Clientes.Add(cliente);
            db.SaveChanges();
            TempData["confirm"] = "Cliente cadastrado com sucesso!";
            return RedirectToAction("Index");
            
        }

        // GET: Clientes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clientes clientes = db.Clientes.Find(id);
            if (clientes == null)
            {
                return HttpNotFound();
            }
            return View(clientes);
        }

        // POST: Clientes/Edit/5
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_cliente,nome_cliente,cpf_cliente,telefone_cliente,email_cliente,renda_mensal,endereco")] Clientes clientes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(clientes).State = EntityState.Modified;
                db.SaveChanges();
                TempData["confirm"] = "Cliente editado com sucesso!";
                return RedirectToAction("Index");
            }
            return View(clientes);
        }

        // GET: Clientes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clientes clientes = db.Clientes.Find(id);
            if (clientes == null)
            {
                return HttpNotFound();
            }
            return View(clientes);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Clientes clientes = db.Clientes.Find(id);
                db.Clientes.Remove(clientes);
                db.SaveChanges();
                TempData["confirm"] = "Cliente excluído com sucesso!";
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["erroDelete"] = "Cliente possui contrato ativo!";
                return RedirectToAction("Index");
            }
            
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
