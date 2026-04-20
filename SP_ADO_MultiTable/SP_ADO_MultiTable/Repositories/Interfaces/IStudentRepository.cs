using SP_ADO_MultiTable.Models;

namespace SP_ADO_MultiTable.Repositories.Interfaces
{
    public interface IStudentRepository
    {
        List<Student> GetAllStudents();
        void InsertStudent(Student student);
        void DeleteStudent(int Id);
    }
}
