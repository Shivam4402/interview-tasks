using Microsoft.AspNetCore.Mvc;
using SP_ADO_MultiTable.Models;
using SP_ADO_MultiTable.Services.Interfaces;

namespace SP_ADO_MultiTable.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IStudentService service;

        public StudentsController(IStudentService service)
        {
            this.service = service;
        }

        public IActionResult Index()
        {
            var students = service.GetAllStudents();
            return View(students);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Student student)
        {
            if (ModelState.IsValid)
            {
                service.AddStudent(student);
                return RedirectToAction("Index");
            }

            return View(student);
        }

        public IActionResult Delete(int id)
        {
            service.DeleteStudent(id);
            return RedirectToAction("Index");
        }
    }
}
