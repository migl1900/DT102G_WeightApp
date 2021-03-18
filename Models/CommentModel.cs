using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DT102G_WeightApp.Models
{
    public class CommentModel
    {
        public int Id { get; set; }
        public int WeightModelId { get; set; }
        public string Comment { get; set; }
        public WeightModel WeightModel { get; set; }
    }
}
