using Cinema.BusinessLogic.Abstractions;
using Cinema.DataAccess;
using Cinema.DataModel;
using CinemaWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaWebApplication.Controllers
{
    public class HomeController : Controller
    {
        private IMoviesRepository moviesRepo;
        private readonly CinemaDbContext _context;

        public HomeController(IMoviesRepository moviesRepository, CinemaDbContext context)
        {
            this.moviesRepo = moviesRepository;
            _context = context;
        }

        public IActionResult Index()
        {
            var movies = moviesRepo.GetCurrentMovies();
            return View(movies);
        }

        public async Task<IActionResult> Details(Guid? id)
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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
