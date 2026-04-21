using ADO_WEBAPI_CRUD.Models;

namespace ADO_WEBAPI_CRUD.Services.Interfaces
{
    public interface IStudentService
    {
        Task<List<Student>> GetAll();
        Task<Student> GetById(int id);
        Task<int> Create(Student student);
        Task<bool> Update(Student student);
        Task<bool> Delete(int id);
    }
}
