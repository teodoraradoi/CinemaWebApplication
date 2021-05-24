using Cinema.BusinessLogic.Abstractions;
using Cinema.DataAccess;
using Cinema.DataModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaWebApplication.Controllers
{
    public class MoviesController : Controller
    {
        private IMoviesRepository moviesRepo;
        private readonly CinemaDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public MoviesController(IMoviesRepository moviesRepository, CinemaDbContext context, IWebHostEnvironment hostEnvironment)
        {
            this.moviesRepo = moviesRepository;
            _context = context;
            webHostEnvironment = hostEnvironment;
        }

        // GET: MoviesController
        public ActionResult Index()
        {
            var movies = moviesRepo.GetCurrentMovies();
            return View(movies);
        }

        public ActionResult Coming_soon()
        {
            var movies = moviesRepo.GetComingSoonMovies();
            return View(movies);
        }

        // GET: MoviesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        // GET: MoviesController/Create
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        // POST: MoviesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] MovieViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(model);
                Movies movie = new Movies
                {
                    Name = model.Name,
                    ReleaseDate = model.ReleaseDate,
                    Description = model.Description,
                    Duration = model.Duration,
                    Rating = model.Rating,
                    Type = model.Type,
                    Genre = model.Genre,
                    Actors = model.Actors,
                    Directors = model.Directors,
                    Trailer = model.Trailer,
                    Image = uniqueFileName,
                };
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        private string UploadedFile(MovieViewModel model)
        {
            string uniqueFileName = null;

            if (model.Poster != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = model.Poster.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Poster.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        [Authorize(Roles = "Admin")]
        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        [Authorize(Roles = "Admin")]
        // POST: MoviesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Image,ReleaseDate,Description,Duration,Rating,Type,Genre,Actors,Directors")] Movies movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        private bool MovieExists(Guid id)
        {
            return _context.Movies.Any(m => m.Id == id);
        }

        [Authorize(Roles = "Admin")]
        // GET: MoviesController/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        [Authorize(Roles = "Admin")]
        // POST: MoviesController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var movie = await _context.Movies.FindAsync(id);
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
