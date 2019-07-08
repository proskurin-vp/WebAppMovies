using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAppSportsLeagueTestTask.WEB.EFModels;
using WebAppSportsLeagueTestTask.WEB.Models;


namespace WebAppSportsLeagueTestTask.WEB.Controllers
{
    public class DirectorsController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        private int _pageSize = 4; // количество режиссёров на страницу  
        private const int MAX_PAGINATION_LINKS = 20;

        // GET: Directors
        [Authorize]
        public ActionResult Index(int? page = 1)
        {
            page = page ?? 1;
      
            var directors = _db.Directors
                .OrderBy(d => d.Id)
                .Skip(((int)page - 1) * _pageSize)
                .Take(_pageSize)
                .Select(d => new DirectorViewModel
                {
                    Id = d.Id,
                    FullName = d.FullName,
                    CountMovies = d.Movies.Count
                })
                .ToList();

            int totalDirectorCount = _db.Directors.Count();

            DirectorsViewModel model = new DirectorsViewModel
            {
                PagedDirectors = new StaticPagedList<DirectorViewModel>(directors, (int)page, _pageSize,  totalDirectorCount < MAX_PAGINATION_LINKS ?
                totalDirectorCount : MAX_PAGINATION_LINKS),
                ApplicationUser =  Utils.GetCurrentAppUser()
            };

            return View(model);
        }

        // GET: Directors/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
            Director director = _db.Directors.Find(id);
            if (director == null)
            {
                return HttpNotFound();
            }

            DirectorViewModel directorVM = new DirectorViewModel
            {
                Id = director.Id,
                FullName = director.FullName,
                CountMovies = director.Movies.Count,
                Movies = _db.Movies.Where(m => m.DirectorId == director.Id).ToList()
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
                    _db.Directors.Add(director);
                    _db.SaveChanges();
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
            Director director = _db.Directors.Find(id);
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
                _db.Entry(director).State = EntityState.Modified;
                _db.SaveChanges();
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
            Director director = _db.Directors.Find(id);
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
            Director director = _db.Directors.Find(id);
            _db.Directors.Remove(director);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
