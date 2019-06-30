using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAppSportsLeagueTestTask.WEB.Models;

namespace WebAppSportsLeagueTestTask.WEB.Controllers
{
    public class DirectorsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Directors
        [Authorize]
        public ActionResult Index()
        {
            return View(db.Directors.Select(d => new DirectorViewModel
            {
                Id = d.Id,
                FullName = d.FullName,
                CountMovies = d.Movies.Count

            }).ToList());
           
        }

        // GET: Directors/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
            Director director = db.Directors.Find(id);
            if (director == null)
            {
                return HttpNotFound();
            }

            DirectorViewModel directorVM = new DirectorViewModel
            {
                Id = director.Id,
                FullName = director.FullName,
                CountMovies = director.Movies.Count,
                Movies = db.Movies.Where(m => m.DirectorId == director.Id).ToList()
                .Select(m => { m.Description = m.Description.Length > 150 ? m.Description.Substring(0, 150) + "..." : m.Description; return m; })
                .ToList()      
            };
            
            return View(directorVM);
        }

        // GET: Directors/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Directors/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "Id,FullName")] Director director)
        {
            if (ModelState.IsValid)
            {
                director.FullName = director.FullName.Trim();
                if (!string.IsNullOrEmpty(director.FullName))
                {
                    db.Directors.Add(director);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            return View(director);
        }

        // GET: Directors/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Director director = db.Directors.Find(id);
            if (director == null)
            {
                return HttpNotFound();
            }
            return View(director);
        }

        // POST: Directors/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "Id,FullName")] Director director)
        {
            if (ModelState.IsValid)
            {
                db.Entry(director).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(director);
        }

        // GET: Directors/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Director director = db.Directors.Find(id);
            if (director == null)
            {
                return HttpNotFound();
            }
            return View(director);
        }

        // POST: Directors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            Director director = db.Directors.Find(id);
            db.Directors.Remove(director);
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
