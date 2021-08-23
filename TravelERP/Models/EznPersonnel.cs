using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelERP.Models
{
    public class EznPersonnel
    {
        public int Id { get; set; }

        [Display(Name = "اسم الموظف")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [Display(Name = "المرتب الاساسى")]
        public int BasicSalary { get; set; }

        [Display(Name = "قيمه حوافز ثابته")]
        public int Incentives { get; set; }

        [Display(Name = "البدلات")]
        public int Allowances { get; set; }

        [Display(Name = "قيمه الاضافى")]
        public int OverVal { get; set; }

        [Display(Name = "قيمه التأخير")]
        public int LateVal { get; set; }

        [Display(Name = "قيمه الغياب")]
        public int AbsenceVal { get; set; }

        [Display(Name = "التأمينات الاجتماعيه")]
        public int Insurance { get; set; }

        [Display(Name = "العمولات")]
        public int Commissions { get; set; }

        [Display(Name = "سلفيات")]
        public int Loans { get; set; }

    }
}
