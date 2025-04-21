using asp_project.Data;
using asp_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace asp_project.Controllers
{
    public class UserController : Controller
    {
        private readonly MyAppContext context;
        public UserController(MyAppContext _context)
        {
            this.context = _context;
        }
        public async Task<IActionResult> Index()
        {
            string username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Authentication");
            }
            var users = await context.Users.ToListAsync();
            return View(users);
        }

        // UPDATE - GET: Hiển thị form sửa
        public async Task<IActionResult> Edit(int id)
        {
            var user = await context.Users.FindAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        // UPDATE - POST: Xử lý dữ liệu sửa
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(User user)
        {
            if (ModelState.IsValid)
            {
                context.Users.Update(user);
                await context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // DELETE - GET: Hiển thị form xác nhận xóa
        public async Task<IActionResult> Delete(int id)
        {
            var user = await context.Users.FindAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        // DELETE - POST: Xác nhận xóa
        [HttpPost, ActionName("DeleteConfirm")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var user = await context.Users.FindAsync(id);
            if (user != null)
            {
                context.Users.Remove(user);
                await context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        // DETAILS
        public async Task<IActionResult> Details(int id)
        {
            var user = await context.Users.FindAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }
    }
}
