using ADO_CRUD.Models;

namespace ADO_CRUD.Services.Interfaces
{
    public interface IStudentService
    {
        List<Student> GetAllStudents();
        Student GetStudentById(int id);
        void AddStudent(Student student);
        void UpdateStudent(Student student);
        void DeleteStudent(int id);
        List<Qualification> GetAllQualifications();

    }
}
