using EF_WEBAPI_CRUD.Data;
using EF_WEBAPI_CRUD.Models;
using EF_WEBAPI_CRUD.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EF_WEBAPI_CRUD.Services.Implementations
{
    public class StudentService : IStudentService
    {
        private readonly AppDbContext _context;

        public StudentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Student>> GetAll()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<Student> GetById(int id)
        {
            return await _context.Students.FindAsync(id);
        }

        public async Task<Student> Create(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return student;
        }
        public async Task<Student> Update(int id, Student student)
        {
            var existing = await _context.Students.FindAsync(id);
            if (existing == null) return null;

            existing.Name = student.Name;
            existing.Email = student.Email;
            existing.ImagePath = student.ImagePath;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> Delete(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return false;

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
