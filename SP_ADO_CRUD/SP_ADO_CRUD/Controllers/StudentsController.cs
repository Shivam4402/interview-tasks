using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using Microsoft.Data.SqlClient;
using SP_ADO_CRUD.Models;

namespace SP_ADO_CRUD.Controllers
{
    public class StudentsController : Controller
    {

        private readonly IConfiguration _config;

        public StudentsController(IConfiguration config)
        {
            _config = config;
        }


        SqlConnection GetConnection()
        {
            return new SqlConnection(
            _config.GetConnectionString("MyCon"));
        }

        public List<Qualification> GetQualifications()
        {
            List<Qualification> list = new List<Qualification>();

            using (SqlConnection con = GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetQualifications", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    list.Add(new Qualification
                    {
                        QualificationId = Convert.ToInt32(dr["QualificationId"]),
                        QualificationName = dr["QualificationName"].ToString()
                    });
                }
            }

            return list;
        }



        public IActionResult Index()
        {
            List<Student> list = new List<Student>();

            using (SqlConnection con = GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetStudents", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    list.Add(new Student
                    {           
                        Id = Convert.ToInt32(dr["Id"]),
                        Name = dr["Name"].ToString(),
                        Gender = dr["Gender"].ToString(),
                        QualificationName = dr["QualificationName"].ToString(),
                        Technologies = dr["Technologies"].ToString(),
                        ImagePath = dr["ImagePath"].ToString()
                    });
                }
            }

            return View(list);
        }
            
        public IActionResult Create()
        {
            ViewBag.Qualifications = GetQualifications();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Student student, string[] Technologies)
        {
            string fileName = "";

            if (student.ImageFile != null)
            {
                fileName = Path.GetFileName(student.ImageFile.FileName);

                string path = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot/images",
                fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    student.ImageFile.CopyTo(stream);
                }
            }


            string technologies = string.Join(",", Technologies);

            using (SqlConnection con = GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_InsertStudent", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Name", student.Name);
                cmd.Parameters.AddWithValue("@Gender", student.Gender);
                cmd.Parameters.AddWithValue("@QualificationId", student.QualificationId);
                cmd.Parameters.AddWithValue("@Technologies", technologies);
                cmd.Parameters.AddWithValue("@ImagePath", fileName);

                con.Open();

                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            Student student = new Student();

            using (SqlConnection con = GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetStudentById", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    student.Id = Convert.ToInt32(dr["Id"]);
                    student.Name = dr["Name"].ToString();
                    student.Gender = dr["Gender"].ToString();
                    student.QualificationName = dr["QualificationName"].ToString();
                    student.QualificationId = Convert.ToInt32(dr["QualificationId"]);
                    student.Technologies = dr["Technologies"].ToString();
                    student.ImagePath = dr["ImagePath"].ToString();
                }
            }

            ViewBag.Qualifications = GetQualifications();

            return View(student);
        }

        [HttpPost]
        public IActionResult Edit(Student student, string[] Technologies)
        {
            string fileName = student.ImagePath;

            if (student.ImageFile != null)
            {
                // delete old image
                string oldPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot/images",
                student.ImagePath); 

                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }

                // upload new image
                fileName = Path.GetFileName(student.ImageFile.FileName);

                string path = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot/images",
                fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    student.ImageFile.CopyTo(stream);
                }
            }


            string technologies = string.Join(",", Technologies);

            using (SqlConnection con = GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateStudent", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", student.Id);
                cmd.Parameters.AddWithValue("@Name", student.Name);
                cmd.Parameters.AddWithValue("@Gender", student.Gender);
                cmd.Parameters.AddWithValue("@QualificationId", student.QualificationId);
                cmd.Parameters.AddWithValue("@Technologies", technologies);
                cmd.Parameters.AddWithValue("@ImagePath", fileName);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {

            Student student = new Student();

            using (SqlConnection con = GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetStudentById", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    student.Id = Convert.ToInt32(dr["Id"]);
                    student.Name = dr["Name"].ToString();
                    student.Gender = dr["Gender"].ToString();
                    student.QualificationName = dr["QualificationName"].ToString();
                    student.QualificationId = Convert.ToInt32(dr["QualificationId"]);
                    student.Technologies = dr["Technologies"].ToString();
                    student.ImagePath = dr["ImagePath"].ToString();
                }
            }

            ViewBag.Qualifications = GetQualifications();

            return View(student);
        }
        public IActionResult Delete(int id)
        {
            using (SqlConnection con = GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteStudent", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }


    }
}
