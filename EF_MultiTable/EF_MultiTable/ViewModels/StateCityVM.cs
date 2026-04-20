using System.ComponentModel.DataAnnotations;

namespace EF_MultiTable.ViewModels
{
    public class StateCityVM
    {
        public int StateId { get; set; }

        [Required]
        public string StateName { get; set; }

        [Required]
        public List<CityVM> Cities { get; set; } = new List<CityVM>();
    }
}
