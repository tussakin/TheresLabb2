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
    public class StudentsController : Controller
    {
        private readonly TheresLabb2DbContext _context;

        public StudentsController(TheresLabb2DbContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            var studentsWithCoursesAndTeachers = await _context.Students
       .Include(s => s.Courses) 
       .ThenInclude(c => c.Teachers) 
       .ToListAsync(); 

            return View(studentsWithCoursesAndTeachers);

        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.StudentYear)
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            ViewData["FkStudentYearId"] = new SelectList(_context.StudentYears, "StudentYearId", "StudentYearId");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId,StudentName,FkStudentYearId,FkCourseId")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FkStudentYearId"] = new SelectList(_context.StudentYears, "StudentYearId", "StudentYearId", student.FkStudentYearId);
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            ViewData["FkStudentYearId"] = new SelectList(_context.StudentYears, "StudentYearId", "StudentYearId", student.FkStudentYearId);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentId,StudentName,FkStudentYearId,FkCourseId")] Student student)
        {
            if (id != student.StudentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.StudentId))
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
            ViewData["FkStudentYearId"] = new SelectList(_context.StudentYears, "StudentYearId", "StudentYearId", student.FkStudentYearId);
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.StudentYear)
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.StudentId == id);
        }


        public void AddCoursesStudent()
        {
            try
            {
                var allStudents = _context.Students.ToList();
                var allCourses = _context.Courses.ToList();
                var random = new Random();

                foreach (var student in allStudents)
                {
                    if (student.Courses == null) 
                    {
                        student.Courses = new List<Course>();
                    }

                    for (int i = 0; i < 3; i++)
                    {
                        var randomCourse = allCourses[random.Next(allCourses.Count)];

                        if (randomCourse != null)
                        {
                            student.Courses.Add(randomCourse);
                        }
                    }
                }

                _context.SaveChanges(); 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while adding courses to students: {ex.Message}");
            }
        }

            public IActionResult TriggerAddCoursesStudent()
        {
            AddCoursesStudent(); 
            return RedirectToAction("Index"); 
        }

        public async Task<IActionResult> StudentsForProgramming1()
        {
            var studentsForProgramming1 = await _context.Courses
                                                        .Where(c => c.CourseTitle == "Programming 1")
                                                        .SelectMany(c => c.Students)
                                                        .Distinct()
                                                        .ToListAsync();

            return View(studentsForProgramming1);
        }


        public async Task<IActionResult> ChangeStudentTeacher(int studentId)
        {
            var student = await _context.Students
                                        .Include(s => s.Courses) 
                                        .ThenInclude(c => c.Teachers) 
                                        .FirstOrDefaultAsync(s => s.StudentId == studentId);

            if (student == null)
            {
                return NotFound(); 
            }

            var programming1 = student.Courses.FirstOrDefault(c => c.CourseTitle == "Programmering 1");

            if (programming1 == null)
            {
                return NotFound(); 
            }

            var reidar = programming1.Teachers.FirstOrDefault(t => t.TeacherName == "Reidar");

            var tobias = programming1.Teachers.FirstOrDefault(t => t.TeacherName == "Tobias");

            if (reidar != null && tobias != null)
            {
                programming1.Teachers.Remove(reidar);
                programming1.Teachers.Add(tobias);

                _context.Update(programming1); 
                await _context.SaveChangesAsync(); 
            }

            return RedirectToAction("Index"); 
        }


    


        public async Task<IActionResult> ChangeTeacherForProgramming1(int studentId, string oldTeacherName, string newTeacherName)
        {
            var student = await _context.Students
                .Include(s => s.Courses)
                .ThenInclude(c => c.Teachers)
                .FirstOrDefaultAsync(s => s.StudentId == studentId);

            if (student == null) return NotFound();

            var programming1 = student.Courses.FirstOrDefault(c => c.CourseTitle == "Programming 1");

            if (programming1 == null) return BadRequest("Student is not enrolled in Programming 1");

            var oldTeacher = programming1.Teachers.FirstOrDefault(t => t.TeacherName == oldTeacherName);
            var newTeacher = _context.Teachers.FirstOrDefault(t => t.TeacherName == newTeacherName);

            if (oldTeacher != null && newTeacher != null)
            {
                programming1.Teachers.Remove(oldTeacher);
                programming1.Teachers.Add(newTeacher);

                await _context.SaveChangesAsync();
            }


            return RedirectToAction("Index");
        }
    }
}
