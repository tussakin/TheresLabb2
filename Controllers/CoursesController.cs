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
    public class CoursesController : Controller
    {
        private readonly TheresLabb2DbContext _context;

        public CoursesController(TheresLabb2DbContext context)
        {
            _context = context;
        }


        public IActionResult TriggerAddTeachersToCourses()
        {
            AddTeachersToCourses();
            return RedirectToAction("Index");
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            var courses = await _context.Courses.ToListAsync();
            return View(courses);
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseId,CourseTitle,CourseDescription")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseId,CourseTitle,CourseDescription")] Course course)
        {
            if (id != course.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.CourseId))
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
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseId == id);
        }

        public void AddTeachersToCourses()
        {
            try
            {
                var allCourses = _context.Courses.ToList();
                var allTeachers = _context.Teachers.ToList(); 
                var random = new Random();

                foreach (var course in allCourses)
                {
                    if (course.Teachers == null)
                    {
                        course.Teachers = new List<Teacher>(); 
                    }

                    int numTeachers = random.Next(1, 4); 

                    for (int i = 0; i < numTeachers; i++)
                    {
                        var randomTeacher = allTeachers[random.Next(allTeachers.Count)];

                        if (randomTeacher != null && !course.Teachers.Contains(randomTeacher)) 
                        {
                            course.Teachers.Add(randomTeacher); 
                        }
                    }
                }

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while adding teachers to courses: {ex.Message}");
            }

          
        }
        public async Task<IActionResult> EditCourseTitle(int courseId, string newTitle)
        {
            var course = await _context.Courses.FindAsync(courseId);

            if (course == null)
            {
                return NotFound();
            }

            course.CourseTitle = newTitle;

            _context.Update(course);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
