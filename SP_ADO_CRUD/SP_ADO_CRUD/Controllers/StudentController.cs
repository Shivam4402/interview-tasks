using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using SP_ADO_CRUD.Models;
using SP_ADO_CRUD.Services.Interfaces;
using SP_ADO_CRUD.ViewModels;
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

     
        public IActionResult Index()
        {
            return View(_service.GetAll());
        }


        public IActionResult Create()
        {
            var vm = new StudentViewModel
            {
                Qualifications = _service.GetQualifications(),
                Technologies = _service.GetTechnologies()
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(StudentViewModel vm, IFormFile file)
        {
            ModelState.Remove("ImagePath");
            ModelState.Remove("Technologies");
            ModelState.Remove("Qualifications");

            if (!ModelState.IsValid)
            {
                vm.Qualifications = _service.GetQualifications();
                vm.Technologies = _service.GetTechnologies();
                return View(vm);
            }

            var model = new Student
            {
                Name = vm.Name,
                Gender = vm.Gender,
                QualificationId = vm.QualificationId,
                DOB = vm.DOB,

                Technologies = vm.SelectedTechnologies != null
                    ? string.Join(",", vm.SelectedTechnologies)
                    : ""
            };

            var fileName = UploadImage(file);
            model.ImagePath = fileName;

            _service.Insert(model);

            return RedirectToAction("Index");
        }


        public IActionResult Edit(int id)
        {
            var data = _service.GetById(id);

            var vm = new StudentViewModel
            {
                Id = data.Id,
                Name = data.Name,
                Gender = data.Gender,
                QualificationId = data.QualificationId,
                ImagePath = data.ImagePath,
                DOB = data.DOB,

                // ✅ Convert CSV → List<int>
                SelectedTechnologies = !string.IsNullOrEmpty(data.Technologies)
                    ? data.Technologies.Split(',').Select(int.Parse).ToList()
                    : new List<int>(),

                Qualifications = _service.GetQualifications(),
                Technologies = _service.GetTechnologies()
            };

            return View(vm);
        }


        [HttpPost]
        public IActionResult Edit(StudentViewModel vm, IFormFile file)
        {
            ModelState.Remove("ImagePath");
            ModelState.Remove("Technologies");
            ModelState.Remove("Qualifications");

            // 🔴 STEP 1: Validation check
            if (!ModelState.IsValid)
            {
                vm.Qualifications = _service.GetQualifications();
                vm.Technologies = _service.GetTechnologies();
                return View(vm);
            }

            var existing = _service.GetById(vm.Id);

            var model = new Student
            {
                Id = vm.Id,
                Name = vm.Name,
                Gender = vm.Gender,
                QualificationId = vm.QualificationId,
                DOB = vm.DOB,

                Technologies = vm.SelectedTechnologies != null
                    ? string.Join(",", vm.SelectedTechnologies)
                    : ""
            };

            // 🔴 STEP 2: Image handling
            var fileName = UploadImage(file);

            if (fileName != null)
            {
                // delete old image
                if (!string.IsNullOrEmpty(existing.ImagePath))
                {
                    string oldPath = Path.Combine(_env.WebRootPath, "images", existing.ImagePath);

                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                model.ImagePath = fileName;
            }
            else
            {
                model.ImagePath = existing.ImagePath;
            }

            _service.Update(model);

            return RedirectToAction("Index");
        }



        public IActionResult Delete(int id)
        {
            var data = _service.GetById(id);

            if (data != null && !string.IsNullOrEmpty(data.ImagePath))
            {
                string imagePath = Path.Combine(_env.WebRootPath, "images", data.ImagePath);

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _service.Delete(id);

            return RedirectToAction("Index");
        }


        private string UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            if (file != null)
            {
                var allowed = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                var ext = Path.GetExtension(file.FileName).ToLower();

                if (!allowed.Contains(ext))
                {
                    ModelState.AddModelError("", "Invalid file type");
                }

                if (file.Length > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError("", "Max size 2MB");
                }
            }

            string uploadsFolder = Path.Combine(_env.WebRootPath, "images");

            // Ensure folder exists
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(uploadsFolder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(stream);

            return fileName;
        }



    }
}
