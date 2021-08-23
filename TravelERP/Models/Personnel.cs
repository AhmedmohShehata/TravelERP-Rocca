using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelERP.Models
{
    public class Personnel
    {
        public int Id { get; set; }

        [Display(Name = "اسم الموظف")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [Display(Name = "الاسم بجهاز البصمه")]
        public int? FingerPrintName { get; set; }

        [Display(Name = "المرتب الاساسى")]
        public int BasicSalary { get; set; }

        [Display(Name = "قيمه حوافز ثابته")]
        public int Incentives { get; set; }

        [Display(Name = "البدلات")]
        public int Allowances { get; set; }

        [Display(Name = "قيمه ساعه الاضافى")]
        public int OverHrVal { get; set; }

        [Display(Name = "قيمه ساعه التأخير")]
        public int LateHrVal { get; set; }

        [Display(Name = "قيمه ساعه الغياب")]
        public int AbsenceHrVal { get; set; }

        [Display(Name = "التأمينات الاجتماعيه")]
        public int Insurance { get; set; }

        [Display(Name = "العمولات")]
        public int Commissions { get; set; }


    }
}
