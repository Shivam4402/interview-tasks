using ADO_WEBAPI_CRUD.DTOs;
using ADO_WEBAPI_CRUD.Helpers;
using ADO_WEBAPI_CRUD.Models;
using ADO_WEBAPI_CRUD.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ADO_WEBAPI_CRUD.Controllers
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

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] StudentDto dto, IFormFile image)
        {
            var path = await _imageHelper.SaveImage(image);

            var student = new Student
            {
                Name = dto.Name,
                Email = dto.Email,
                ImagePath = path
            };

            var id = await _service.Create(student);

            return Ok(new { id });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAll();

            var result = data.Select(s => new
            {
                s.Id,
                s.Name,
                s.Email,
                ImageUrl = $"{Request.Scheme}://{Request.Host}{s.ImagePath}"
            });

            return Ok(result);
        }

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

            await _service.Update(existing);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _service.GetById(id);
            if (student == null) return NotFound();

            _imageHelper.DeleteImage(student.ImagePath);
            await _service.Delete(id);

            return Ok();
        }

    }
}
