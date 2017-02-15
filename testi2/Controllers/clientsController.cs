using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using testi2.Context;

namespace testi2.Controllers
{
    public class clientsController : Controller
    {
        private bd_siugEntities db = new bd_siugEntities();

        // GET: clients
        public ActionResult Index()
        {
            var tb_clients = db.tb_clients.Include(t => t.tb_state);
            return View(tb_clients.ToList());
        }

        // GET: clients/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_clients tb_clients = db.tb_clients.Find(id);
            if (tb_clients == null)
            {
                return HttpNotFound();
            }
            return View(tb_clients);
        }

        // GET: clients/Create
        public ActionResult Create()
        {
            ViewBag.cli_state = new SelectList(db.tb_state, "sta_id", "sta_name");
            return View();
        }

        // POST: clients/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "cli_id,cli_nit,cli_name_company,cli_name_contact,cli_last_name_contact,cli_phone,cli_cel_phone,cli_email,cli_date,cli_state")] tb_clients tb_clients)
        {
            if (ModelState.IsValid)
            {
                db.tb_clients.Add(tb_clients);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.cli_state = new SelectList(db.tb_state, "sta_id", "sta_name", tb_clients.cli_state);
            return View(tb_clients);
        }

        // GET: clients/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_clients tb_clients = db.tb_clients.Find(id);
            if (tb_clients == null)
            {
                return HttpNotFound();
            }
            ViewBag.cli_state = new SelectList(db.tb_state, "sta_id", "sta_name", tb_clients.cli_state);
            return View(tb_clients);
        }

        // POST: clients/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "cli_id,cli_nit,cli_name_company,cli_name_contact,cli_last_name_contact,cli_phone,cli_cel_phone,cli_email,cli_date,cli_state")] tb_clients tb_clients)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tb_clients).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cli_state = new SelectList(db.tb_state, "sta_id", "sta_name", tb_clients.cli_state);
            return View(tb_clients);
        }

        // GET: clients/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_clients tb_clients = db.tb_clients.Find(id);
            if (tb_clients == null)
            {
                return HttpNotFound();
            }
            return View(tb_clients);
        }

        // POST: clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            tb_clients tb_clients = db.tb_clients.Find(id);
            db.tb_clients.Remove(tb_clients);
            db.SaveChanges();
            return RedirectToAction("Index");
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
