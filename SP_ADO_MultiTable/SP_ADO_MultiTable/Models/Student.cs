using System.ComponentModel.DataAnnotations;
namespace SP_ADO_MultiTable.Models
{
    public class Student
    {
        public int StudentId { get; set; }

        [Required]
        public string StudentName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string MobileNo { get; set; }

        [Required]
        public DateTime DOB { get; set; }

        public string State { get; set; }

        public List<Qualification> Qualifications { get; set; }
    }
}
