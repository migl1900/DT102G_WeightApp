using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DT102G_WeightApp.Models
{
    // Viewmodel enabling working with multiple models
    public class WeightViewModel
    {
        public int WeightId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Date { get; set; }

        [Required]
        public decimal Weight { get; set; }
        public string Comment { get; set; }
    }
}
