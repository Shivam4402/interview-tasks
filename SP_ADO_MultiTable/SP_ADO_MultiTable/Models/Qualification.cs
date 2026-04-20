using System.ComponentModel.DataAnnotations;

namespace SP_ADO_MultiTable.Models
{
    public class Qualification
    {
        public int QualificationId { get; set; }

        [Required]
        public string QualificationName { get; set; }

        [Required]
        public int PassingYear { get; set; }

        [Required]
        public decimal Percentage { get; set; }

        [Required]
        public string University { get; set; }
    }
}
