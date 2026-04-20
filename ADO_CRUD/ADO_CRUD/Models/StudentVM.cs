namespace ADO_CRUD.Models
{
    public class StudentVM
    {
        //public int StudentId { get; set; }

        public string Name { get; set; }
        public string Gender { get; set; }
        public DateTime DOB { get; set; }
        public int QualificationId { get; set; }

        public string[] Technologies { get; set; }

        public IFormFile ImageFile { get; set; }

        //public string ExistingImagePath { get; set; }
    }
}
