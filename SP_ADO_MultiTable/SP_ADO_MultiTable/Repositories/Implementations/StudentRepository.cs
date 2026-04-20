using Microsoft.Data.SqlClient;
using SP_ADO_MultiTable.Data;
using SP_ADO_MultiTable.Models;
using SP_ADO_MultiTable.Repositories.Interfaces;
using System.Data;

namespace SP_ADO_MultiTable.Repositories.Implementations
{
    public class StudentRepository : IStudentRepository
    {
        public readonly DbConnection _dbCon;

        public StudentRepository(DbConnection dbCon)
        {
            _dbCon = dbCon;
        }
        public void InsertStudent(Student student)
        {
            int studentId = 0;

            using (SqlConnection con = _dbCon.CreateConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_InsertStudent", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@StudentName", student.StudentName);
                cmd.Parameters.AddWithValue("@Email", student.Email);
                cmd.Parameters.AddWithValue("@MobileNo", student.MobileNo);
                cmd.Parameters.AddWithValue("@DOB", student.DOB);
                cmd.Parameters.AddWithValue("@State", student.State);

                con.Open();

                studentId = Convert.ToInt32(cmd.ExecuteScalar());
            }

            foreach (var q in student.Qualifications)
            {
                using (SqlConnection con = _dbCon.CreateConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_InsertQualification", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@StudentId", studentId);
                    cmd.Parameters.AddWithValue("@QualificationName", q.QualificationName);
                    cmd.Parameters.AddWithValue("@PassingYear", q.PassingYear);
                    cmd.Parameters.AddWithValue("@Percentage", q.Percentage);
                    cmd.Parameters.AddWithValue("@University", q.University);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteStudent(int id)
        {
            using (SqlConnection con = _dbCon.CreateConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteStudent", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@StudentId", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }


        public List<Student> GetAllStudents()
        {
            List<Student> students = new List<Student>();

            using (SqlConnection con = _dbCon.CreateConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetAllStudents", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int studentId = Convert.ToInt32(reader["StudentId"]);

                    var student = students.FirstOrDefault(s => s.StudentId == studentId);

                    if (student == null)
                    {
                        student = new Student
                        {
                            StudentId = studentId,
                            StudentName = reader["StudentName"].ToString(),
                            Email = reader["Email"].ToString(),
                            MobileNo = reader["MobileNo"].ToString(),
                            DOB = Convert.ToDateTime(reader["DOB"]),
                            State = reader["State"].ToString(),
                            Qualifications = new List<Qualification>()
                        };

                        students.Add(student);
                    }

                    if (reader["QualificationId"] != DBNull.Value)
                    {
                        student.Qualifications.Add(new Qualification
                        {
                            QualificationId = Convert.ToInt32(reader["QualificationId"]),
                            QualificationName = reader["QualificationName"].ToString(),
                            PassingYear = Convert.ToInt32(reader["PassingYear"]),
                            Percentage = Convert.ToDecimal(reader["Percentage"]),
                            University = reader["University"].ToString()
                        });
                    }
                }
            }

            return students;
        }
    }
}
