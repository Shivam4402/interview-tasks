using JS_CRUD_SinglePage.Models;
using Microsoft.AspNetCore.Mvc;

namespace JS_CRUD_SinglePage.Controllers
{
    public class StudentController : Controller
    {
        JsCrudSinglePageContext db;
        IWebHostEnvironment env;

        public StudentController(JsCrudSinglePageContext db, IWebHostEnvironment env)
        {
            this.db = db;
            this.env = env;
        }
        public IActionResult Index()
        {
            return View();
        }

        public JsonResult GetStates()
        {
            return Json(db.States.ToList());
        }

        public JsonResult GetCities(int stateId)
        {
            return Json(db.Cities.Where(c => c.StateId == stateId).ToList());
        }

        public JsonResult GetAllStudents()
        {
            return Json(db.Students.ToList());
        }

        public JsonResult GetStudentById(int id)
        {
            return Json(db.Students.Find(id));
        }

        [HttpPost]
        public async Task<JsonResult> AddStudent()
        {
            var form = Request.Form;

            Student st = new Student();
            st.Name = form["Name"];
            st.Email = form["Email"];
            st.Gender = form["Gender"];
            st.Dob = Convert.ToDateTime(form["Dob"]);
            st.StateId = Convert.ToInt32(form["StateId"]);
            st.CityId = Convert.ToInt32(form["CityId"]);
            st.Technologies = form["Technologies"];

            var file = Request.Form.Files["ProfileImage"];
            if (file != null)
            {
                string folderPath = Path.Combine(env.WebRootPath, "images");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                string fullPath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                st.ProfileImage = fileName;
            }

            db.Students.Add(st);
            db.SaveChanges();

            return Json(new { success = true });
        }


        [HttpPost]
        public JsonResult DeleteStudent(int id)
        {
            var st = db.Students.Find(id);
            db.Students.Remove(st);
            db.SaveChanges();

            return Json(true);
        }

    }
}
