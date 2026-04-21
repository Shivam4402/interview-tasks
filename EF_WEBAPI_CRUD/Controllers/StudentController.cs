using EF_WEBAPI_CRUD.DTOs;
using EF_WEBAPI_CRUD.Helpers;
using EF_WEBAPI_CRUD.Models;
using EF_WEBAPI_CRUD.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EF_WEBAPI_CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _service;
        private readonly ImageHelper _imageHelper;

        public StudentController(IStudentService service, ImageHelper imageHelper)
        {
            _service = service;
            _imageHelper = imageHelper;
        }

        // GET ALL
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAll();
            return Ok(data);
        }

        // GET BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var data = await _service.GetById(id);
            if (data == null) return NotFound();
            return Ok(data);
        }

        // CREATE (WITH IMAGE)
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] StudentDto dto, IFormFile image)
        {
            var imagePath = await _imageHelper.SaveImage(image);

            var student = new Student
            {
                Name = dto.Name,
                Email = dto.Email,
                ImagePath = imagePath
            };

            var result = await _service.Create(student);
            return Ok(result);
        }

        // UPDATE
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] StudentDto dto, IFormFile image)
        {
            var existing = await _service.GetById(id);
            if (existing == null) return NotFound();

            if (image != null)
            {
                _imageHelper.DeleteImage(existing.ImagePath);

                existing.ImagePath = await _imageHelper.SaveImage(image);
            }

            existing.Name = dto.Name;
            existing.Email = dto.Email;

            var result = await _service.Update(id, existing);

            return Ok(result);
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _service.GetById(id);
            if (student == null) return NotFound();

            _imageHelper.DeleteImage(student.ImagePath);

            await _service.Delete(id);

            return Ok("Deleted successfully");
        }
    }
}
