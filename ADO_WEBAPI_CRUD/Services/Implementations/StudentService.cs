using ADO_WEBAPI_CRUD.Models;
using ADO_WEBAPI_CRUD.Services.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ADO_WEBAPI_CRUD.Services.Implementations
{
    public class StudentService: IStudentService
    {
        private readonly IConfiguration _config;
        private readonly string _conn;

        public StudentService(IConfiguration config)
        {
            _config = config;
            _conn = _config.GetConnectionString("DefaultConnection");
        }

        public async Task<List<Student>> GetAll()
        {
            var list = new List<Student>();

            using (SqlConnection con = new SqlConnection(_conn))
            using (SqlCommand cmd = new SqlCommand("sp_GetStudents", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                await con.OpenAsync();

                var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(new Student
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"].ToString(),
                        Email = reader["Email"].ToString(),
                        ImagePath = reader["ImagePath"].ToString()
                    });
                }
            }

            return list;
        }

        public async Task<Student> GetById(int id)
        {
            Student student = null;

            using (SqlConnection con = new SqlConnection(_conn))
            using (SqlCommand cmd = new SqlCommand("sp_GetStudentById", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);

                await con.OpenAsync();
                var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    student = new Student
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"].ToString(),
                        Email = reader["Email"].ToString(),
                        ImagePath = reader["ImagePath"].ToString()
                    };
                }
            }

            return student;
        }

        public async Task<int> Create(Student student)
        {
            using (SqlConnection con = new SqlConnection(_conn))
            using (SqlCommand cmd = new SqlCommand("sp_AddStudent", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Name", student.Name);
                cmd.Parameters.AddWithValue("@Email", student.Email);
                cmd.Parameters.AddWithValue("@ImagePath", student.ImagePath ?? (object)DBNull.Value);

                await con.OpenAsync();
                var result = await cmd.ExecuteScalarAsync();

                return Convert.ToInt32(result);
            }
        }

        public async Task<bool> Update(Student student)
        {
            using (SqlConnection con = new SqlConnection(_conn))
            using (SqlCommand cmd = new SqlCommand("sp_UpdateStudent", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", student.Id);
                cmd.Parameters.AddWithValue("@Name", student.Name);
                cmd.Parameters.AddWithValue("@Email", student.Email);
                cmd.Parameters.AddWithValue("@ImagePath", student.ImagePath ?? (object)DBNull.Value);

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                return true;
            }
        }

        public async Task<bool> Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(_conn))
            using (SqlCommand cmd = new SqlCommand("sp_DeleteStudent", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                return true;
            }
        }
    }
}
