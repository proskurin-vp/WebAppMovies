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

namespace WebAppSportsLeagueTestTask.WEB.Controllers
{
    public class MoviesController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        private int _pageSize = 4; // количество фильмов на страницу    

        // GET: Movies
        public ActionResult Index(int page = 1)
        {
            var movies = _db.Movies.Include(m => m.Director)
                .ToList()
                .Select(m => 
                {
                    m.Description = m.Description.Length > 120 ?
                    m.Description.Substring(0, 120) + "..." :
                    m.Description;
                    return m;
                });
           
                   
            IEnumerable<Movie> moviesPerPages = movies.Skip((page - 1) * _pageSize).Take(_pageSize);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = _pageSize, TotalItems = movies.Count(), Offset = 2 };
            PageViewModel pvm = new PageViewModel { PageInfo = pageInfo, Movies = moviesPerPages };

            var appUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            ViewBag.IsUserAuthorize = appUser == null ? false : true;
            ViewBag.userId = appUser == null ? null : appUser.Id;

            HttpCookie cookie =  CreateCookie("currentPage", pageInfo.PageNumber.ToString());

            if(!ControllerContext.HttpContext.Request.Cookies.AllKeys.Contains("currentPage"))
            {                
                Response.Cookies.Add(cookie);
            }
            else
            {
                Response.Cookies.Set(cookie);
            }            

            return View(pvm);
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
            var appUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
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
            var validImageTypes = new string[]
            {
                "image/gif",
                "image/jpeg",
                "image/pjpeg",
                "image/png"
            };

            if (model.ImageUpload == null || model.ImageUpload.ContentLength == 0)
            {
                ModelState.AddModelError("ImageUpload", "This field is required");
            }
            else if (!validImageTypes.Contains(model.ImageUpload.ContentType))
            {
                ModelState.AddModelError("ImageUpload", "Please choose either a GIF, JPG or PNG image.");
            }

            string directorId = Request.Form["Directors"].ToString();

            if (ModelState.IsValid)
            {
                if (model.ImageUpload != null && model.ImageUpload.ContentLength > 0)
                {
                    try
                    {
                        var uploadDir = "~/Posters";
                        var imagePath = Path.Combine(Server.MapPath(uploadDir), model.ImageUpload.FileName);
                        var imageUrl = Path.Combine(uploadDir, model.ImageUpload.FileName);
                        model.ImageUpload.SaveAs(imagePath);

                        var appUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                        Movie movie = new Movie
                        {
                            ApplicationUserId = appUser.Id,
                            Description = model.Description,
                            DirectorId = Convert.ToInt32(directorId),
                            Name = model.Name,
                            Year = model.Year,
                            Poster = model.ImageUpload.FileName
                        };

                        _db.Movies.Add(movie);
                        _db.SaveChanges();

                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("Error", ex.Message);
                    }
                
                    int movieCount = _db.Movies.Count();
                    int page = movieCount / _pageSize;
                    if (movieCount%_pageSize != 0)
                    {
                        page++;
                    }                   
                    return RedirectToAction("Index", "Movies", new { page = page } );
                }                
            }

            SelectList directors = new SelectList(_db.Directors, "Id", "FullName");
            ViewBag.Directors = directors;
            return View(model);           
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
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Year,ImageUpload")] MovieViewModel model)
        {
            string directorId = Request.Form["Directors"].ToString();

            if (ModelState.IsValid)
            {                
                Movie movie = _db.Movies.Find(model.Id);
                if(model.ImageUpload != null)
                {
                    try
                    {
                        var uploadDir = "~/Posters";
                        var imagePath = Path.Combine(Server.MapPath(uploadDir), model.ImageUpload.FileName);
                        var imageUrl = Path.Combine(uploadDir, model.ImageUpload.FileName);
                        model.ImageUpload.SaveAs(imagePath);
                        movie.Poster = model.ImageUpload.FileName;
                    }
                    catch(Exception ex)
                    {
                        ModelState.AddModelError("Error", ex.Message);
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

            ViewBag.DirectorId = new SelectList(_db.Directors, "Id", "FullName", directorId);
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
    }
}
