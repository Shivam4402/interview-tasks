using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using SP_ADO_CRUD.DTOs;
using SP_ADO_CRUD.Models;
using SP_ADO_CRUD.Services.Interfaces;
using System.Data;

namespace SP_ADO_CRUD.Controllers
{
    public class StudentController : Controller
    {

        private readonly IStudentService _service;
        private readonly IWebHostEnvironment _env;

        public StudentController(IStudentService service, IWebHostEnvironment env)
        {
            _service = service;
            _env = env;
        }


        void LoadData()
        {
            ViewBag.Qualifications = _service.GetQualifications();
            ViewBag.Technologies = _service.GetTechnologies();
        }

        public IActionResult Index()
        {
            return View(_service.GetAll());
        }


        public IActionResult Create()
        {
            LoadData();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Student model, List<int> selectedTechnologies, IFormFile file)
        {
            model.Technologies = selectedTechnologies != null
                ? string.Join(",", selectedTechnologies)
                : "";

            if (file != null)
            {
                string uploadsFolder = Path.Combine(_env.WebRootPath, "images");

                // ✅ Ensure folder exists
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(uploadsFolder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                file.CopyTo(stream);

                model.ImagePath = fileName;
            }

            _service.Insert(model);
            return RedirectToAction("Index");
        }


        public IActionResult Edit(int id)
        {
            var data = _service.GetById(id);

            ViewBag.SelectedTech = data.Technologies?.Split(',') ?? new string[] { };

            LoadData();
            return View(data);
        }


        [HttpPost]
        public IActionResult Edit(Student model, List<int> selectedTechnologies, IFormFile file)
        {
            model.Technologies = selectedTechnologies != null
                ? string.Join(",", selectedTechnologies)
                : "";

            if (file != null)
            {
                string uploadsFolder = Path.Combine(_env.WebRootPath, "images");

                // ✅ Ensure folder exists
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                string path = Path.Combine(_env.WebRootPath, "images", fileName);

                using var stream = new FileStream(path, FileMode.Create);
                file.CopyTo(stream);

                model.ImagePath = fileName;
            }

            _service.Update(model);
            return RedirectToAction("Index");
        }


        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return RedirectToAction("Index");
        }


    }
}
