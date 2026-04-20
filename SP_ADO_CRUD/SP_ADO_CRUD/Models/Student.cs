namespace SP_ADO_CRUD.Models
{
    public class Student
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Gender { get; set; }

        public int QualificationId { get; set; }

        public string QualificationName { get; set; }

        public string Technologies { get; set; }

        public string ImagePath { get; set; }

        public IFormFile ImageFile { get; set; }
    }
}
