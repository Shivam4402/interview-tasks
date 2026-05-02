using SP_ADO_CRUD.Models;

namespace SP_ADO_CRUD.Services.Interfaces
{
    public interface IStudentService
    {
        List<Student> GetAll();
        Student GetById(int id);
        void Insert(Student model);
        void Update(Student model);
        void Delete(int id);
        List<Qualification> GetQualifications();
        List<Technology> GetTechnologies();
    }
}
