using asp_project.Data;
using asp_project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace asp_project.Controllers
{
    public class CourseController : Controller
    {
        private readonly MyAppContext context;

        public CourseController(MyAppContext _context)
        {
            this.context = _context;
        }

        // GET: /Course
        public async Task<IActionResult> Index()
        {
            var courses = await context.Courses.Include(c => c.Teacher).ToListAsync();
            return View(courses);
        }
        private List<SelectListItem> SelectListTeachers()
        {
            return context.Users
            .Where(u => u.Role == Role.Teacher)
            .Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                Text = u.FirstName + " " + u.LastName
            })
            .ToList();
        } 
        // GET: /Course/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            Debug.WriteLine($"Create course");
            ViewBag.Teachers = SelectListTeachers();
            return View();
        }

        // POST: /Course/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Course course, IFormFile ImageFile)
        {
            Debug.WriteLine($"Course: {course}");
            Debug.WriteLine($"Teacher: { course.TeacherId}");
            Debug.WriteLine($"Image File: {ImageFile?.FileName}");
            Debug.WriteLine($"Image: {course.Image}");
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                var filePath = Path.Combine("Images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }
                course.Image = "/images/" + fileName;
            }
            Debug.WriteLine($"Image: {course.Image}");
            if (!ModelState.IsValid)
            {
                foreach (var entry in ModelState)
                {
                    var key = entry.Key;
                    var errors = entry.Value.Errors;
                    foreach (var error in errors)
                    {
                        Debug.WriteLine($"ModelState Error - Key: {key}, Error: {error.ErrorMessage}");
                    }
                }
            }
            if (ModelState.IsValid)
            {
                Debug.WriteLine($"ModelState is valid");
                context.Courses.Add(course);
                await context.SaveChangesAsync();
                return RedirectToAction("Index","Home");
            }
            ViewBag.Teachers = SelectListTeachers();

            return View(course);
        }

        // GET: /Course/Edit/{id}
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var course = await context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            ViewBag.Teachers = context.Users.Where(u => u.Role == Role.Teacher).ToList();
            return View(course);
        }

        // POST: /Course/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, Course updatedCourse)
        {
            if (id != updatedCourse.Id) return NotFound();

            if (ModelState.IsValid)
            {
                context.Update(updatedCourse);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Teachers = context.Users.Where(u => u.Role == Role.Teacher).ToList();
            return View(updatedCourse);
        }

        // GET: /Course/Delete/{id}
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var course = await context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            context.Courses.Remove(course);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: /Course/Enroll/{id}
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Enroll(int id)
        {

            return RedirectToAction("Index");
        }
    }
}
