using SP_ADO_CRUD.Models;
using System.ComponentModel.DataAnnotations;

namespace SP_ADO_CRUD.ViewModels
{
    public class StudentViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Select gender")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Select qualification")]
        public int QualificationId { get; set; }

        public List<int> SelectedTechnologies { get; set; } = new();

        public List<Qualification> Qualifications { get; set; }
        public List<Technology> Technologies { get; set; }

        public string ImagePath { get; set; }

        [Required(ErrorMessage = "DOB is required")]
        [DataType(DataType.Date)]
        public DateTime? DOB { get; set; }
    }
}
