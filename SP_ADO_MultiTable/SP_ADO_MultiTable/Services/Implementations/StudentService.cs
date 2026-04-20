using SP_ADO_MultiTable.Models;
using SP_ADO_MultiTable.Repositories.Interfaces;
using SP_ADO_MultiTable.Services.Interfaces;

namespace SP_ADO_MultiTable.Services.Implementations
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository repository;
        public StudentService(IStudentRepository repository)
        {
            this.repository = repository;
        }
        public void AddStudent(Student student)
        {
            repository.InsertStudent(student);

        }
        public void DeleteStudent(int id)
        {
            repository.DeleteStudent(id);
        }

        public List<Student> GetAllStudents()
        {
            return repository.GetAllStudents();
        }
    }
}
