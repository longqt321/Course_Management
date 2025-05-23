﻿using asp_project.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace asp_project.Controllers
{
    public class StudentController : Controller
    {
        private readonly MyAppContext context;
        public StudentController(MyAppContext _context) {
            this.context = _context;
        }
        public async Task<IActionResult> Index()
        {
            var students = await context.Users.Where(user => user.Role == Models.Role.Student)
                .ToListAsync();
            return View(students);
        }
    }
}
