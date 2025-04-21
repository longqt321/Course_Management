using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using asp_project.Data;
using asp_project.Models;
using System.Security.Principal;
using System.Diagnostics;
using System.Text;
using System.Security.Cryptography;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace asp_project.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly MyAppContext context;
        private readonly IConfiguration config;
        public AuthenticationController(MyAppContext _content, IConfiguration _config)
        {
            this.context = _content;
            this.config = _config;
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home"); // hoặc trang mặc định của bạn
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = context.Users.SingleOrDefault(u => u.Username == username);
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                };
                var identity = new ClaimsIdentity(claims, "MyDUTCookie");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("MyDUTCookie", principal);

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Sai tài khoản hoặc mật khẩu");
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyDUTCookie");
            return RedirectToAction("Login", "Authentication");
        }
        [AllowAnonymous]
        public IActionResult Register()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user,string RoleKey = "")
        {
            Debug.WriteLine("RoleKey: " + RoleKey);
            if (ModelState.IsValid)
            {
                String TeacherSecretKey = config["Keys:TeacherRoleKey"];
                String AdminSecretKey = config["Keys:AdminRoleKey"];
                Debug.WriteLine("TeacherSecretKey: " + TeacherSecretKey);
                Debug.WriteLine("AdminSecretKey: " + AdminSecretKey);
                bool isExistingUser = context.Users.Any(u => u.Username == user.Username);
                if (isExistingUser == true)
                {
                    ModelState.AddModelError("Username", "Username đã tồn tại");
                    return View(user);
                }
                Debug.WriteLine("Role: " + user.Role);
                Debug.WriteLine("Teacher = " + Role.Teacher);
                String EncryptedRoleKey = EncryptWithSHA256(RoleKey);
                Debug.WriteLine("EncryptedRoleKey: " + EncryptedRoleKey);
                if (user.Role == Role.Teacher && TeacherSecretKey != EncryptedRoleKey)
                {
                    ModelState.AddModelError("Role", "Mật khẩu xác thực không chính xác");
                    return View(user);
                }
                else if(user.Role == Role.Admin && AdminSecretKey != EncryptedRoleKey)
                {
                    ModelState.AddModelError("Role", "Mật khẩu xác thực không chính xác");
                    return View(user);
                }
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                context.Users.Add(user);
                await context.SaveChangesAsync();
                return RedirectToAction("Login");
            }
            return View(user);
        }
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
        private string EncryptWithSHA256(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
