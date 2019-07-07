using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAppSportsLeagueTestTask.WEB.Models;
using PagedList.Mvc;
using PagedList;
using System.Data.SqlClient;

namespace WebAppSportsLeagueTestTask.WEB.Controllers
{
    public class MoviesController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        private int _pageSize = 4; // количество фильмов на страницу  
        private const int MAX_DESCRIPTION_COUNT = 120;
        private const int MAX_PAGINATION_LINKS = 20;

        // GET: Movies
        public ActionResult Index(int? page = 1)
        {
            page = page ?? 1;    

            var movies = _db.Movies
                .OrderBy(m => m.Id)
                .Skip(((int)page - 1) * _pageSize)
                .Take(_pageSize)
                .Include(d => d.Director).ToList();

            movies.ForEach(m =>
            {
                m.Description = m.Description.Length > MAX_DESCRIPTION_COUNT ?
                m.Description.Substring(0, MAX_DESCRIPTION_COUNT) + "..." :
                m.Description;
            });

            int totalMoviesCount = _db.Movies.Count();

            MoviesViewModel model = new MoviesViewModel
            {
                PagedMovies = new StaticPagedList<Movie>(movies, (int)page, _pageSize, totalMoviesCount < MAX_PAGINATION_LINKS ? totalMoviesCount : MAX_PAGINATION_LINKS),
                ApplicationUser = GetCurrentAppUser()
            };

            HttpCookie cookie = CreateCookie("currentPage", page.ToString());

            if (!ControllerContext.HttpContext.Request.Cookies.AllKeys.Contains("currentPage"))
            {
                Response.Cookies.Add(cookie);
            }
            else
            {
                Response.Cookies.Set(cookie);
            }

            return View(model);
        }

        // GET: Movies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = _db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            var appUser = GetCurrentAppUser();
            ViewBag.IsUserAuthorize = appUser == null ? false : true;
            ViewBag.userId = appUser == null ? null : appUser.Id;
            return View(movie);
        }

        // GET: Movies/Create
        [Authorize]
        public ActionResult Create()
        {
            SelectList directors = new SelectList(_db.Directors, "Id", "FullName");
            ViewBag.Directors = directors;
            return View(new MovieViewModel());
        }

        // POST: Movies/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "Name,Description,Year,ImageUpload")] MovieViewModel model)
        {
            ViewBag.Directors = new SelectList(_db.Directors, "Id", "FullName");
          
            if (!ModelState.IsValid)
            {                
                return View(model);
            } 
            
            string posterImagePath = string.Empty;
            try
            {
                posterImagePath = GetPosterImagePath(model.ImageUpload.FileName);
                model.ImageUpload.SaveAs(posterImagePath);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("SavePosterError", "Ошибка сохранения постера: " + ex.Message);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string directorId = Request.Form["Directors"].ToString();
            Movie movie = new Movie
            {
                ApplicationUserId = GetCurrentAppUser().Id,
                Description = model.Description,
                DirectorId = Convert.ToInt32(directorId),
                Name = model.Name,
                Year = model.Year,
                Poster = Path.GetFileName(posterImagePath)
            };

            _db.Movies.Add(movie);
            _db.SaveChanges();

            int movieCount = _db.Movies.Count();
            int page = movieCount / _pageSize;
            if (movieCount%_pageSize != 0)
            {
                page++;
            }                   
            return RedirectToAction("Index", "Movies", new { page = page } );            
         
        }

        // GET: Movies/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = _db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }        
           
            ViewBag.Directors = new SelectList(_db.Directors, "Id", "FullName", movie.DirectorId);
            ViewBag.Poster = movie.Poster;          

            MovieViewModel mvm = new MovieViewModel
            {
                Id = movie.Id,
                Name = movie.Name,
                Description = movie.Description,
                Year = movie.Year,
                ApplicationUserId = movie.ApplicationUserId,
                Directors = _db.Directors.Select(d => new DirectorInfo { Id = d.Id, FullName = d.FullName })                
            };

            return View(mvm);
        }

        // POST: Movies/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Year,ImageUpload,ApplicationUserId")] MovieViewModel model)
        {
            string directorId = Request.Form["Directors"].ToString();
            ViewBag.Directors = new SelectList(_db.Directors, "Id", "FullName", directorId);

            Movie movie = _db.Movies.Find(model.Id);
            ViewBag.Poster = movie.Poster;

            if (model.ApplicationUserId == GetCurrentAppUser().Id)
            {                             

                if (model.ImageUpload == null)
                {
                    ModelState.Remove("ImageUpload");
                }

                if (ModelState.IsValid)
                {

                    if (model.ImageUpload != null)
                    {
                        try
                        {
                            string posterImagePath = GetPosterImagePath(model.ImageUpload.FileName);
                            model.ImageUpload.SaveAs(posterImagePath);
                            movie.Poster = Path.GetFileName(posterImagePath);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("SavePosterError", "Ошибка сохранения постера: " + ex.Message);
                        }
                    }                   

                    movie.Name = model.Name;
                    movie.Description = model.Description;
                    movie.DirectorId = Convert.ToInt32(directorId);
                    movie.Year = model.Year;

                    _db.SaveChanges();

                    if (ControllerContext.HttpContext.Request.Cookies.AllKeys.Contains("currentPage"))
                    {
                        var pageNumber = Request.Cookies["currentpage"].Value;
                        return RedirectToAction("Index", "Movies", new { page = pageNumber });
                    }
                    return RedirectToAction("Index", "Movies");
                }
            }
            return View(model);
        }

        // GET: Movies/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = _db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            Movie movie = _db.Movies.Find(id);
            try
            {
                var uploadDir = "~/Posters";
                var imagePath = Path.Combine(Server.MapPath(uploadDir), movie.Poster);
                System.IO.File.Delete(imagePath);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
            }
            
            _db.Movies.Remove(movie);
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

        private HttpCookie CreateCookie(string key, string value)
        {
            HttpCookie StudentCookies = new HttpCookie(key);
            StudentCookies.Value = value;
            StudentCookies.Expires = DateTime.Now.AddHours(1);
            return StudentCookies;
        }

        private ApplicationUser GetCurrentAppUser()
        {
            return System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
        }

        private string GetPosterImagePath(string fileName)
        {
            var uploadDir = "~/Posters";
            string fileImage = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetExtension(fileName);
            return Path.Combine(Server.MapPath(uploadDir), fileImage);
        }

       
    }
}
