using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Labb2Theres.Models;
using TheresLabb2.Data;

namespace TheresLabb2.Controllers
{
    public class StudentYearsController : Controller
    {
        private readonly TheresLabb2DbContext _context;

        public StudentYearsController(TheresLabb2DbContext context)
        {
            _context = context;
        }

        // GET: StudentYears
        public async Task<IActionResult> Index()
        {
            return View(await _context.StudentYears.ToListAsync());
        }

        // GET: StudentYears/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentYear = await _context.StudentYears
                .FirstOrDefaultAsync(m => m.StudentYearId == id);
            if (studentYear == null)
            {
                return NotFound();
            }

            return View(studentYear);
        }

        // GET: StudentYears/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StudentYears/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentYearId,StudentYearName")] StudentYear studentYear)
        {
            if (ModelState.IsValid)
            {
                _context.Add(studentYear);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(studentYear);
        }

        // GET: StudentYears/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentYear = await _context.StudentYears.FindAsync(id);
            if (studentYear == null)
            {
                return NotFound();
            }
            return View(studentYear);
        }

        // POST: StudentYears/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentYearId,StudentYearName")] StudentYear studentYear)
        {
            if (id != studentYear.StudentYearId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studentYear);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentYearExists(studentYear.StudentYearId))
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
            return View(studentYear);
        }

        // GET: StudentYears/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentYear = await _context.StudentYears
                .FirstOrDefaultAsync(m => m.StudentYearId == id);
            if (studentYear == null)
            {
                return NotFound();
            }

            return View(studentYear);
        }

        // POST: StudentYears/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var studentYear = await _context.StudentYears.FindAsync(id);
            if (studentYear != null)
            {
                _context.StudentYears.Remove(studentYear);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentYearExists(int id)
        {
            return _context.StudentYears.Any(e => e.StudentYearId == id);
        }
    }
}
