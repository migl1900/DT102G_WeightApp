using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace DT102G_WeightApp.Models
{
    // Custom values in user database
    public class ApplicationUser : IdentityUser
    {
        public decimal? StartWeight { get; set; }
        public decimal? TargetWeight { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? TargetDate { get; set; }
        public byte[] ProfilePicture { get; set; }
    }
}
