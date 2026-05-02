namespace SP_ADO_CRUD.DTOs
{
    public class StudentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public int QualificationId { get; set; }
        public string Technologies { get; set; } // CSV
        public string ImagePath { get; set; }
        public DateTime? DOB { get; set; }
    }
}
