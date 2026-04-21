using JS_CRUD_SinglePage.Models;
using JS_CRUD_SinglePage.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JS_CRUD_SinglePage.Services.Implementations
{
    public class StudentService : IStudentService
    {
        private readonly JsCrudSinglePageContext _db;
        private readonly IWebHostEnvironment _env;

        public StudentService(JsCrudSinglePageContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public async Task<List<Student>> GetAllAsync()
        {
            return await _db.Students.ToListAsync();
        }

        public async Task<Student> GetByIdAsync(int id)
        {
            return await _db.Students.FindAsync(id);
        }

        public async Task<string> AddAsync(StudentDto dto)
        {
            if (_db.Students.Any(x => x.Email == dto.Email))
                return "Email already exists";

            var student = new Student
            {
                Name = dto.Name,
                Email = dto.Email,
                Gender = dto.Gender,
                Dob = dto.Dob,
                StateId = dto.StateId,
                CityId = dto.CityId,
                Technologies = dto.Technologies
            };

            if (dto.ProfileImage != null)
            {
                student.ProfileImage = await SaveFile(dto.ProfileImage);
            }

            _db.Students.Add(student);
            await _db.SaveChangesAsync();

            return "Added";
        }

        public async Task<string> UpdateAsync(StudentDto dto)
        {
            var student = await _db.Students.FindAsync(dto.Id);

            if (student == null)
                return "Not Found";

            student.Name = dto.Name;
            student.Email = dto.Email;
            student.Gender = dto.Gender;
            student.Dob = dto.Dob;
            student.StateId = dto.StateId;
            student.CityId = dto.CityId;
            student.Technologies = dto.Technologies;

            if (dto.ProfileImage != null)
            {
                student.ProfileImage = await SaveFile(dto.ProfileImage);
            }

            await _db.SaveChangesAsync();

            return "Updated";
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var student = await _db.Students.FindAsync(id);

            if (student == null) return false;

            _db.Students.Remove(student);
            await _db.SaveChangesAsync();

            return true;
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string path = Path.Combine(_env.WebRootPath, "images", fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }
    }
}
