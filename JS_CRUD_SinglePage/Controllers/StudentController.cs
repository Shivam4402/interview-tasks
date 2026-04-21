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
            var data = (from s in db.Students
                        join st in db.States on s.StateId equals st.StateId
                        join c in db.Cities on s.CityId equals c.CityId
                        select new
                        {
                            s.Id,
                            s.Name,
                            s.Gender,
                            s.Dob,
                            s.Email,
                            s.Technologies,
                            s.ProfileImage,
                            StateName = st.StateName,
                            CityName = c.CityName
                        }).ToList();

            return Json(data);
        }

        public JsonResult GetStudentById(int id)
        {
            return Json(db.Students.Find(id));
        }

        [HttpPost]
        public async Task<JsonResult> AddStudent()
        {
            var form = Request.Form;


            string name = form["Name"];
            string email = form["Email"];
            string gender = form["Gender"];
            string tech = form["Technologies"];

            if (string.IsNullOrEmpty(name))
                return Json(new { success = false, message = "Name is required" });

            if (string.IsNullOrEmpty(email))
                return Json(new { success = false, message = "Email is required" });

            if (!email.Contains("@"))
                return Json(new { success = false, message = "Invalid email format" });

            if (string.IsNullOrEmpty(gender))
                return Json(new { success = false, message = "Select gender" });

            if (string.IsNullOrEmpty(tech))
                return Json(new { success = false, message = "Select at least one technology" });

            // Optional: check duplicate email
            if (db.Students.Any(x => x.Email == email))
                return Json(new { success = false, message = "Email already exists" });

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
        public async Task<IActionResult> UpdateStudent([FromForm] StudentDto dto)
        {
            var st = db.Students.Find(dto.Id);

            if (st == null)
                return Json(false);

            st.Name = dto.Name;
            st.Email = dto.Email;
            st.Gender = dto.Gender;
            st.Dob = dto.Dob;
            st.StateId = dto.StateId;
            st.CityId = dto.CityId;
            st.Technologies = dto.Technologies;

            if (dto.ProfileImage != null)
            {
                string fileName = Guid.NewGuid() + Path.GetExtension(dto.ProfileImage.FileName);
                string path = Path.Combine(env.WebRootPath, "images", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await dto.ProfileImage.CopyToAsync(stream);
                }

                st.ProfileImage = fileName;
            }

            db.SaveChanges();

            return Json(true);
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
