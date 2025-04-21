using System.Diagnostics;
using asp_project.Data;
using asp_project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace asp_project.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly MyAppContext context;
        public UserController(MyAppContext _context)
        {
            this.context = _context;
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var users = await context.Users.ToListAsync();
            return View(users);
        }

        // UPDATE - GET: Hiển thị form sửa
        [Authorize]
        public async Task<IActionResult> Edit()
        {
            var currentUsername = User?.Identity?.Name;
            if (currentUsername == null) return RedirectToAction("Login", "Authentication");
            var user = context.Users.SingleOrDefault(u => u.Username == currentUsername);
            Debug.WriteLine($"Current user: {user.Username}");
            Debug.WriteLine($"Current user ID: {user.Id}");
            if (user == null) return NotFound();
            return View(user);
        }

        // UPDATE - POST: Xử lý dữ liệu sửa
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(User input)
        {
            var currentUsername = User?.Identity?.Name;
            if (currentUsername == null) return RedirectToAction("Login", "Authentication");
            var user = context.Users.SingleOrDefault(u => u.Username == currentUsername);
            
            if (!String.IsNullOrEmpty(input.Password) 
                && !String.IsNullOrEmpty(user.Password) 
                && !BCrypt.Net.BCrypt.Verify(input.Password, user.Password))
            {
                ModelState.AddModelError("Password", "Mật khẩu xác thực không chính xác");
                return View(user);
            }
            user.FirstName = input.FirstName;
            user.LastName = input.LastName;
            user.Email = input.Email;
            user.PhoneNumber = input.PhoneNumber;
            user.BirthDay = input.BirthDay;


            if (ModelState.IsValid)
            {
                context.Users.Update(user);
                await context.SaveChangesAsync();
                return RedirectToAction("Index","Home");
            }
            return View(user);
        }

        // DELETE - GET: Hiển thị form xác nhận xóa
        [Authorize(Roles = "Admin")]
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
            return RedirectToAction("Index","Home");
        }

        // DETAILS
        public async Task<IActionResult> Details()
        {
            var currentUsername = User?.Identity?.Name;
            if (currentUsername == null) return RedirectToAction("Login", "Authentication");
            var user = context.Users.SingleOrDefault(u => u.Username == currentUsername);
            Debug.WriteLine($"Current user: {user.Username}");
            Debug.WriteLine($"Current user ID: {user.Id}");
            if (user == null) return NotFound();
            return View(user);
        }
    }
}
