using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DT102G_WeightApp.Models
{
    public class WeightModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public DateTime Date { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public Decimal Weight { get; set; }
    }
}
