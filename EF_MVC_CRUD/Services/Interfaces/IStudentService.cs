using EF_MVC_CRUD.DTOs;
using EF_MVC_CRUD.Models;

namespace EF_MVC_CRUD.Services.Interfaces
{
    public interface IStudentService
    {
        Task<List<Student>> GetAllAsync();
        Task<Student> GetByIdAsync(int id);
        Task CreateAsync(StudentDto dto);
        Task UpdateAsync(StudentDto dto);
        Task DeleteAsync(int id);
    }
}
