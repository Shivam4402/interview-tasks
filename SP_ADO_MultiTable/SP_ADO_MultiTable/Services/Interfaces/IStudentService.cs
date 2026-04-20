using SP_ADO_MultiTable.Models;

namespace SP_ADO_MultiTable.Services.Interfaces
{
    public interface IStudentService
    {
        void AddStudent(Student student);
        void DeleteStudent(int id);
        List<Student> GetAllStudents();

    }
}
