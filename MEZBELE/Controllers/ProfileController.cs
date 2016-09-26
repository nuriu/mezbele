﻿using System.Data.Entity;
using System.Net;
using System.Web.Mvc;
using MEZBELE.Context;
using MEZBELE.Models;

namespace MEZBELE.Controllers
{
    public class ProfileController : Controller
    {
        /// <summary>
        /// Veritabanı.
        /// </summary>
        private MezbeleContext db = new MezbeleContext();

        /// <summary>
        /// Giriş yapmış kullanıcın profil bilgilerini gösterir.
        /// </summary>
        /// <returns>Kullanıcı bilgilerini içeren görünümü döndürür.</returns>
        public ActionResult Index()
        {
            if (Session["UserId"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users user = db.Users.Find(Session["UserId"]);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        /// <summary>
        /// Kullanıcı bilgilerinin düzenlendiği görünüm.
        /// </summary>
        /// <returns>Edit görünümüne yönlendirir.</returns>
        public ActionResult Edit()
        {
            if (Session["UserId"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users user = db.Users.Find(Session["UserId"]);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        /// <summary>
        /// Kullanıcı bilgilerini değiştirir ve veritabanına kaydeder.
        /// </summary>
        /// <param name="user">Yeni kullanıcı bilgileri.</param>
        /// <returns>İşlem başarılıysa Index görünümüne yönlendirir.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserName,Password,FirstName,LastName,EMail,UserAvatar")] Users user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        /// <summary>
        /// Kullanıcıyı sistemden silme işlemini kontrol eder.
        /// </summary>
        /// <returns>Index görünümüne yönlendirir.</returns>
        public ActionResult Delete()
        {
            if (Session["UserId"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users user = db.Users.Find(Session["UserId"]);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        /// <summary>
        /// Silme işlemini onaylar.
        /// </summary>
        /// <returns>Index görünümüne yönlendirir.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed()
        {
            Users user = db.Users.Find(Session["UserId"]);
            db.Users.Remove(user);
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