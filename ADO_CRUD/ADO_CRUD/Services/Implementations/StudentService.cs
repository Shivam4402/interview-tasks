using ADO_CRUD.Data;
using ADO_CRUD.Models;
using ADO_CRUD.Services.Interfaces;
using Microsoft.Data.SqlClient;
using System.Reflection;

namespace ADO_CRUD.Services.Implementations
{
    public class StudentService : IStudentService
    {

        private readonly DbConnection _dbConnection;
        private readonly  IWebHostEnvironment env;

        public StudentService(DbConnection dbConnection, IWebHostEnvironment env)
        {
            _dbConnection = dbConnection;
            this.env = env;
        }


        public void AddStudent(Student student)
        {
            string fileName = "";

            if (student.ImageFile != null && student.ImageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(env.WebRootPath, "images");

                // Ensure folder exists
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                fileName = Guid.NewGuid().ToString() + Path.GetExtension(student.ImageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    student.ImageFile.CopyTo(stream);
                }
            }


            using (SqlConnection con = _dbConnection.CreateConnection())
            {
                string query = @"INSERT INTO Student
                        (Name,Gender,DOB,QualificationId,Technologies,ImagePath)
                        VALUES
                        (@Name,@Gender,@DOB,@QualificationId,@Technologies,@ImagePath)";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@Name", student.Name);
                cmd.Parameters.AddWithValue("@Gender", student.Gender);
                cmd.Parameters.AddWithValue("@DOB", student.DOB);
                cmd.Parameters.AddWithValue("@QualificationId", student.QualificationId);
                cmd.Parameters.AddWithValue("@Technologies", student.Technologies);
                cmd.Parameters.AddWithValue("@ImagePath", fileName);

                con.Open();
                cmd.ExecuteNonQuery();
            }

        }

        public void DeleteStudent(int id)
        {
            string imagePath = "";

            using (SqlConnection con = _dbConnection.CreateConnection())
            {
                con.Open();

                // 1. Get ImagePath
                string selectQuery = "SELECT ImagePath FROM Student WHERE StudentId=@Id";
                using (SqlCommand cmd = new SqlCommand(selectQuery, con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);


                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        imagePath = result.ToString();
                    }
                }

                // 2. Delete file from server
                if (!string.IsNullOrEmpty(imagePath))
                {
                    string fullPath = Path.Combine(env.WebRootPath, "images", imagePath);

                    if (File.Exists(fullPath))
                    {
                        File.Delete(fullPath);
                    }
                }

                // 3. Delete record from DB
                string deleteQuery = "DELETE FROM Student WHERE StudentId=@Id";
                using (SqlCommand cmd = new SqlCommand(deleteQuery, con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Student> GetAllStudents()
        {
            List<Student> list = new List<Student>();

            using (SqlConnection con = _dbConnection.CreateConnection())
            {
                string query = @"SELECT s.StudentId,
                        s.Name,
                        s.Gender,
                        s.DOB,
                        s.Technologies,
                        s.ImagePath,
                        q.QualificationName
                 FROM Student s
                 INNER JOIN Qualification q
                 ON s.QualificationId = q.QualificationId";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new Student
                    {
                        StudentId = Convert.ToInt32(reader["StudentId"]),
                        Name = reader["Name"].ToString(),
                        Gender = reader["Gender"].ToString(),
                        DOB = Convert.ToDateTime(reader["DOB"]),
                        Technologies = reader["Technologies"].ToString(),
                        ImagePath = reader["ImagePath"].ToString(),
                        QualificationName = reader["QualificationName"].ToString()
                    });
                }
            }

            return list;
        }

        public List<Qualification> GetAllQualifications()
        {
            List<Qualification> list = new List<Qualification>();
            using (SqlConnection con = _dbConnection.CreateConnection())
            {
                string query = "SELECT * FROM Qualification";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new Qualification
                    {
                        QualificationId = Convert.ToInt32(reader["QualificationId"]),
                        QualificationName = reader["QualificationName"].ToString()
                    });
                }
            }
            return list;
        }

        public Student GetStudentById(int id)
        {
            Student student = new Student();

            using (SqlConnection con = _dbConnection.CreateConnection())
            {
                string query = @"SELECT s.StudentId,
                        s.Name,
                        s.Gender,
                        s.DOB,
                        s.Technologies,
                        s.QualificationId,
                        s.ImagePath,
                        q.QualificationName
                 FROM Student s
                 INNER JOIN Qualification q
                 ON s.QualificationId = q.QualificationId 
                 WHERE StudentId=@Id";


                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    student.StudentId = Convert.ToInt32(reader["StudentId"]);
                    student.Name = reader["Name"].ToString();
                    student.Gender = reader["Gender"].ToString();
                    student.DOB = Convert.ToDateTime(reader["DOB"]);
                    student.QualificationId = Convert.ToInt32(reader["QualificationId"]);
                    student.QualificationName = reader["QualificationName"].ToString();
                    student.Technologies = reader["Technologies"].ToString();
                    student.ImagePath = reader["ImagePath"].ToString();
                }
            }


            return student;
        }




        public void UpdateStudent(Student student)
        {
            string fileName = student.ImagePath;

            if (student.ImageFile != null)
            {
                string oldPath = Path.Combine(env.WebRootPath, "images", student.ImagePath);

                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }


                fileName = Guid.NewGuid().ToString() + Path.GetExtension(student.ImageFile.FileName);

                string newPath = Path.Combine(env.WebRootPath, "images", fileName);

                using (var stream = new FileStream(newPath, FileMode.Create))
                {
                    student.ImageFile.CopyTo(stream);
                }

                student.ImagePath = fileName;
            }


            using (SqlConnection con = _dbConnection.CreateConnection())
            {
                string query = @"UPDATE Student
                         SET Name=@Name,
                         Gender=@Gender,
                         DOB=@DOB,
                         QualificationId=@QualificationId,
                         Technologies=@Technologies,
                         ImagePath=@ImagePath
                         WHERE StudentId=@StudentId";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@Name", student.Name);
                cmd.Parameters.AddWithValue("@Gender", student.Gender);
                cmd.Parameters.AddWithValue("@DOB", student.DOB);
                cmd.Parameters.AddWithValue("@QualificationId", student.QualificationId);
                cmd.Parameters.AddWithValue("@Technologies", student.Technologies);
                cmd.Parameters.AddWithValue("@ImagePath", fileName);
                cmd.Parameters.AddWithValue("@StudentId", student.StudentId);

                con.Open();
                cmd.ExecuteNonQuery();
            }

        }
    }
}
