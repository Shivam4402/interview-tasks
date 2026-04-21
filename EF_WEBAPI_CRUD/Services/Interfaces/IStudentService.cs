using EF_WEBAPI_CRUD.Models;

namespace EF_WEBAPI_CRUD.Services.Interfaces
{
    public interface IStudentService
    {
        Task<List<Student>> GetAll();
        Task<Student> GetById(int id);
        Task<Student> Create(Student student);
        Task<Student> Update(int id, Student student);
        Task<bool> Delete(int id);      
    }
}
