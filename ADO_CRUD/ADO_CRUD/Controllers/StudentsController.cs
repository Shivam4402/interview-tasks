using ADO_CRUD.Models;
using ADO_CRUD.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ADO_CRUD.Controllers
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
            var list = service.GetAllStudents();
            return View(list);
        }

        public IActionResult Create()
        {
            ViewBag.Qualifications = service.GetAllQualifications();
            return View();
        }

        [HttpPost]
        public IActionResult Create(StudentVM vm)
        {
            if (ModelState.IsValid)
            {
                // Convert ViewModel → Model
                Student student = new Student
                {
                    Name = vm.Name,
                    Gender = vm.Gender,
                    DOB = vm.DOB,
                    QualificationId = vm.QualificationId,
                    Technologies = vm.Technologies != null
                        ? string.Join(",", vm.Technologies)
                        : "",
                    ImageFile = vm.ImageFile
                };

                service.AddStudent(student);

                return RedirectToAction("Index");
            }

            ViewBag.Qualifications = service.GetAllQualifications();
            return View(vm);
        }

        public IActionResult Delete(int id)
        {
            service.DeleteStudent(id);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var student = service.GetStudentById(id);

            var vm = new StudentEditVM
            {
                StudentId = student.StudentId,
                Name = student.Name,
                Gender = student.Gender,
                DOB = student.DOB,
                QualificationId = student.QualificationId,
                Technologies = student.Technologies?.Split(','),
                ExistingImagePath = student.ImagePath
            };

            ViewBag.Qualifications = service.GetAllQualifications();
            return View(vm);
        }

        [HttpPost]
        public IActionResult Edit(StudentEditVM vm)
        {
            ViewBag.Qualifications = service.GetAllQualifications();

            if (ModelState.IsValid)
            {
                // Convert ViewModel → Model
                Student student = new Student
                {
                    StudentId = vm.StudentId,
                    Name = vm.Name,
                    Gender = vm.Gender,
                    DOB = vm.DOB,
                    QualificationId = vm.QualificationId,
                    Technologies = vm.Technologies != null
                        ? string.Join(",", vm.Technologies)
                        : "",
                    ImageFile = vm.ImageFile,
                    ImagePath = vm.ExistingImagePath
                };

                service.UpdateStudent(student);

                return RedirectToAction("Index");
            }

            return View(vm);
        }


        public IActionResult Details(int id)
        {
            var student = service.GetStudentById(id);

            var vm = new Student
            {
                StudentId = student.StudentId,
                Name = student.Name,
                Gender = student.Gender,
                DOB = student.DOB,
                QualificationId = student.QualificationId,
                QualificationName = student.QualificationName,
                //Technologies = student.Technologies?.Split(','),
                Technologies = student.Technologies ?? string.Empty,  // Don't split - Technologies is string, not string[]
                ImagePath = student.ImagePath
            };

            return View(vm);
        }


    }
}
