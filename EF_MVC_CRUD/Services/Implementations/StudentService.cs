using EF_MVC_CRUD.DTOs;
using EF_MVC_CRUD.Models;
using EF_MVC_CRUD.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EF_MVC_CRUD.Services.Implementations
{
    public class StudentService : IStudentService
    {
        private readonly EfMvcCrudContext _context;
        private readonly IWebHostEnvironment _env;

        public StudentService(EfMvcCrudContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task CreateAsync(StudentDto dto)
        {
            var imagePath = await SaveImage(dto.ImageFile);

            var techNames = _context.Technologies
                .Where(t => dto.SelectedTechnologies.Contains(t.TechnologyId))
                .Select(t => t.TechnologyName)
                .ToList();

            var student = new Student
            {
                Name = dto.Name,
                Gender = dto.Gender,
                StateId = dto.StateId,
                Dob = dto.Dob,
                ImagePath = imagePath,
                Technologies = string.Join(",", techNames)
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Student>> GetAllAsync()
        {
            return await _context.Students.Include(s => s.State).ToListAsync();
        }

        public async Task<Student> GetByIdAsync(int id)
        {
            return await _context.Students.FindAsync(id);
        }


        public async Task UpdateAsync(StudentDto dto)
        {
            var student = await _context.Students.FindAsync(dto.StudentId);

            if (student == null) return;

            if (dto.ImageFile != null)
            {
                student.ImagePath = await SaveImage(dto.ImageFile);
            }

            var techNames = _context.Technologies
                .Where(t => dto.SelectedTechnologies.Contains(t.TechnologyId))
                .Select(t => t.TechnologyName)
                .ToList();

            student.Name = dto.Name;
            student.Gender = dto.Gender;
            student.StateId = dto.StateId;
            student.Dob = dto.Dob;
            student.Technologies = string.Join(",", techNames);

            await _context.SaveChangesAsync();
        }


        private async Task<string> SaveImage(IFormFile file)
        {
            if (file == null) return null;

            var folder = Path.Combine(_env.WebRootPath, "images");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var path = Path.Combine(folder, fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return "/images/" + fileName;
        }

    }
}
