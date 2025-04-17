using asp_project.Data;
using asp_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace asp_project.Controllers
{
    public class StudentController : Controller
    {
        private readonly MyAppContext context;
        public StudentController(MyAppContext _context)
        {
            this.context = _context;
        }
        public async Task<IActionResult> Index()
        {
            var students = await context.Students.ToListAsync();
            return View(students);
        }
        // CREATE - GET: Hiển thị form tạo mới
        public IActionResult Create()
        {
            return View();
        }

        // CREATE - POST: Xử lý dữ liệu từ form gửi về
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            if (ModelState.IsValid)
            {
                context.Students.Add(student);
                await context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // UPDATE - GET: Hiển thị form sửa
        public async Task<IActionResult> Edit(int id)
        {
            var student = await context.Students.FindAsync(id);
            if (student == null) return NotFound();
            return View(student);
        }

        // UPDATE - POST: Xử lý dữ liệu sửa
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Student student)
        {
            if (ModelState.IsValid)
            {
                context.Students.Update(student);
                await context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // DELETE - GET: Hiển thị form xác nhận xóa
        public async Task<IActionResult> Delete(int id)
        {
            var student = await context.Students.FindAsync(id);
            if (student == null) return NotFound();
            return View(student);
        }

        // DELETE - POST: Xác nhận xóa
        [HttpPost, ActionName("DeleteConfirm")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var student = await context.Students.FindAsync(id);
            if (student != null)
            {
                context.Students.Remove(student);
                await context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        // DETAILS
        public async Task<IActionResult> Details(int id)
        {
            var student = await context.Students.FindAsync(id);
            if (student == null) return NotFound();
            return View(student);
        }
    }
}
