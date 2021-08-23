using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TravelERP.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "من فضلك ادخل رقم الهاتف")]
        [Range(0, int.MaxValue, ErrorMessage = "رقم هاتف غير صحيح")]
        [Display(Name = "رقم الهاتف 1")]
        public string PhoneNumber1 { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "رقم هاتف غير صحيح")]
        [Display(Name = "رقم الهاتف 2")]
        public string PhoneNumber2 { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }

    }
}
