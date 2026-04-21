using JS_CRUD_SinglePage.Models;

namespace JS_CRUD_SinglePage.Services.Interfaces
{
    public interface IStudentService
    {
        Task<List<StudentViewDto>> GetAllAsync();
        Task<Student> GetByIdAsync(int id);
        Task<List<State>> GetStatesAsync();
        Task<List<City>> GetCitiesAsync(int stateId);
        Task<string> AddAsync(StudentDto dto);
        Task<string> UpdateAsync(StudentDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
