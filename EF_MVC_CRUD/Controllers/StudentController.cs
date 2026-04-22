using EF_MVC_CRUD.DTOs;
using EF_MVC_CRUD.Models;
using EF_MVC_CRUD.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EF_MVC_CRUD.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService _service;
        private readonly EfMvcCrudContext _context;

        public StudentController(IStudentService service, EfMvcCrudContext context)
        {
            _service = service;
            _context = context;
        }
        

        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAllAsync();
            return View(data);
        }

        public IActionResult Create()
        {
            ViewBag.States = _context.States.ToList();
            ViewBag.Technologies = _context.Technologies.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(StudentDto dto)
        {
            if (!ModelState.IsValid)
            {
                // reload dropdowns
                ViewBag.States = _context.States.ToList();
                ViewBag.Technologies = _context.Technologies.ToList();

                return View(dto);
            }
            await _service.CreateAsync(dto);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int id)
        {
            var student = await _service.GetByIdAsync(id);

            var dto = new StudentDto
            {
                StudentId = student.StudentId,
                Name = student.Name,
                Gender = student.Gender,
                StateId = student.StateId,
                Dob = student.Dob
            };

            ViewBag.States = _context.States.ToList();
            ViewBag.Technologies = _context.Technologies.ToList();
            ViewBag.SelectedTechnologies = student.Technologies?.Split(',').ToList();
            ViewBag.ExistingImage = student.ImagePath;

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(StudentDto dto)
        {
            if (!ModelState.IsValid)
            {
                // reload dropdowns
                ViewBag.States = _context.States.ToList();
                ViewBag.Technologies = _context.Technologies.ToList();

                return View(dto);
            }
            await _service.UpdateAsync(dto);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction("Index");
        }

    }
}
