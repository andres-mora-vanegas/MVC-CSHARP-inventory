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
            var tb_users = db.tb_users.Include(u => u.tb_state);
            return View(tb_users.ToList());
        }

        // GET: users/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            users users = db.tb_users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // GET: users/Create
        public ActionResult Create()
        {
            ViewBag.userState = new SelectList(db.tb_state, "sta_id", "sta_name");
            return View();
        }

        // POST: users/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "userId,userName,userLastNamel,userPhone,userEmail,userPass,userCityId,userCreateDate,userState")] users users)
        {
            if (ModelState.IsValid)
            {
                db.tb_users.Add(users);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.userState = new SelectList(db.tb_state, "sta_id", "sta_name", users.userState);
            return View(users);
        }

        // GET: users/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            users users = db.tb_users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            ViewBag.userState = new SelectList(db.tb_state, "sta_id", "sta_name", users.userState);
            return View(users);
        }

        // POST: users/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "userId,userName,userLastNamel,userPhone,userEmail,userPass,userCityId,userCreateDate,userState")] users users)
        {
            if (ModelState.IsValid)
            {
                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.userState = new SelectList(db.tb_state, "sta_id", "sta_name", users.userState);
            return View(users);
        }

        // GET: users/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            users users = db.tb_users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            users users = db.tb_users.Find(id);
            db.tb_users.Remove(users);
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
