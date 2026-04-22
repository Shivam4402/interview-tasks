using System.ComponentModel.DataAnnotations;

namespace EF_MVC_CRUD.DTOs
{
    public class StudentDto
    {
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Max 50 characters allowed")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please select gender")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Please select state")]
        public int StateId { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date)]
        public DateOnly Dob { get; set; }

        public IFormFile ImageFile { get; set; }

        [Required(ErrorMessage = "Select at least one technology")]
        public List<int> SelectedTechnologies { get; set; }
    }
}
