using Cinema.BusinessLogic.Abstractions;
using Cinema.DataAccess;
using Cinema.DataModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Security.Principal;

namespace CinemaWebApplication.Controllers
{
    public class ReservationsController : Controller
    {
        private ITicketsRepository ticketsRepo;
        private readonly CinemaDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ReservationsController(ITicketsRepository ticketsRepository, CinemaDbContext context, UserManager<IdentityUser> userManager)
        {
            this.ticketsRepo = ticketsRepository;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Tickets(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            IEnumerable<Tickets> tickets = _context.Tickets.Where(t => t.MovieId == id);
            if (tickets == null)
            {
                return NotFound();
            }
            return View(tickets);
        }

        [Authorize(Roles = "Client, Admin")]
        public async Task<IActionResult> BookTickets(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BookingViewModel booking = new BookingViewModel();
            booking.ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == id);
            booking.movie = _context.Movies.FirstOrDefault(t => t.Id == booking.ticket.MovieId);

            if (booking.ticket == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: ReservationsController
        public async Task<IActionResult> Index(Reservations res)
        {
            if (ModelState.IsValid)
            {
                _context.Add(res);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // GET: ReservationsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        // GET: ReservationsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReservationsController/Create

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Tickets ticket, [FromRoute] Guid Id)
        {
            if (ModelState.IsValid)
            {
                ticket.MovieId = Id;
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View(ticket);
        }

        // GET: ReservationsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ReservationsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "Admin")]
        // GET: ReservationsController/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        [Authorize(Roles = "Admin")]
        // POST: ReservationsController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Client, Admin")]
        [HttpGet]
        public ActionResult Confirm([FromRoute] Guid Id)
        {
            BookingViewModel booking = new BookingViewModel();
            booking.ticket.Id = Id;
            return View(booking);
        }

        [Authorize(Roles = "Client, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirm([FromForm] BookingViewModel res, [FromRoute] Guid Id)
        {
            if (ModelState.IsValid)
            {
                var ticket = _context.Tickets.FirstOrDefault(t => t.Id == Id);
                var user = _userManager.GetUserId(User);

                Reservations reservation = new Reservations
                {
                    TicketId = Id,
                    MovieId = ticket.MovieId,
                    UserId = user,
                    NumberOfSeats = res.reservation.NumberOfSeats
                };
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
    }
}
