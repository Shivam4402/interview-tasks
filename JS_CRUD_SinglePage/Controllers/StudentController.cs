using JS_CRUD_SinglePage.Models;
using JS_CRUD_SinglePage.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JS_CRUD_SinglePage.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService _service;

        public StudentController(IStudentService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> GetStates()
        {
            return Json(await _service.GetStatesAsync());
        }

        public async Task<JsonResult> GetCities(int stateId)
        {
            return Json(await _service.GetCitiesAsync(stateId));
        }

        public async Task<JsonResult> GetAllStudents()
        {
            return Json(await _service.GetAllAsync());
        }

        public async Task<JsonResult> GetStudentById(int id)
        {
            return Json(await _service.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<JsonResult> AddStudent([FromForm] StudentDto dto)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Invalid data" });

            var result = await _service.AddAsync(dto);

            return Json(new { success = result});
        }

        [HttpPost]
        public async Task<JsonResult> UpdateStudent([FromForm] StudentDto dto)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Invalid data" });

            var success = await _service.UpdateAsync(dto);

            return Json(success);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteStudent(int id)
        {
            return Json(await _service.DeleteAsync(id));
        }

    }
}
