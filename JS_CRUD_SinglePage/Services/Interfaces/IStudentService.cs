using JS_CRUD_SinglePage.Models;

namespace JS_CRUD_SinglePage.Services.Interfaces
{
    public interface IStudentService
    {
        Task<List<Student>> GetAllAsync();
        Task<Student> GetByIdAsync(int id);
        Task<string> AddAsync(StudentDto dto);
        Task<string> UpdateAsync(StudentDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
