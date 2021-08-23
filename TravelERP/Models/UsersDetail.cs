using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelERP.Models
{
    public class UsersDetail
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "من فضلك اختر اسم الموظف")]
        [Display(Name = "اسم الموظف")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [Required(ErrorMessage = "من فضلك ادخل قميه العموله")]
        [Display(Name = "العموله")]
        public float Commission { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "اسم الشركه")]
        public int CompanyID { get; set; }
        public Company Company { get; set; }

    }
}
