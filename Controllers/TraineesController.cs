using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyfirstCoreMVC.Models;
using NToastNotify;

namespace MyfirstCoreMVC.Controllers
{
    public class TraineesController : Controller
    {
        // NToastNotify
        private readonly ILogger<TraineesController> _logger;
        private readonly IToastNotification _toastNotification;
        private readonly EvryContext _context;

        public TraineesController(ILogger<TraineesController> logger, IToastNotification toastNotification, EvryContext context)
        {
            _logger = logger;
            _toastNotification = toastNotification;
            _context = context;
        }

        // GET: Trainees
        public async Task<IActionResult> Index()
        {
              return View(await _context.Trainees.ToListAsync());
        }

        // GET: Trainees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Trainees == null)
            {
                return NotFound();
            }

            var trainee = await _context.Trainees
                .FirstOrDefaultAsync(m => m.Tid == id);
            if (trainee == null)
            {
                return NotFound();
            }

            return View(trainee);
        }

        // GET: Trainees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Trainees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Tid,Name,Tage")] Trainee trainee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(trainee);
                await _context.SaveChangesAsync();
                // toastNotification in green color - > SUCCESS
                _toastNotification.AddSuccessToastMessage("Employee created successfully");
                return RedirectToAction(nameof(Index));
            }
            return View(trainee);
        }

        // GET: Trainees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Trainees == null)
            {
                return NotFound();
            }

            var trainee = await _context.Trainees.FindAsync(id);
            if (trainee == null)
            {
                return NotFound();
            }
            return View(trainee);
        }

        // POST: Trainees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Tid,Name,Tage")] Trainee trainee)
        {
            if (id != trainee.Tid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainee);
                    await _context.SaveChangesAsync();
                    //toastNotification in yellow color - > WARNING
                    _toastNotification.AddWarningToastMessage("Employee updated successfully");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TraineeExists(trainee.Tid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(trainee);
        }

        // GET: Trainees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Trainees == null)
            {
                return NotFound();
            }

            var trainee = await _context.Trainees
                .FirstOrDefaultAsync(m => m.Tid == id);
            if (trainee == null)
            {
                return NotFound();
            }

            return View(trainee);
        }

        // POST: Trainees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Trainees == null)
            {
                return Problem("Entity set 'EvryContext.Trainees'  is null.");
            }
            var trainee = await _context.Trainees.FindAsync(id);
            if (trainee != null)
            {
                _context.Trainees.Remove(trainee);
            }
            
            await _context.SaveChangesAsync();
            //toastNotification in red color - > Error
            _toastNotification.AddErrorToastMessage("Employee deleted successfully");
            return RedirectToAction(nameof(Index));
        }

        private bool TraineeExists(int id)
        {
          return _context.Trainees.Any(e => e.Tid == id);
        }
    }
}
