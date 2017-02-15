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
    public class usersController : Controller
    {
        private bd_siugEntities db = new bd_siugEntities();

        // GET: users
        public ActionResult Index()
        {
            var tb_users = db.tb_users.Include(t => t.tb_state);
            return View(tb_users.ToList());
        }

        // GET: users/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_users tb_users = db.tb_users.Find(id);
            if (tb_users == null)
            {
                return HttpNotFound();
            }
            return View(tb_users);
        }

        // GET: users/Create
        public ActionResult Create()
        {
            ViewBag.us_state = new SelectList(db.tb_state, "sta_id", "sta_name");
            return View();
        }

        // POST: users/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "us_id,us_name,us_lastname,us_phone,us_email,us_pass,us_cityId,us_createDate,us_state")] tb_users tb_users)
        {
            if (ModelState.IsValid)
            {
                db.tb_users.Add(tb_users);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.us_state = new SelectList(db.tb_state, "sta_id", "sta_name", tb_users.us_state);
            return View(tb_users);
        }

        // GET: users/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_users tb_users = db.tb_users.Find(id);
            if (tb_users == null)
            {
                return HttpNotFound();
            }
            ViewBag.us_state = new SelectList(db.tb_state, "sta_id", "sta_name", tb_users.us_state);
            return View(tb_users);
        }

        // POST: users/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "us_id,us_name,us_lastname,us_phone,us_email,us_pass,us_cityId,us_createDate,us_state")] tb_users tb_users)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tb_users).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.us_state = new SelectList(db.tb_state, "sta_id", "sta_name", tb_users.us_state);
            return View(tb_users);
        }

        // GET: users/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_users tb_users = db.tb_users.Find(id);
            if (tb_users == null)
            {
                return HttpNotFound();
            }
            return View(tb_users);
        }

        // POST: users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            tb_users tb_users = db.tb_users.Find(id);
            db.tb_users.Remove(tb_users);
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
