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
    public class stockController : Controller
    {
        private bd_siugEntities db = new bd_siugEntities();

        // GET: stock

        public ActionResult Index()
        {

            var tb_stock = db.tb_stock.Include(t => t.tb_category).Include(t => t.tb_provider).Include(t => t.tb_state).ToList();
            //var tb_stock = from x in db.tb_stock.ToList()
            //               orderby x.sto_descript
            //               select new { id = x.sto_id };
            //    select new {desc= x.sto_descript }
            //    );
            //var name = from x in db.tb_stock
            //           //let fullName = x.name + " " + x.surname
            //           //where fullName == "Jean Paul Olvera"
            //           //orderby x.surname
            //           select new {name= x.sto_descript };
            return View(tb_stock);
        }

        // GET: stock/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_stock tb_stock = db.tb_stock.Find(id);
            if (tb_stock == null)
            {
                return HttpNotFound();
            }
            return View(tb_stock);
        }

        // GET: stock/Create
        public ActionResult Create()
        {
            ViewBag.sto_category = new SelectList(db.tb_category, "cat_id", "cat_name");
            ViewBag.sto_provId = new SelectList(db.tb_provider, "pro_id", "pro_name");
            ViewBag.sto_state = new SelectList(db.tb_state, "sta_id", "sta_name");
            return View();
        }

        // POST: stock/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "sto_id,sto_descript,sto_state,sto_avaible,sto_buyPrice,sto_salePrice,sto_url,sto_provId,sto_category")] tb_stock tb_stock)
        {
            if (ModelState.IsValid)
            {
                db.tb_stock.Add(tb_stock);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.sto_category = new SelectList(db.tb_category, "cat_id", "cat_name", tb_stock.sto_category);
            ViewBag.sto_provId = new SelectList(db.tb_provider, "pro_id", "pro_name", tb_stock.sto_provId);
            ViewBag.sto_state = new SelectList(db.tb_state, "sta_id", "sta_name", tb_stock.sto_state);
            return View(tb_stock);
        }

        // GET: stock/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_stock tb_stock = db.tb_stock.Find(id);
            if (tb_stock == null)
            {
                return HttpNotFound();
            }
            ViewBag.sto_category = new SelectList(db.tb_category, "cat_id", "cat_name", tb_stock.sto_category);
            ViewBag.sto_provId = new SelectList(db.tb_provider, "pro_id", "pro_name", tb_stock.sto_provId);
            ViewBag.sto_state = new SelectList(db.tb_state, "sta_id", "sta_name", tb_stock.sto_state);
            return View(tb_stock);
        }

        // POST: stock/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "sto_id,sto_descript,sto_state,sto_avaible,sto_buyPrice,sto_salePrice,sto_url,sto_provId,sto_category,sto_alert")] tb_stock tb_stock)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tb_stock).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.sto_category = new SelectList(db.tb_category, "cat_id", "cat_name", tb_stock.sto_category);
            ViewBag.sto_provId = new SelectList(db.tb_provider, "pro_id", "pro_name", tb_stock.sto_provId);
            ViewBag.sto_state = new SelectList(db.tb_state, "sta_id", "sta_name", tb_stock.sto_state);
            return View(tb_stock);
        }

        // GET: stock/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_stock tb_stock = db.tb_stock.Find(id);
            if (tb_stock == null)
            {
                return HttpNotFound();
            }
            return View(tb_stock);
        }

        // POST: stock/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            tb_stock tb_stock = db.tb_stock.Find(id);
            db.tb_stock.Remove(tb_stock);
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
