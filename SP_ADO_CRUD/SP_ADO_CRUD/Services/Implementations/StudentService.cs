using Microsoft.Data.SqlClient;
using SP_ADO_CRUD.Models;
using SP_ADO_CRUD.Services.Interfaces;
using System.Data;

namespace SP_ADO_CRUD.Services.Implementations
{
    public class StudentService : IStudentService
    {


        private readonly string _con;

        public StudentService(IConfiguration config)
        {
            _con = config.GetConnectionString("DefaultConnection");
        }

        public List<Student> GetAll()
        {
            var students = new List<Student>();
            var qualifications = GetQualifications();   
            var technologies = GetTechnologies();       

            using SqlConnection con = new(_con);
            using SqlCommand cmd = new("sp_GetStudents", con);
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();
            var dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                var student = new Student
                {
                    Id = (int)dr["Id"],
                    Name = dr["Name"].ToString(),
                    Gender = dr["Gender"].ToString(),
                    QualificationId = (int)dr["QualificationId"],
                    Technologies = dr["Technologies"].ToString(),
                    ImagePath = dr["ImagePath"].ToString(),
                    DOB = Convert.ToDateTime(dr["DOB"])
                };

                student.QualificationName = qualifications
                    .FirstOrDefault(q => q.QualificationId == student.QualificationId)
                    ?.QualificationName;

                var techIds = student.Technologies?.Split(',') ?? new string[] { };

                student.TechnologyNames = string.Join(", ",
                    technologies
                    .Where(t => techIds.Contains(t.TechnologyId.ToString()))
                    .Select(t => t.TechnologyName));

                students.Add(student);
            }

            return students;
        }

        public Student GetById(int id)
        {
            Student model = new();

            using SqlConnection con = new(_con);
            using SqlCommand cmd = new("sp_GetStudentById", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);

            con.Open();
            var dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                model.Id = (int)dr["Id"];
                model.Name = dr["Name"].ToString();
                model.Gender = dr["Gender"].ToString();
                model.QualificationId = (int)dr["QualificationId"];
                model.Technologies = dr["Technologies"].ToString();
                model.ImagePath = dr["ImagePath"].ToString();
                model.DOB = Convert.ToDateTime(dr["DOB"]);
            }

            return model;
        }

        public void Insert(Student m)
        {
            using SqlConnection con = new(_con);
            using SqlCommand cmd = new("sp_InsertStudent", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Name", m.Name);
            cmd.Parameters.AddWithValue("@Gender", m.Gender);
            cmd.Parameters.AddWithValue("@QualificationId", m.QualificationId);
            cmd.Parameters.AddWithValue("@Technologies", m.Technologies);
            cmd.Parameters.AddWithValue("@ImagePath", m.ImagePath);
            cmd.Parameters.AddWithValue("@DOB", m.DOB);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void Update(Student m)
        {
            using SqlConnection con = new(_con);
            using SqlCommand cmd = new("sp_UpdateStudent", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Id", m.Id);
            cmd.Parameters.AddWithValue("@Name", m.Name);
            cmd.Parameters.AddWithValue("@Gender", m.Gender);
            cmd.Parameters.AddWithValue("@QualificationId", m.QualificationId);
            cmd.Parameters.AddWithValue("@Technologies", m.Technologies);
            cmd.Parameters.AddWithValue("@ImagePath", m.ImagePath);
            cmd.Parameters.AddWithValue("@DOB", m.DOB);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using SqlConnection con = new(_con);
            using SqlCommand cmd = new("sp_DeleteStudent", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);

            con.Open();
            cmd.ExecuteNonQuery();
        }


        public List<Qualification> GetQualifications()
        {
            List<Qualification> list = new();

            using SqlConnection con = new(_con);
            using SqlCommand cmd = new("sp_GetQualifications", con);
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();
            var dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                list.Add(new Qualification
                {
                    QualificationId = (int)dr["QualificationId"],
                    QualificationName = dr["QualificationName"].ToString()
                });
            }

            return list;
        }

        public List<Technology> GetTechnologies()
        {
            List<Technology> list = new();

            using SqlConnection con = new(_con);
            using SqlCommand cmd = new("sp_GetTechnologies", con);
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();
            var dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                list.Add(new Technology
                {
                    TechnologyId = (int)dr["TechnologyId"],
                    TechnologyName = dr["TechnologyName"].ToString()
                });
            }

            return list;
        }

    }
}
