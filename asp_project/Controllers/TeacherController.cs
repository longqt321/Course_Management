using asp_project.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace asp_project.Controllers
{
    public class TeacherController : Controller
    {
        private readonly MyAppContext context;
        public TeacherController(MyAppContext _context)
        {
            this.context = _context;
        }
        public async Task<IActionResult> Index()
        {
            var teachers = await context.Users.Where(user => user.Role == Models.Role.Teacher)
                .ToListAsync();
            return View(teachers);
        }
    }
}
